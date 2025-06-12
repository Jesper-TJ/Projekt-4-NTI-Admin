using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace AdminApi.Models;

public class Review
{
    public long Id { get; set; }
    [Required]
    public long UserId { get; set; }
    [Required]
    public User User { get; set; } = null!;
    [Required]
    public long BookTitleId { get; set; }
    [Required]
    public BookTitle BookTitle { get; set; } = null!;

    public string Content { get; set; } = "";

    public double Rating { get; set; }
    public bool Anonymous { get; set; } = false;

    public DateTime CreatedAt { get; set; }
}
