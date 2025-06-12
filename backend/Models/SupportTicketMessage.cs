using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace AdminApi.Models;

public class SupportTicketMessage
{
    public long Id { get; set; }
    [Required]
    public long UserId { get; set; }
    [Required]
    public User User { get; set; } = null!;
    [Required]
    public long SupportTicketId { get; set; }
    [Required]
    public SupportTicket SupportTicket { get; set; } = null!;

    public string Message { get; set; } = "";

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
