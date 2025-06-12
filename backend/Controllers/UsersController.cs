using Microsoft.AspNetCore.Mvc;
using AdminApi.Dtos;
using AdminApi.Models;
using AdminApi.helperFunctions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AdminContext _context;

        public UsersController(AdminContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            List<FullUserDto> users = await _context.Users
                .OrderBy(e => e.Name)
                .Select(log => new FullUserDto(
                    log.Id,
                    log.Name,
                    log.Email,
                    log.Klass,
                    HelperFunctions.ConvertBitRolesToString(log.Roles), // Assuming this returns a List<string>
                    log.ComputerLogs.Select(cl => new ComputerLoanDto(
                        cl.Id,
                        cl.UserId,
                        cl.User.Name,  // Assuming cl.User.Name is a valid string
                        cl.ComputerId,
                        new ComputerDto(
                            cl.Computer.Id,
                            cl.Computer.Name,
                            cl.Computer.Serial,
                            cl.Computer.Damage,
                            cl.Computer.Status,
                            cl.Computer.ComputerLogs
                                .Select(loan => loan.Id.ToString())  // Convert to List<string>
                                .ToList(),
                            cl.Computer.EntryDate
                        ),
                        cl.ReturnDate,
                        cl.ReturnedAt,
                        cl.EntryDate
                    )).ToList()  // This now returns a List<ComputerLoanDto>
                ))
                .ToListAsync();

            if (users == null)
                return NotFound();

           foreach (var user in users)
            {
                if (user.Id == 1 || user.Id == 2){Console.WriteLine($"Id: {user.Id}, Name: {user.Name}, Klass: {user.Klass}");
                Console.WriteLine($"Email: {user.Email}");
                Console.WriteLine("Roles: " + string.Join(", ", user.Roles));  // Assuming Roles is a List<string>

                if (user.ComputerLoans != null && user.ComputerLoans.Count > 0)
                {
                    foreach (var computerLoan in user.ComputerLoans)
                    {
                        Console.WriteLine($"ComputerLoan Id: {computerLoan.Id}");
                        Console.WriteLine($"  UserId: {computerLoan.UserId}");
                        Console.WriteLine($"  User: {computerLoan.User}");
                        Console.WriteLine($"  ComputerId: {computerLoan.ComputerId}");
                        Console.WriteLine($"  ReturnDate: {computerLoan.ReturnDate}");
                        Console.WriteLine($"  ReturnedAt: {computerLoan.ReturnedAt}");
                        Console.WriteLine($"  CreatedAt: {computerLoan.EntryDate}");

                        // Print details of the Computer object
                        Console.WriteLine($"Computer Id: {computerLoan.Computer.Id}");
                        Console.WriteLine($"  Model: {computerLoan.Computer.Name}");
                        Console.WriteLine($"  Serial: {computerLoan.Computer.Serial}");
                        Console.WriteLine($"  State: {computerLoan.Computer.State}");
                        Console.WriteLine($"  CreatedAt: {computerLoan.Computer.EntryDate}");

                        // Print the list of computer loan IDs (if there are any)
                        if (computerLoan.Computer.ComputerLogs != null)
                        {
                            Console.WriteLine("  Associated Computer Loans: " + string.Join(", ", computerLoan.Computer.ComputerLogs));
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No computer loans available.");
                }}
                

               
            }


            return Ok(users);
        }
    }
}
