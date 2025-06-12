using Microsoft.AspNetCore.Mvc;
using AdminApi.Dtos;
using AdminApi.Models;
using AdminApi.AccessCards;
using AdminApi.helperFunctions;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;

namespace AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessCardController : ControllerBase
    {
        private readonly AdminContext _context;

        public AccessCardController(AdminContext context)
        {
            _context = context;
        }

        [HttpGet("users")]
        public async Task<ActionResult<User>> GetUsers()
        {
            List<BasicUserDto> users = await _context.Users
                .OrderBy(e => e.Name)
                .Select(log => new BasicUserDto(
                    log.Id,
                    log.Name,
                    log.Email,
                    HelperFunctions.ConvertBitRolesToString(log.Roles),
                    string.IsNullOrEmpty(log.AccessCardImagePath) ? null : Base64EncodeImage(log.AccessCardImagePath)
                ))
                .ToListAsync();

            if (users == null)
                return NotFound();

            return Ok(
                users
            );
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<FinishedAccessCardDto>> GetAccessCard([FromRoute] long id)
        {
            Console.WriteLine("en skibidi fetch");
            User user = await _context.Users.FindAsync(id);

            if (user == null)
                return NotFound();

            if(string.IsNullOrEmpty(user.AccessCardImagePath))
                return NotFound();

            FinishedAccessCardDto accesscard = new(
                user.Id,
                Base64EncodeImage(user.AccessCardImagePath)
            );

            return Ok(
                accesscard
            ); 
        }

        [HttpPost("upload")]
        public async Task<ActionResult> PostImage([FromRoute] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is not uploaded or is empty.");

            try
            {
                string fileEmail = Path.GetFileNameWithoutExtension(file.FileName);

                User? user = await _context.Users
                    .FirstOrDefaultAsync(e => e.Email == fileEmail);

                if (user == null)
                    throw new ArgumentException("User not found");
                
                string accessCardPath = await CreateAccessCard(file, user);
                
                // Return success response (or processed data)
                return Ok(new { Message = "File processed successfully", ImagePath = accessCardPath });
            }
            catch (Exception ex)
            {
                // Handle errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("upload/bulk")]
        public async Task<ActionResult> PostBulkFile([FromRoute] IFormFile zipFile)
        {
            if (zipFile == null || zipFile.Length == 0)
                return BadRequest("File is not uploaded or is empty.");

            try
            {  
                string zipPath = Path.Combine(Path.GetTempPath(), zipFile.FileName);
                string extractPath = $"{Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory)?.Parent?.Parent?.Parent?.FullName}/AccessCards/bulkZip";

                ZipFile.ExtractToDirectory(zipPath, extractPath);

                foreach (var filePath in Directory.EnumerateFiles(extractPath, "*", SearchOption.AllDirectories))
                {
                    if(filePath.Contains("__MACOSX") || filePath.Contains("DS_Store"))
                    {
                        // Skipping bullshit fucking stupid ass folder that ruins everything and that nobody likes because it is shit.
                        continue;
                    }

                    //Extract the email from the filename
                    string fileEmail = Path.GetFileNameWithoutExtension(filePath);

                    // Find the user by email
                    User? user = await _context.Users
                        .FirstOrDefaultAsync(e => e.Email == fileEmail);

                    if (user == null)
                    {
                        continue; // Skip if user is not found
                    }

                    using (var stream = System.IO.File.OpenRead(filePath))
                    {
                        IFormFile File = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name));

                        await CreateAccessCard(File, user);
                    }
                }

                // Deletes zipped items
                DirectoryInfo di = new DirectoryInfo(extractPath);
                foreach (FileInfo file in di.EnumerateFiles())
                {
                    if(file.Name == ".keep")
                        continue;
                        
                    file.Delete(); 
                }
                foreach (DirectoryInfo dir in di.EnumerateDirectories())
                {
                    dir.Delete(true); 
                }
                
                // Return success response (or processed data)
                return Ok(new { Message = "File processed successfully" });
            }
            catch (Exception ex)
            {
                // Handle errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("upload/{id:int}")]
        public async Task<ActionResult> PostSpecificUserImage(IFormFile file, [FromRoute]long id)
        {
            Console.WriteLine("skibidi fetch 2");
            if (file == null || file.Length == 0)
                return BadRequest("File is not uploaded or is empty.");

            try
            {
                User user = await _context.Users.FindAsync(id) ?? throw new ArgumentException("User not found");

                string accessCardPath = await CreateAccessCard(file, user);
                
                // Return success response
                return Ok(new { Message = "File processed successfully", ImagePath = accessCardPath });
            }
            catch (Exception ex)
            {
                // Handle errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        /*
            Takes in an image path as a string and returns a string if the image is found, else it returns null.
        */
        private static string? Base64EncodeImage(string imagePath)
        {
            if(!System.IO.File.Exists(imagePath))
                return null;

            byte[] imageArray = System.IO.File.ReadAllBytes(imagePath);
            string base64ImageRepresentation = Convert.ToBase64String(imageArray);

            return base64ImageRepresentation;
        }

        /*
            Takes in a image file and a user object and returns a string of the image path to the access card. If it was not successful it throws errors.
        */
        private async Task<string> CreateAccessCard(IFormFile file, User user)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Invalid file");

            if (user == null)
                throw new ArgumentException("User not found");

            string safeFileName = Path.GetFileName(file.FileName);
            string tempFilePath = Path.Combine(Path.GetTempPath(), safeFileName);

            try
            {

                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                string accessCardPath = await AccessCard.GenerateAccessCardData(user, tempFilePath);

                try
                
                {
                    System.IO.File.Delete(tempFilePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to delete temp file {tempFilePath}: {ex.Message}");
                }

                user.AccessCardImagePath = accessCardPath;

                await _context.SaveChangesAsync();

                return accessCardPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateAccessCard: {ex.Message}");
                throw;
            }
        }
    }
}
