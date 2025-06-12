using System;
using System.Linq;
using AdminApi.Models;

namespace AdminApi.Dtos
{
public class ReviewDto
{
    public long Id { get; set; }
    public string? UserName {get; set;} = "";
    public double Rating { get; set; }
    public required string CreatedAt {get; set;}
    public required string Content { get; set; }
    public bool Anonymous {get; set;}
}
}