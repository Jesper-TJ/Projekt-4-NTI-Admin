using System;
using System.Linq;
using AdminApi.Models;

namespace AdminApi.Dtos
{
public class BookDto
{
    public long Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public string BarCode { get; set; } = string.Empty;
    public bool IsAvailable { get; set; } // Derived from BookLogs
}
}