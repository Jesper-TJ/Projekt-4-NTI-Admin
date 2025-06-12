using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using AdminApi.Models;
using AdminApi.GoogleApi;
using System.Text.Json;
using System.Collections;


namespace AdminApi
{
    public static class Seeder
    {
        private static List<User> savedUsers = new List<User>();

        public static void Seed(AdminContext context, int seedType)
        {
            ValidateSeedType(seedType);

            if (seedType == 0)
            {
                SaveExistingUsers(context, seedType);
            }

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            SeedData(context, seedType);
        }

        private static void ValidateSeedType(int seedType)
        {
            if (seedType < 0 || seedType > 2)
            {
                throw new ArgumentOutOfRangeException(nameof(seedType), "Invalid seed type");
            }
        }

        private static void SaveExistingUsers(AdminContext context, int seedType)
        {
            if (seedType != 1)
            {
                savedUsers = context.Users.ToList();
            }
        }

        private static List<User> FetchUsersFromGoogleAPI()
        {
            var allUsers = GoogleUsersApiRequest.Main();
            var newUsers = new List<User>();

            Console.Write("Enter Admin Email: ");
            string? input = Console.ReadLine();

            foreach (var userElement in allUsers.RootElement.EnumerateArray())
            {
                string? primaryEmail = "";
                if (userElement.TryGetProperty("primaryEmail", out JsonElement emailElement))
                {
                    primaryEmail = emailElement.GetString() ?? "unknown";
                }
                var fullName = userElement.GetProperty("name").GetProperty("fullName").GetString();
                var created = userElement.GetProperty("creationTime").GetDateTime();

                bool isStaff = false;
                bool isStudent = false;

                string klass = "";
                string? orgUnitPath = userElement.GetProperty("orgUnitPath").GetString();
                if (orgUnitPath?.Contains("Personal") == true)
                {
                    isStaff = true;
                    klass = "Personal";
                }
                else if (orgUnitPath?.Contains("Elever") == true)
                {
                    isStudent = true;
                    klass = Path.GetFileName(orgUnitPath.TrimEnd('/'));
                    
                }

                bool isLibrarian = false;
                bool isJanitor = false;
                bool isHeadmaster = false;
                if (Environment.GetEnvironmentVariable("LIBRARIAN_PRIMARYEMAIL") == primaryEmail)
                {
                    isLibrarian = true;
                }
                if (Environment.GetEnvironmentVariable("JANITOR_PRIMARYEMAIL") == primaryEmail)
                {
                    isJanitor = true;
                }
                if (Environment.GetEnvironmentVariable("HEADMASTER_PRIMARYEMAIL") == primaryEmail)
                {
                    isHeadmaster = true;
                }

                bool isAdmin = false;
                if (primaryEmail == input)
                {
                    isAdmin = true;
                }


                var newUser = new User
                {
                    Name = fullName ?? "Unknown",
                    Email = primaryEmail,
                    Roles = new BitArray(new[] { false, isStudent, isStaff, isLibrarian, isJanitor, isHeadmaster, isAdmin }),
                    Klass = klass,
                    AccessCardImagePath = "",
                    CreatedAt = created,
                    UpdatedAt = DateTime.UtcNow
                };

                newUsers.Add(newUser);
            }
            return newUsers;
        }

        private static List<User> GetFakeUsers()
        {
            return new List<User>
            {
            new User
            {
                Name = "John Doe",
                Email = "john.doe@example.com",
                Klass = "TE23A",
                AccessCardImagePath = "",
                Roles = new BitArray(new[] { true, false, false }), // Deleted user
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Name = "Jane Smith",
                Email = "jane.smith@example.com",
                Klass = "TE23A",
                AccessCardImagePath = "",
                Roles = new BitArray(new[] { false, true, false }), // Student
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Name = "Jörgen Knapare",
                Email = "jorgen.knapare@ga.ntig.se",
                Klass = "Personal",
                AccessCardImagePath = "",
                Roles = new BitArray(new[] { false, false, true, false, true }), // Janitor and staff
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Name = "Anneli Bok",
                Email = "anneli.bok@ga.ntig.se",
                Klass = "Personal",
                AccessCardImagePath = "",
                Roles = new BitArray(new[] { false, false, true, true }), // Librarian and staff
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Name = "Erik Johansson",
                Email = "erik.johansson@ga.ntig.se",
                Klass = "Personal",
                AccessCardImagePath = "",
                Roles = new BitArray(new[] { false, false, true }), // Staff
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Name = "Sara Lindgren",
                Email = "sara.lindgren@ga.ntig.se",
                Klass = "Personal",
                AccessCardImagePath = "",
                Roles = new BitArray(new[] { false, false, true }), //  staff
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Name = "David Nilsson",
                Email = "david.nilsson@ga.ntig.se",
                Klass = "EE23B",
                AccessCardImagePath = "",
                Roles = new BitArray(new[] { false, true, false, false }), // Student
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Name = "Mikaela Pettersson",
                Email = "mikaela.pettersson@ga.ntig.se",
                Klass = "Personal",
                AccessCardImagePath = "",
                Roles = new BitArray(new[] { false, false, true, true, true }), // Staff, Librarian, Janitor
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            },
            new User
            {
                Name = "Lola Gorgonzola",
                Email = "lola.gorgonzola@ga.ntig.se",
                Klass = "Personal",
                AccessCardImagePath = "",
                Roles = new BitArray(new[] { false, false, true, false, false, true }), // Headmaster and staff
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }
        };
        }

        private static void SeedData(AdminContext context, int seedType)
        {
            var users = seedType switch
            {
                0 => savedUsers,
                1 => FetchUsersFromGoogleAPI(),
                2 => GetFakeUsers(),
                _ => throw new ArgumentOutOfRangeException(nameof(seedType), "Invalid seed type")
            };

            context.Database.EnsureCreated();

            AddUsersToDatabase(context, users);
            AddComputersToDatabase(context);
            AddBookTitlesToDatabase(context);
            List<Book> books = AddBooksToDatabase(context);
            AddBookGenresToDatabase(context);
            AddBookGenresBookTitlesToDatabase(context);
            AddComputerLoansToDatabase(context, users);
            AddBookLoansToDatabase(context, users, books);
            AddReviewsToDatabase(context, users);
            AddSupportTicketsToDatabase(context, users);
            SupportTicketMessages(context, users);
        }

        private static void AddUsersToDatabase(AdminContext context, List<User> users)
        {
            if (users.Any())
            {
                context.Users.AddRange(users);
                context.SaveChanges();
                Console.WriteLine($"{users.Count} users have been added to the database.");
            }
            else
            {
                Console.WriteLine("No users found.");
            }
        }

        private static void AddComputersToDatabase(AdminContext context)
        {
            var computers = new List<Computer>
            {
                new Computer { Name = "Dell XPS 13", Serial = "DXPS-001", Status = "Available", EntryDate = DateTime.UtcNow},
                new Computer { Name = "MacBook Pro 16", Serial = "MBP-002", Status = "In Use", EntryDate = DateTime.UtcNow}
            };

            context.Computers.AddRange(computers);
            context.SaveChanges();
        }

        private static void AddBookTitlesToDatabase(AdminContext context)
        {
            var bookTitles = new List<BookTitle>
            {
                new BookTitle { Title = "C# in Depth", Description = "A comprehensive guide to C#.", ISBN = "9781617294532", CourseBook = true, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new BookTitle { Title = "Entity Framework Core in Action", Description = "Best practices for working with EF Core.", ISBN = "9781617299421", CourseBook = false, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };

            context.BookTitles.AddRange(bookTitles);
            context.SaveChanges();
        }

        private static List<Book> AddBooksToDatabase(AdminContext context)
        {
            var books = new List<Book>
            {
                new Book { BookTitleId = 1, Status = "Broken", BarCode = "111111", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Book { BookTitleId = 2, Status = "Fine", BarCode = "222222", CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };

            context.Books.AddRange(books);
            context.SaveChanges();
            return books;
        }

        private static void AddBookGenresToDatabase(AdminContext context)
        {
            var bookGenres = new List<BookGenre>
            {
                new BookGenre { name = "Science Fiction" },
                new BookGenre { name = "Fantasy" },
                new BookGenre { name = "Mystery" },
                new BookGenre { name = "Romance" },
                new BookGenre { name = "Horror" },
                new BookGenre { name = "Historical Fiction" },
                new BookGenre { name = "Non-Fiction" },
                new BookGenre { name = "Thriller" },
                new BookGenre { name = "Biography" },
            };

            context.BookGenres.AddRange(bookGenres);
            context.SaveChanges();
        }

        private static void AddBookGenresBookTitlesToDatabase(AdminContext context)
        {
            var bokGenresBookTitles = new List<BookGenreBookTitle>
            {
                new BookGenreBookTitle {
                    BookGenreId = 1,
                    BookTitleId = 1,
                },
                new BookGenreBookTitle {
                    BookGenreId = 2,
                    BookTitleId = 1,
                },
                new BookGenreBookTitle {
                    BookGenreId = 3,
                    BookTitleId = 1,
                },
                new BookGenreBookTitle {
                    BookGenreId = 4,
                    BookTitleId = 1,
                },
                new BookGenreBookTitle {
                    BookGenreId = 5,
                    BookTitleId = 1,
                },
            };

            context.BookGenresBookTitles.AddRange(bokGenresBookTitles);
            context.SaveChanges();
        }

        private static void AddComputerLoansToDatabase(AdminContext context, List<User> users)
        {
            var Computerlogs = new List<ComputerLog>
            {
                new ComputerLog
                {
                    UserId = users[0].Id,
                    ComputerId = 1,
                    ReturnDate = DateTime.UtcNow.AddDays(30),
                    ReturnedAt = null,
                    EntryDate = DateTime.UtcNow
                },
                new ComputerLog
                {
                    UserId = users[1].Id,
                    ComputerId = 2,
                    ReturnDate = DateTime.UtcNow.AddDays(15),
                    ReturnedAt = DateTime.UtcNow.AddDays(14),
                    EntryDate = DateTime.UtcNow
                }
            };

            context.ComputerLogs.AddRange(Computerlogs);
            context.SaveChanges();
        }

        private static void AddBookLoansToDatabase(AdminContext context, List<User> users, List<Book> books)
        {
            var BookLoans = new List<BookLoan>
            {
                new BookLoan
                {
                    UserId = users[0].Id,
                    BookId = 1,
                    ReturnDate = DateTime.UtcNow.AddDays(15),
                    ReturnedAt = null,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new BookLoan
                {
                    UserId = users[0].Id,
                    BookId = books[1].Id,
                    ReturnDate = DateTime.UtcNow.AddDays(30),
                    ReturnedAt = DateTime.UtcNow.AddDays(-1),
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow
                },
                new BookLoan
                {
                    UserId = users[0].Id,
                    BookId = books[1].Id,
                    ReturnDate = DateTime.UtcNow.AddDays(30),
                    ReturnedAt = DateTime.UtcNow.AddDays(-1),
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    UpdatedAt = DateTime.UtcNow
                }

            };

            context.BookLoans.AddRange(BookLoans);
            context.SaveChanges();
        }

        private static void AddReviewsToDatabase(AdminContext context, List<User> users)
        {
            var reviews = new List<Review>
            {
                new Review { UserId = users[0].Id, BookTitleId = 1, Content = "A good book about C#", Rating = 5, CreatedAt = DateTime.UtcNow },
                new Review { UserId = users[1].Id, BookTitleId = 2, Content = "A good book about best practices", Rating = 5, CreatedAt = DateTime.UtcNow }
            };

            context.Reviews.AddRange(reviews);
            context.SaveChanges();
        }

        private static void AddSupportTicketsToDatabase(AdminContext context, List<User> users)
        {
            var supportTickets = new List<SupportTicket>
            {
                new SupportTicket { UserId = users[0].Id, JanitorId = users[1].Id, Status = 1, IssueCategory = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            };

            context.SupportTickets.AddRange(supportTickets);
            context.SaveChanges();
        }

        private static void SupportTicketMessages(AdminContext context, List<User> users)
        {
            var supportTicketMessages = new List<SupportTicketMessage>
            {
                new SupportTicketMessage
                {
                    UserId = users[0].Id,
                    SupportTicketId = 1,
                    Message = "Hello my computer is broken",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            context.SupportTicketMessages.AddRange(supportTicketMessages);
            context.SaveChanges();
        }
    }
}
