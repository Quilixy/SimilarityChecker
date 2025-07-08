namespace api.DTOs.SettingsDTO;

public class UserProfileDTO
{
    public int? Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? ThemePreference { get; set; }
}