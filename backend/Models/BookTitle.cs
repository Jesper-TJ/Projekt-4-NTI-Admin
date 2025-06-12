namespace AdminApi.Models;

public class BookTitle
{
    public long Id { get; set; }
    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string Description { get; set; } = "";
    public string ISBN { get; set; } = "";
    public bool CourseBook { get; set; } = false;
    public ICollection<Book> Books { get; set; } = new List<Book>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<BookGenreBookTitle> BookGenreBookTitles { get; set; } = new List<BookGenreBookTitle>();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
