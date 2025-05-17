using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // For EF Core
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using EduSyncWebApi.DTO;
using EduSyncWebApi.Data;
using EduSyncWebApi.Models;

namespace EduSyncWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            if (await _context.UserModels.AnyAsync(u => u.Email == dto.Email))
                return BadRequest("User already exists");

            var user = new UserModel()
            {
                Name = dto.Name,
                Email = dto.Email,
                Role = dto.Role,
                PasswordHash = HashPassword(dto.PasswordHash) // For simplicity, use proper hashing in production
            };

            _context.UserModels.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User registered successfully");
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var user = await _context.UserModels.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || user.PasswordHash != HashPassword(dto.PasswordHash))
                return Unauthorized("Invalid credentials");

            return Ok("Login successful");
        }

        private static string HashPassword(string password)
        {
            // Simple SHA256 hash - use something stronger in production
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }
    }
}
