using System.Security.Claims;
using api.Data;
using api.DTOs.AccountDTOs;
using api.Interfaces;
using api.Models;
using Jose.native;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ITokenService _tokenService;

    public AuthController(AppDbContext context, ITokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm]RegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            return BadRequest("Bu e-posta zaten kayıtlı.");

        var user = new User
        {
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "user"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = _tokenService.CreateToken(user);
        return Ok(new { token, user = new { user.Id, user.Email, user.Role } });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm]LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            return Unauthorized("Geçersiz kullanıcı veya şifre.");

        var token = _tokenService.CreateToken(user);
        return Ok(new { token, user = new { user.Id, user.Email, user.Role } });
    }
    
    [HttpDelete("delete")]
    [Authorize]
    public async Task<IActionResult> DeleteAccount()
    {
        var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdStr))
            return Unauthorized("Token içinde kullanıcı ID bulunamadı.");

        if (!int.TryParse(userIdStr, out int userId))
            return BadRequest("Kullanıcı ID hatalı.");

        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return NotFound("Kullanıcı bulunamadı.");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return Ok("Hesabınız başarıyla silindi.");
    }


}