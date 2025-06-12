using AdminApi.JWTDecoder;
using AdminApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AdminApi.Authenticated
{
    public static class Authenticated
    {

        public static async Task<bool> Main(string token, AdminContext _context)
        {
            string decodedJWT = JwtDecoder.main(token);
            // Assuming the decodedJWT contains the email
            string email = GetEmailFromDecodedJWT(decodedJWT);

            // Fetch users from the database and check if the user's email is the same as the decoded JWT email
            bool userExists = await _context.Users.AnyAsync(user => user.Email == email);

            return userExists;
        }

        private static string GetEmailFromDecodedJWT(string decodedJWT)
        {
            var jwtPayload = System.Text.Json.JsonDocument.Parse(decodedJWT).RootElement;
            if (jwtPayload.TryGetProperty("email", out var emailElement))
            {
                return emailElement.GetString()!;
            }
            throw new InvalidOperationException("Email not found"); 
        }
    }
}