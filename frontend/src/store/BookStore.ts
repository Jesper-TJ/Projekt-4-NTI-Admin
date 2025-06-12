import { defineStore } from "pinia";
import { DataTableFetchOpts } from "@/types/VuetifyTypes";
import { BookTitlePreview, BookTitlePreviewResponse } from "@/types/BookTitle";
import { availableParallelism } from "os";
import { defaultApiURL } from "@/config";

export interface IComment {
  content: string;
  createdAt: string;
  rating: number;
  id: number;
  userName: string;
  anonymous: boolean;
}

// export interface IBook {
//   title: string;
//   description: string;
//   author: string;
//   isbn: number;
//   genres: string[];
//   comments: IComment[];
// }

export interface IBook {
  id: number;
  title: string;
  description: string;
  author: string;
  isbn: string;
  totalBooks: number;
  availableBooks: number;
  books: {id: string; barCode: number; isAvailable: boolean; status: string;}
  rating: number;
  reviews: IComment[];
}

export type BookLoanPreview = {
  id: number;
  title: string;
  returnAt: string;
  returnDate: string;
  createdAt: string;
  status: string;
};

export const useBookStore = defineStore("BookStore", {
  state: () => {
    return {
      books: [] as IBook[],
      book: null as IBook | null,
      loanedBooks: [] as BookLoanPreview[],
    };
  },
  actions: {
    async borrowABook(bookId: number, userId: number) {
      try {
        const response = await fetch(
          `${defaultApiURL}/api/Books/borrow/${bookId}`,
          {
            method: "POST",
            headers: {
              "Content-Type": "application/json",
            },
            body: JSON.stringify(userId),
          },
        );

        if (response.ok) {
            const updatedBook = await response.json();
            this.books = this.books.map(book =>
                book.id === bookId ? { ...book, availableBooks: book.availableBooks - 1 } : book
            );
            console.log("Book borrowed successfully:", updatedBook);
        } else {
          console.error("Failed to borrow book.");
        }
      } catch (error) {
        console.error("Error borrowing book:", error);
      }
    },

    async fetchBooks() {
      try {
        const response = await fetch(`${defaultApiURL}/api/Books/all`, {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
          },
        });

        if (response.ok) {
          const data = await response.json();
          console.log("Fetched data:", data);
          this.books = data; // Update books in store
        } else {
          console.warn("API call failed, using default values.");
        }
      } catch (error) {
        console.error("Failed to fetch books:", error);

        console.log("Going with hardcoded data");

        // this.books = [
        //   {
        //     title: "The Hitchhiker's Guide to the Galaxy",
        //     description: `The Hitchhiker's Guide to the Galaxy is the first book in the
        //           Hitchhiker's Guide to the Galaxy comedy science fiction "trilogy of
        //           five books" by Douglas Adams, with a sixth book written by Eoin
        //           Colfer. The novel is an adaptation of the first four parts of Adams's
        //           radio series of the same name, centering on the adventures of the only
        //           man to survive the destruction of Earth; while roaming outer space, he
        //           comes to learn the truth behind Earth's existence.`,
        //     author: "Douglas Adams",
        //     isbn: 1337,
        //     // genres: ["science fiction", "comedy"],
        //     // comments: [
        //     //   { userName: "Jason", rating: "1/5", commentText: "I hate it" },
        //     //   { userName: "Mason", rating: "2/5", commentText: "It aight" },
        //     //   {
        //     //     userName: "Chason",
        //     //     rating: "6/5",
        //     //     commentText: "I quite enjoy reading this from time to time",
        //     //   },
        //     // ],
        //   },
        //   {
        //     title: "To Kill a Mockingbird",
        //     description: `Harper Lee's To Kill a Mockingbird is a classic novel of modern American literature. It explores themes of racial injustice, moral growth, and empathy.`,
        //     author: "Harper Lee",
        //     isbn: 9780061120084,
        //     genres: ["classic", "fiction"],
        //     comments: [
        //       { userName: "Sarah", rating: "5/5", commentText: "A must-read." },
        //       {
        //         userName: "John",
        //         rating: "4/5",
        //         commentText: "Very insightful.",
        //       },
        //     ],
        //   },
        //   {
        //     title: "1984",
        //     description: `George Orwell's dystopian novel 1984 depicts a totalitarian society controlled by Big Brother, highlighting themes of surveillance and freedom.`,
        //     author: "George Orwell",
        //     isbn: 9780451524935,
        //     genres: ["dystopian", "political fiction"],
        //     comments: [
        //       {
        //         userName: "Emma",
        //         rating: "5/5",
        //         commentText: "Eerily relevant.",
        //       },
        //       {
        //         userName: "Liam",
        //         rating: "4/5",
        //         commentText: "Thought-provoking.",
        //       },
        //     ],
        //   },
        // ];
      }

    },
    async fetchLoanedBooks(userId: number) {
      try {
        const response = await fetch(`${defaultApiURL}/api/Books/${userId}`, {
          method: "GET",
          headers: {
            "Content-Type": "application/json",
          },
        });

        if (response.ok) {
          const data = await response.json();
          console.log("Fetched data:", data);
          this.loanedBooks = data; // Update books in store
        } else {
          console.warn("API call failed, using default values.");
        }
      } catch (error) {
        console.error("Failed to fetch books:", error);
      }
    },
    async postReview(review: {}, bookTitleId: number): Promise<number>{

      try {
        const response = await fetch(`${defaultApiURL}/api/Books/new/review/${bookTitleId}`, {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(review),
        });

        if (!response.ok) {
          throw new Error(`Failed to post review: ${response.statusText}`);
        }

        const result = await response.json();
        console.log("Review successfully posted:", result);
        return 0;
      } catch (error) {
        console.error("Error posting review:", error);
        return 1;
      }

    },
    addComment(bookIsbn: string, comment: IComment) {
      const book = this.books.find((b) => b.isbn === bookIsbn);
      if (book) {
        book.reviews?.push(comment);
      }
    },
  },
  getters: {
    allBooks(): IBook[] {
      return this.books;
    },
    bookByIsbn:
      (state) =>
      (isbn: string): IBook | undefined => {
        return state.books.find((book) => book.isbn === isbn);
      },
    chosenBook:
    (state) =>
      (bookid: number): BookTitlePreview | undefined => {{
      return state.books.find((title) => title.id === bookid)
    }},
    chosenBookInfo:
    (state) =>
      (bookid: number): IBook | undefined => {{
      return state.books.find((book) => book.id === bookid)
    }},
    allLoanedBooks(): BookLoanPreview[] {
      return this.loanedBooks;
    },
  },
  },
);
