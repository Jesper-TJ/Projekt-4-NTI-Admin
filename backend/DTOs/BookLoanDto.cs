using System;
using System.Linq;
using AdminApi.Models;

namespace AdminApi.Dtos
{

public class BookLoanDto

{

    public long Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public required string ReturnDate { get; set; }

    public string? ReturnedAt { get; set; }

    public required string CreatedAt { get; set; }

    public string Status { get; set; } = string.Empty;
}
}