using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AdminApi.Models;

public class BookLoan
{
    public long Id { get; set; }
    [Required]
    public long UserId { get; set; }
    [Required]
    public User User { get; set; } = null!;
    [Required]
    public long BookId { get; set; }
    [Required]
    public Book Book { get; set; } = null!;

    public DateTime ReturnDate { get; set; }
    public DateTime? ReturnedAt { get; set; } = null;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
