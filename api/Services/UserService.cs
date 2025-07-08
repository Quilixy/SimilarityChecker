using System.Threading.Tasks;
using api.Data;
using Microsoft.EntityFrameworkCore;
using api.Dtos;
using api.DTOs.SettingsDTO;
using api.Interfaces;
using api.Models;

namespace api.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }
        
        public async Task<UserProfileDTO?> GetProfileAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return null;

            return new UserProfileDTO
            {
                Email = user.Email,
            };
        }
        
        public async Task<bool> UpdateProfileAsync(int userId, UpdateProfileDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return false;

            user.Email = dto.Email;

            _context.Users.Update(user);
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        // Şifre değiştir (Basit örnek, hash işlemi dahil değil, kendin ekle)
        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return false;

            // Basit doğrulama, gerçek projede hash kontrolü yapmalısın
            if (user.PasswordHash != dto.OldPassword) return false;

            // Şifreyi hashleyip sakla (burada düz metin var, hash ile değiştir)
            user.PasswordHash = dto.NewPassword;

            _context.Users.Update(user);
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
        
        public async Task<bool> DeleteAccountAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
        
        public async Task<string> GetThemePreferenceAsync(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return "system";
            return user.ThemePreference ?? "system";
        }
        
        public async Task<bool> SetThemePreferenceAsync(int userId, string theme)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) return false;

            user.ThemePreference = theme;

            _context.Users.Update(user);
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }
    }
}
