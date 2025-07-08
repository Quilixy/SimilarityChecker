using api.DTOs.SettingsDTO;

namespace api.Interfaces;

public interface IUserService
{
    Task<UserProfileDTO?> GetProfileAsync(int userId);
    Task<bool> UpdateProfileAsync(int userId, UpdateProfileDTO dto);
    Task<bool> ChangePasswordAsync(int userId, ChangePasswordDTO dto);
    Task<bool> DeleteAccountAsync(int userId);
    Task<string> GetThemePreferenceAsync(int userId);
    Task<bool> SetThemePreferenceAsync(int userId, string theme);
}