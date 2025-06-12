using System;
using System.Data;
using System.Linq;
using AdminApi.Models;
namespace AdminApi.Dtos
{
public class BookTitleDto
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int TotalBooks { get; set; }
    public int AvailableBooks { get; set; }
    public double Rating {get; set;}
    public List<ReviewDto>? Reviews { get; set; } = new();
    public List<BookDto> Books { get; set; } = new();
}
}