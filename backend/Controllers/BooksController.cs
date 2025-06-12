using AdminApi.Dtos;
using AdminApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AdminApi.Controllers
{
[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly AdminContext _context;

    public BooksController(AdminContext context)
    {
        _context = context;
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllBookDetails()
    {
        Console.WriteLine("skibidi fetch");
        List<BookTitle> bookTitles = await _context.BookTitles
            .Include(bt => bt.Books) // Include related books
            .ThenInclude(b => b.BookLoans) // Include related book logs
            .Include(bt => bt.Reviews) // Include related reviews
            .ThenInclude(b => b.User)
            .Include(b => b.BookGenreBookTitles)
            .ToListAsync();

        // Log the count of book titles to check if data is fetched
        Console.WriteLine($"Fetched {bookTitles.Count} book titles.");

        if (!bookTitles.Any())
        {
            Console.WriteLine("No book titles found.");
            return NotFound(new { Message = "No book titles found." });
        }

        List<BookTitleDto> bookTitleDtos = bookTitles.Select(bookTitle =>
        {
            int totalBooks = bookTitle.Books.Count;
            int availableBooks = bookTitle.Books.Count(b =>
                b.BookLoans.All(bl => bl.ReturnedAt != null)
            );
            double rating = bookTitle.Reviews.Any() 
                   ? bookTitle.Reviews.Average(r => r.Rating) 
                   : 0;
            



            List<BookDto> bookDtos = bookTitle.Books.Select(b => new BookDto
            {
                Id = b.Id,
                Status = b.Status,
                BarCode = b.BarCode,
                IsAvailable = !b.BookLoans.Any(bl => bl.ReturnDate > DateTime.Now && bl.ReturnedAt == default)
            }).ToList();

            List<ReviewDto> reviewDtos = bookTitle.Reviews?.Select(b => new ReviewDto
            {
                Id = b.Id,
                UserName = b.User.Name,
                Rating = b.Rating,
                CreatedAt = b.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"), // Format DateTime
                Content = b.Content,
                Anonymous = b.Anonymous
            }).ToList() ?? new List<ReviewDto>();




            return new BookTitleDto
            {
                Id = bookTitle.Id,
                Title = bookTitle.Title,
                Author = bookTitle.Author,
                Description = bookTitle.Description,
                ISBN = bookTitle.ISBN,
                TotalBooks = totalBooks,
                AvailableBooks = availableBooks,
                Books = bookDtos,
                Rating = rating,
                Reviews = reviewDtos
            };
        }).ToList();

        Console.WriteLine($"Returning {bookTitleDtos.Count} book titles.");
        return Ok(bookTitleDtos);
    }


    [HttpGet("{userId}")]
    public async Task<IActionResult> GetBookLoansByUser(long userId)
    {
        var bookLoans = await _context.BookLoans
            .Include(bl => bl.Book)
            .ThenInclude(b => b.BookTitle)
            .Where(bl => bl.UserId == userId)
            .ToListAsync();

        if (!bookLoans.Any())
        {
            return NotFound(new { Message = "No book loans found for this user." });
        }

        var bookLoanDtos = bookLoans.Select(bl => new BookLoanDto
        {
            Id = bl.Id,
            Title = bl.Book.BookTitle.Title,
            ReturnDate = bl.ReturnDate.ToString("yyyy-MM-dd"),
            ReturnedAt = bl.ReturnedAt?.ToString("yyyy-MM-dd"), // Handle nullable DateTime
            CreatedAt = bl.CreatedAt.ToString("yyyy-MM-dd"),
            Status = bl.ReturnedAt != null ? "Returned" : (bl.ReturnDate < DateTime.Now ? "Overdue" : "Borrowed")
        }).ToList();

        return Ok(bookLoanDtos);
    }

    [HttpPost("borrow/{bookId}")]
    public async Task<IActionResult> BorrowABook(long bookId, [FromBody]long userId)
    {
        var book = await _context.Books
            .Include(b => b.BookLoans)
            .FirstOrDefaultAsync(b => b.Id == bookId);

        if (book == null || !book.BookLoans.All(bl => bl.ReturnedAt != null))
        {
            Console.WriteLine("couldnt find book");
            return BadRequest(new { Message = "Book not available for borrowing." });
        }

        var now = DateTime.UtcNow; // Use UTC time
        var bookLoan = new BookLoan
        {
            BookId = bookId,
            UserId = userId,
            CreatedAt = now,
            ReturnDate = now.AddDays(14), // Borrow for 2 weeks
            ReturnedAt = null,
            UpdatedAt = now // Add UpdatedAt field
        };

        book.BookLoans.Add(bookLoan);
        await _context.SaveChangesAsync();

        return Ok(new { Message = "Book borrowed successfully." });
    }



    [HttpPost("new/review/{bookTitleId}")]
    [Tags("Review")]
    public async Task<IActionResult> AddNewReview([FromBody] ReviewDto reviewDto, [FromRoute] long bookTitleId)
    {
        Console.WriteLine("skibidi post");

        Console.WriteLine(bookTitleId);
        // Validate the incoming DTO
        if (reviewDto == null || string.IsNullOrWhiteSpace(reviewDto.Content))
        {
            return BadRequest(new { Message = "Invalid review data. Content is required." });
        }
        Console.WriteLine(reviewDto.Anonymous);
        Console.WriteLine(reviewDto.Content);
        Console.WriteLine(reviewDto.UserName);
        try
        {
            // Check if the user exists
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Name == reviewDto.UserName);
            if (user == null)
            {
                Console.WriteLine("user");
                return BadRequest(new { Message = "User not found." });
            }
            Console.WriteLine("Nu ska den adda alla values");
            // Create a new review entity
            var review = new Review
        {
            UserId = user.Id, // Link review to the user
            BookTitleId = bookTitleId,  // Hardcoded
            Content = reviewDto.Content,
            Rating = reviewDto.Rating,
            Anonymous = reviewDto.Anonymous,
            CreatedAt = DateTime.UtcNow  // Default to current UTC date/time
        };

            Console.WriteLine("Nu ska den l'ggas till");
            // Save the review to the database
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Review added successfully." });
        }
        catch (DbUpdateException dbEx)
        {
            // Log and return database-related errors
            Console.WriteLine($"Database error adding review: {dbEx.Message}");
            return StatusCode(500, new { Message = "A database error occurred while adding the review." });
        }
        catch (Exception ex)
        {
            // Log unexpected exceptions
            Console.WriteLine($"Unexpected error adding review: {ex.Message}");
            return StatusCode(500, new { Message = "An unexpected error occurred while adding the review." });
        }
    }



}
}