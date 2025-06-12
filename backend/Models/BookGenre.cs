using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace AdminApi.Models;

public class BookGenre
{
    public long Id { get; set; }
    public string name { get; set; } = null!;
    public ICollection<BookGenreBookTitle> BookGenreBookTitles { get; set; } = new List<BookGenreBookTitle>();
}
