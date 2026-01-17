using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using web_api_p5.Data;
using web_api_p5.DTOs;
using web_api_p5.Models;

namespace web_api_p5.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await _context.Customers.AnyAsync(c => c.Email == request.Email))
            {
                return BadRequest(new { message = "Email already exists" });
            }

            var customer = new Customer
            {
                Email = request.Email,
                Password = request.Password, // Note: Should hash password in production
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber ?? "",
                Address = request.Address ?? "",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Registration successful" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == request.Email && c.Password == request.Password);

            if (customer == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            if (!customer.IsActive)
            {
                 return Unauthorized(new { message = "Account is inactive" });
            }

            var token = GenerateJwtToken(customer);

            return Ok(new
            {
                token = token,
                user = new
                {
                    id = customer.Id,
                    email = customer.Email,
                    full_name = customer.FullName
                },
                student_id = "p5" // Hardcoded student ID
            });
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
             // TODO: Implement GET Me logic using User.Identity
             return Ok();
        }

        private string GenerateJwtToken(Customer customer)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, customer.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", customer.Id.ToString()),
                new Claim("role", "Customer") // Simple role
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
