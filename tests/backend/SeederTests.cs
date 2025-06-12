using System;
using System.Linq;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Xunit;
using AdminApi.Models;
using AdminApi;
using dotenv.net;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Npgsql;

public class SeederTests
{
    private void EnsureDatabaseExists(string? connectionString, string? DbName)
    {
        using (var connection = new NpgsqlConnection(connectionString))
        {
            connection.Open();

            // Check if the database exists
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = $"SELECT 1 FROM pg_database WHERE datname = '{DbName}';";
                var result = cmd.ExecuteScalar();

                // If the database does not exist, create it
                if (result == null)
                {
                    cmd.CommandText = $"CREATE DATABASE \"{DbName}\";";
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }


    [Fact]
    public void Seed_ShouldPopulateDatabaseWithSeedData()
    {
        // Load environment variables
        DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] {"./.env"}));
        
        var envVars = DotEnv.Read();

    WebApplicationBuilder builder = WebApplication.CreateBuilder();

    string? testDbHost = envVars["TEST_DB_HOST"];
    string? testDbPort = envVars["TEST_DB_PORT"];
    string? testDbUsername = envVars["TEST_DB_USERNAME"];
    string? testDbPassword = envVars["TEST_DB_PASSWORD"];
    string? testDbName = envVars["TEST_DB_NAME"];

    string? adminDbHost = envVars["ADMIN_DB_HOST"];
    string? adminDbPort = envVars["ADMIN_DB_PORT"];
    string? adminDbUsername = envVars["ADMIN_DB_USERNAME"];
    string? adminDbPassword = envVars["ADMIN_DB_PASSWORD"];
    string? adminDbName = envVars["ADMIN_DB_NAME"];

    // Add connection strings to configuration
    builder.Configuration["ConnectionStrings:TestConnection"] =
        $"Host={testDbHost};Port={testDbPort};Username={testDbUsername};Password={testDbPassword};Database={testDbName}";

    builder.Configuration["ConnectionStrings:AdminConnection"] =
        $"Host={adminDbHost};Port={adminDbPort};Username={adminDbUsername};Password={adminDbPassword};Database={adminDbName}";


        EnsureDatabaseExists(builder.Configuration.GetConnectionString("AdminConnection"), testDbName);

        // Create DbContextOptions for test database
        builder.Services.AddDbContext<AdminContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("TestConnection"));
        });

    WebApplication app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<AdminContext>();
            Seeder.Seed(context, 2);

      // Assert: Verify Users
      var users = context.Users.ToList();
      Assert.Equal(9, users.Count);
      Assert.Contains(users, u => u.Name == "John Doe");
      Assert.Contains(users, u => u.Name == "Jane Smith");

      // Verify Computers
      var computers = context.Computers.ToList();
      Assert.Equal(2, computers.Count);
      Assert.Contains(computers, c => c.Name == "Dell XPS 13");
      Assert.Contains(computers, c => c.Name == "MacBook Pro 16");

      // Verify ComputerLogs
      var computerLogs = context.ComputerLogs.ToList();
      Assert.Equal(2, computerLogs.Count);

            // Assert: Verify BookTitles
            var bookTitles = context.BookTitles.ToList();
            Assert.Equal(2, bookTitles.Count);
            Assert.Contains(bookTitles, b => b.Title == "C# in Depth");

            // Assert: Verify Books
            var books = context.Books.ToList();
            Assert.Equal(2, books.Count);

            // Assert: Verify BookGenres
            var bookGenres = context.BookGenres.ToList();
            Assert.Equal(9, bookGenres.Count);
            Assert.Contains(bookGenres, bg => bg.name == "Science Fiction");

            // Assert: Verify BookGenresBookTitles
            var bookGenresBookTitles = context.BookGenresBookTitles.ToList();
            Assert.Equal(5, bookGenresBookTitles.Count);

      // Assert: Verify BookLoans
      var bookLoans = context.BookLoans.ToList();
      Assert.Equal(3, bookLoans.Count);

      // Assert: Verify Reviews
      var reviews = context.Reviews.ToList();
      Assert.Equal(2, reviews.Count);
      Assert.Contains(reviews, r => r.Content == "A good book about C#");

      // Assert: Verify SupportTickets
      var tickets = context.SupportTickets.ToList();
      Assert.Single(tickets);
      Assert.Contains(tickets, t => t.Status == 1);

      // Assert: Verify SupportTicketMessages
      var messages = context.SupportTicketMessages.ToList();
      Assert.Single(messages);
      Assert.Contains(messages, m => m.Message == "Hello my computer is broken");

      // Assert: Verify User-ComputerLog Connections
      foreach (var user in users)
      {
        var userComputerLogs = computerLogs.Where(cl => cl.UserId == user.Id).ToList();
        Assert.Equal(userComputerLogs.Count, user.ComputerLogs.Count); // Ensure correct association

        foreach (var log in userComputerLogs)
        {
          Assert.Contains(log, user.ComputerLogs);  // Ensure log exists in the user's ComputerLogs
        }
      }

      // Assert: Verify User-Review Connections
      foreach (var user in users)
      {
        var userReviews = reviews.Where(r => r.UserId == user.Id).ToList();
        Assert.Equal(userReviews.Count, user.Reviews.Count);

                foreach (var review in userReviews)
                {
                    Assert.Contains(review, user.Reviews);
                }
            }

      // Assert: Verify User-BookLoans Connections
      foreach (var user in users)
      {
        var userBookLoans = bookLoans.Where(cl => cl.UserId == user.Id).ToList();
        Assert.Equal(userBookLoans.Count, user.BookLoans.Count);

        foreach (var loan in userBookLoans)
        {
          Assert.Contains(loan, user.BookLoans);  // Ensure loan exists in the user's BookLoans
        }
      }

      // Assert: Verify User-SupportTickets Connections
      foreach (var user in users)
      {
        var userTickets = tickets.Where(t => t.UserId == user.Id).ToList();
        Assert.Equal(userTickets.Count, user.SupportTickets.Count);

                foreach (var ticket in userTickets)
                {
                    Assert.Contains(ticket, user.SupportTickets);
                }
            }

      // Assert: Verify User-SupportTicketMessages Connections
      foreach (var user in users)
      {
        var userMessages = messages.Where(m => m.UserId == user.Id).ToList();
        Assert.Equal(userMessages.Count, user.SupportTicketMessages.Count);

                foreach (var message in userMessages)
                {
                    Assert.Contains(message, user.SupportTicketMessages);
                }
            }

      // Assert: Verify Computer-ComputerLog Connections (matching the updated model)
      foreach (var computer in computers)
      {
        var computerComputerLogs = computerLogs.Where(cl => cl.ComputerId == computer.Id).ToList();
        Assert.Equal(computerComputerLogs.Count, computer.ComputerLogs.Count);

        foreach (var log in computerComputerLogs)
        {
          Assert.Contains(log, computer.ComputerLogs);  // Ensure log exists in the computer's ComputerLogs
        }
      }

      // Assert: Verify BookTitle-Book Connections
      foreach (var bookTitle in bookTitles)
      {
        var bookBooks = books.Where(b => b.BookTitleId == bookTitle.Id).ToList();
        Assert.Equal(bookBooks.Count, bookTitle.Books.Count);

                foreach (var book in bookBooks)
                {
                    Assert.Contains(book, bookTitle.Books);
                }
            }

      // Assert: Verify BookTitle-Review Connections
      foreach (var bookTitle in bookTitles)
      {
        var bookTitleReviews = reviews.Where(r => r.BookTitleId == bookTitle.Id).ToList();
        Assert.Equal(bookTitleReviews.Count, bookTitle.Reviews.Count);

                foreach (var review in bookTitleReviews)
                {
                    Assert.Contains(review, bookTitle.Reviews);
                }
            }

            // Assert: Verify BookTitle-Genres Connections
            foreach (var bookTitle in bookTitles)
            {
                var bookGenresBookTitles2 = bookGenresBookTitles.Where(bgbt => bgbt.BookTitleId == bookTitle.Id).ToList();
                ICollection<BookGenreBookTitle> bookTitleBookGenreBookTitles = bookTitle.BookGenreBookTitles;
                Assert.Equal(bookGenresBookTitles2.Count, bookTitleBookGenreBookTitles.Count);

                foreach (var bookGenreBookTitle in bookGenresBookTitles2)
                {
                    Assert.Contains(bookGenreBookTitle, bookTitleBookGenreBookTitles);
                }
            }

      // Assert: Verify SupportTicket-SupportTicketMessage Connections
      foreach (var ticket in tickets)
      {
        var ticketMessages = messages.Where(m => m.SupportTicketId == ticket.Id).ToList();
        Assert.Equal(ticketMessages.Count, ticket.SupportTicketMessages.Count);

                foreach (var message in ticketMessages)
                {
                    Assert.Contains(message, ticket.SupportTicketMessages);
                }
            }
        }
    }
}
