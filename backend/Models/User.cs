using System.Collections;
using NuGet.Protocol.Plugins;

namespace AdminApi.Models;

public class User
{
    public long Id { get; set; }

    public string Name { get; set; } = "";

    public string Email { get; set; } = "";
    public string Klass { get; set; } = ""; // Student class, EX TE23A, For staff set Personal
    public string AccessCardImagePath { get; set; } = "";
    public ICollection<ComputerLog> ComputerLogs { get; set; } = new List<ComputerLog>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<BookLoan> BookLoans { get; set; } = new List<BookLoan>();
    public ICollection<SupportTicket> SupportTickets { get; set; } = new List<SupportTicket>();
    public ICollection<SupportTicketMessage> SupportTicketMessages { get; set; } = new List<SupportTicketMessage>();
    public BitArray Roles { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
