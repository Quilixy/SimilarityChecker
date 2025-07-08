using System.Security.Claims;
using api.DTOs.SettingsDTO;
using api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/settings")]
public class SettingsController : ControllerBase
{
    private readonly IUserService _userService;

    public SettingsController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet("account/profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        if (userId == null) return Unauthorized();

        var profile = await _userService.GetProfileAsync(userId);
        return Ok(profile);
    }
    
    [HttpPut("account/profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDTO dto)
    {
        int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        if (userId == null) return Unauthorized();

        var result = await _userService.UpdateProfileAsync(userId, dto);
        return result ? NoContent() : BadRequest();
    }
    
    [HttpPost("account/change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDTO dto)
    {
        int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        if (userId == null) return Unauthorized();

        var success = await _userService.ChangePasswordAsync(userId, dto);
        return success ? Ok("Şifre güncellendi") : BadRequest("Şifre değiştirme başarısız");
    }
    
    [HttpDelete("account/delete")]
    [Authorize]
    public async Task<IActionResult> DeleteAccount()
    {
        int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        if (userId == null) return Unauthorized();

        var success = await _userService.DeleteAccountAsync(userId);
        return success ? NoContent() : BadRequest("Hesap silinemedi");
    }
    
    [HttpGet("appearance/theme")]
    [Authorize]
    public async Task<IActionResult> GetTheme()
    {
        int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        if (userId == null) return Unauthorized();

        var theme = await _userService.GetThemePreferenceAsync(userId);
        return Ok(new { theme });
    }
    
    [HttpPost("appearance/theme")]
    [Authorize]
    public async Task<IActionResult> SetTheme([FromBody] ThemeDTO dto)
    {
        int userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        if (userId == null) return Unauthorized();

        var updated = await _userService.SetThemePreferenceAsync(userId, dto.Theme);
        return updated ? NoContent() : BadRequest("Tema ayarlanamadı");
    }
    
    [HttpPost("support/feedback")]
    [Authorize]
    public IActionResult SendFeedback([FromBody] FeedBackDTO dto)
    {
        return Ok("Geri bildiriminiz alınmıştır. Teşekkürler!");
    }
    
    [HttpGet("support/about")]
    public IActionResult GetAboutInfo()
    {
        return Ok(new
        {
            AppName = "ExampleApp",
            Version = "v1.0.3",
            Developer = "Yıldıray Dönmez",
            Contact = "destek@exampleapp.com"
        });
    }
}
