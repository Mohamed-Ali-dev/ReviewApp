using ReviewApp.Dtos;
using ReviewApp.Models;
using System.IdentityModel.Tokens.Jwt;

namespace ReviewApp.Services
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterDto dto);
        Task<AuthModel> GetTokenAsync(LogInDto dto);
        Task<string> AddRoleAsync(AddRoleDto dto);
        Task<AuthModel> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
    }
}
