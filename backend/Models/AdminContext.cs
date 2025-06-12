using Microsoft.EntityFrameworkCore;

namespace AdminApi.Models;

public class AdminContext : DbContext
{
	public AdminContext(DbContextOptions<AdminContext> options)
		: base(options)
	{
	}

	public DbSet<Computer> Computers { get; set; } = null!;
	public DbSet<ComputerLog> ComputerLogs { get; set; } = null!;
	public DbSet<User> Users { get; set; } = null!;
	public DbSet<BookTitle> BookTitles { get; set; } = null!;
	public DbSet<Book> Books { get; set; } = null!;
	public DbSet<BookGenre> BookGenres { get; set; } = null!;
	public DbSet<BookGenreBookTitle> BookGenresBookTitles { get; set; } = null!;
	public DbSet<BookLoan> BookLoans { get; set; } = null!;
	public DbSet<Review> Reviews { get; set; } = null!;
	public DbSet<SupportTicket> SupportTickets { get; set; } = null!;
	public DbSet<SupportTicketMessage> SupportTicketMessages { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<ComputerLog>()
			.HasOne(cl => cl.User)
			.WithMany(u => u.ComputerLogs)
			.HasForeignKey(cl => cl.UserId);


		modelBuilder.Entity<ComputerLog>()
			.HasOne(cl => cl.Computer)
			.WithMany(c => c.ComputerLogs)
			.HasForeignKey(cl => cl.ComputerId);


		modelBuilder.Entity<Review>()
			.HasOne(r => r.BookTitle)
			.WithMany(bt => bt.Reviews)
			.HasForeignKey(r => r.BookTitleId)
			.OnDelete(DeleteBehavior.Cascade);


		modelBuilder.Entity<Review>()
			.HasOne(r => r.User)
			.WithMany(u => u.Reviews)
			.HasForeignKey(r => r.UserId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<Book>()
			.HasOne(b => b.BookTitle)
			.WithMany(bt => bt.Books)
			.HasForeignKey(b => b.BookTitleId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<BookGenreBookTitle>()
			.HasKey(j => new { j.BookGenreId, j.BookTitleId });

		modelBuilder.Entity<BookGenreBookTitle>()
			.HasOne(j => j.BookGenre)
			.WithMany(bg => bg.BookGenreBookTitles)
			.HasForeignKey(j => j.BookGenreId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<BookGenreBookTitle>()
			.HasOne(j => j.BookTitle)
			.WithMany(bt => bt.BookGenreBookTitles)
			.HasForeignKey(j => j.BookTitleId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<BookLoan>()
			.HasOne(bl => bl.Book)
			.WithMany(b => b.BookLoans)
			.HasForeignKey(bl => bl.BookId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<BookLoan>()
			.HasOne(bl => bl.User)
			.WithMany(u => u.BookLoans)
			.HasForeignKey(bl => bl.UserId)
			.OnDelete(DeleteBehavior.Cascade);

		modelBuilder.Entity<SupportTicket>()
			.HasOne(st => st.User)
			.WithMany(u => u.SupportTickets)
			.HasForeignKey(st => st.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<SupportTicket>()
			.HasOne(st => st.Janitor)
			.WithMany()
			.HasForeignKey(st => st.JanitorId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<SupportTicketMessage>()
			.HasOne(stm => stm.User)
			.WithMany(u => u.SupportTicketMessages)
			.HasForeignKey(stm => stm.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		modelBuilder.Entity<SupportTicketMessage>()
			.HasOne(stm => stm.SupportTicket)
			.WithMany(st => st.SupportTicketMessages)
			.HasForeignKey(stm => stm.SupportTicketId)
			.OnDelete(DeleteBehavior.Restrict);
	}

}
