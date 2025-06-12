using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AdminApi.Models;

public class Computer
{
	public long Id { get; set; }
	public string Name { get; set; } = string.Empty; // Matches IComputer.name
	public string Serial { get; set; } = string.Empty; // Matches IComputer.serial
	public string Status { get; set; } = string.Empty; // Matches IComputer.status
	public string Damage { get; set; } = string.Empty; // Matches IComputer.damage
	public DateTime EntryDate { get; set; } // Matches IComputer.entry_date
	public ICollection<ComputerLog> ComputerLogs { get; set; } = new List<ComputerLog>();
}

public class ComputerLog
{
	public long Id { get; set; }
	[Required]
	public long UserId { get; set; }
	[Required]
	public string UserName { get; set; } = string.Empty;
	[Required]
	public long ComputerId { get; set; }
	public DateTime EntryDate { get; set; }
	public DateTime ReturnDate { get; set; }
	public DateTime? ReturnedAt { get; set; }
	[Required]
	public User User { get; set; } = null!;
	public Computer Computer { get; set; }
}

