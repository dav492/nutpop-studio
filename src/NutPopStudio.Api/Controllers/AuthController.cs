using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using NutPopStudio.Api.Data;
using NutPopStudio.Api.Dtos;
using NutPopStudio.Api.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;

namespace NutPopStudio.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(
        RegisterRequestDto request)
    {
        if (_context.Users.Any(u =>
            u.Username == request.Username))
        {
            return BadRequest("Username already exists.");
        }

        var user = new User
        {
            Username = request.Username,
            PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    request.Password)
        };

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
    LoginRequestDto request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u =>
                u.Username == request.Username);

        if (user == null)
        {
            return Unauthorized("Invalid credentials.");
        }

        var validPassword =
            BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash);

        if (!validPassword)
        {
            return Unauthorized("Invalid credentials.");
        }

        var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Role, user.Role.ToString())
    };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials);

        return Ok(new
        {
            token = new JwtSecurityTokenHandler()
                .WriteToken(token)
        });
    }
}