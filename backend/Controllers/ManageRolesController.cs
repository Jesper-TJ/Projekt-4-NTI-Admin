using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdminApi.Models;
using AdminApi.Dtos;
using AdminApi.Authenticated;
using System.Collections;
using Microsoft.IdentityModel.Tokens;
using AdminApi.helperFunctions;

namespace AdminApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageRolesController : ControllerBase
    {
        private readonly AdminContext _context;

        public ManageRolesController(AdminContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<BasicUserDto>> GetTeachers()
        {
            var authHeader = Request.Headers["Authorization"].ToString();

            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized("JWT token is missing or invalid.");
            }

            // Extract the token (remove "Bearer ")
            var token = authHeader.Substring("Bearer ".Length);

            if (Authenticated.Authenticated.Main(token, _context).Result)
            {
                Console.WriteLine("Authenticated");
            }
            else
            {
                return Unauthorized();
            }

            List<User> users = await _context.Users.ToListAsync();

            List<BasicUserDto> previews = users
                .Where(e => HelperFunctions.CheckIfStaff(e.Roles))
                .OrderBy(e => e.Name)
                .Select(log => new BasicUserDto(log.Id, log.Name, log.Email, HelperFunctions.ConvertBitRolesToString(log.Roles), null))
                .ToList();

            return Ok(
                previews
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeacherRole([FromRoute] long id, BasicUserDto teacher)
        {
            if (id != teacher.Id)
                return BadRequest();

            User existingTeacher = await _context.Users.FindAsync(id);

            if (existingTeacher == null)
                return NotFound();

            existingTeacher.Roles = HelperFunctions.ConvertRolesToBitArray(teacher.Roles);

            // Save changes
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
