using AdminApi.Dtos;
using AdminApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdminApi.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ComputersController : ControllerBase
  {
    private readonly AdminContext _context;

    public ComputersController(AdminContext context)
    {
      _context = context;
    }

    [HttpGet("all")]
public async Task<IActionResult> GetAllComputersWithLogs()
{
    Console.WriteLine("Fetching all computers with logs.");

    // Fetch all computers, including logs and related users
    List<Computer> computers = await _context.Computers
        .Include(c => c.ComputerLogs)  // Include related logs
        .ThenInclude(cl => cl.User)    // Include the related User for each ComputerLog
        .ToListAsync();

    Console.WriteLine($"Fetched {computers.Count} computers.");

    if (!computers.Any())
    {
        Console.WriteLine("No computers found.");
        return NotFound(new { Message = "No computers found." });
    }

    // Map to DTOs
    List<ComputerDto> computerDtos = computers.Select(computer =>
    {
        // Map the logs to ComputerLoanDto
        List<ComputerLoanDto> logDtos = computer.ComputerLogs.Select(log => new ComputerLoanDto(
            log.Id,                      // Id
            log.UserId,                  // UserId
            log.User?.Name,              // User (null-safe)
            log.ComputerId,              // ComputerId
            null,                        // Computer (optional, omit circular reference)
            log.ReturnDate,              // ReturnDate
            log.ReturnedAt,              // ReturnedAt
            log.EntryDate                // EntryDate
        )).ToList();

        // Create the ComputerDto including logs
        return new ComputerDto(
            computer.Id,                 // Id
            computer.Name,               // Name
            computer.Serial,             // Serial
            computer.Damage,             // State
            computer.Status,             // Status
            logDtos.Select(log => log.User ?? "Unknown").ToList(), // Logs as a list of strings
            computer.EntryDate           // EntryDate
        );
    }).ToList();

    Console.WriteLine($"Returning {computerDtos.Count} computers with logs.");
    return Ok(computerDtos);
}


        [HttpGet("{computerId}")]
        public async Task<IActionResult> GetComputerWithLogs(long computerId)
        {
            Console.WriteLine($"Fetching computer with ID {computerId}.");

            // Fetch the computer with logs and users
            Computer? computer = await _context.Computers
                .Include(c => c.ComputerLogs)  // Include related logs
                .ThenInclude(cl => cl.User)    // Include the related User for each ComputerLog
                .FirstOrDefaultAsync(c => c.Id == computerId);

            if (computer == null)
            {
                Console.WriteLine("Computer not found.");
                return NotFound(new { Message = "Computer not found." });
            }

            // Map to DTO
            ComputerDto computerDto = new ComputerDto(
                computer.Id,                  // Id
                computer.Name,                // Name
                computer.Serial,              // Serial
                computer.Damage,              // Damage (State)
                computer.Status,              // Status
                computer.ComputerLogs
                    .Select(log => log.User?.Name ?? "Unknown User") // Map logs to strings (e.g., User Names)
                    .ToList(),                // ComputerLogs as List<string>
                computer.EntryDate            // EntryDate
            );

            Console.WriteLine("Returning computer with logs.");
            return Ok(computerDto);
        }

  }

}
