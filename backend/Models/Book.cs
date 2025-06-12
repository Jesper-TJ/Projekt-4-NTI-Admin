using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace AdminApi.Models;

public class Book
{
    public long Id { get; set; }
    [Required]
    public long BookTitleId { get; set; }
    [Required]
    public BookTitle BookTitle { get; set; } = null!;
    [Required]

    public ICollection<BookLoan> BookLoans { get; set; } = new List<BookLoan>();

    public string Status { get; set; } = "";

    public string BarCode { get; set; } = "";

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
