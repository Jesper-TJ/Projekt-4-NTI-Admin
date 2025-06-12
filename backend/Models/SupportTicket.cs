using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace AdminApi.Models;

public class SupportTicket
{
    public long Id { get; set; }
    [Required]
    public long UserId { get; set; }
    [Required]
    public User User { get; set; } = null!;
    [Required]
    public long JanitorId { get; set; }
    [Required]
    public User Janitor { get; set; } = null!;

    public ICollection<SupportTicketMessage> SupportTicketMessages { get; set; } = new List<SupportTicketMessage>();

    public int Status { get; set; }

    public int IssueCategory { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
