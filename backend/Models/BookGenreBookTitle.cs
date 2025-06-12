using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace AdminApi.Models;

public class BookGenreBookTitle
{
    public long BookGenreId { get; set; }
    public BookGenre BookGenre { get; set; } = null!;

    public long BookTitleId { get; set; }
    public BookTitle BookTitle { get; set; } = null!;
}
