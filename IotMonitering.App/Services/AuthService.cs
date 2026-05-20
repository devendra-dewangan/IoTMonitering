using IoTMonitoring.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IoTMonitoring.App.Services
{
    public class AuthService : IAuthService
    {
        private UserManager<IoTMonitering.Domain.Entity.User> _userManager;
        private ITokenService _tokenService;

        public AuthService(UserManager<IoTMonitering.Domain.Entity.User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<(string, string)> AuthenticateUser(UserLoginDto request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null)
            {
                return (string.Empty, string.Empty);
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return (string.Empty, string.Empty);
            }

            var token = _tokenService.GenerateToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return (token, refreshToken);
        }

        public async Task<(string, string)> GetToken(string refreshToken)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(
                    x =>
                    x.RefreshToken == refreshToken);

            if (user == null)
            {
                return (string.Empty, string.Empty);
            }

            if (
                user.RefreshTokenExpiryTime
                <= DateTime.UtcNow)
            {
                return (string.Empty, string.Empty);
            }

            var accessToken =
                _tokenService
                .GenerateToken(user);

            var refreshTokennew =
                _tokenService
                .GenerateRefreshToken();

            user.RefreshToken =
                refreshTokennew;

            await _userManager
                .UpdateAsync(user);

            return (accessToken,
                    refreshToken);
        }

        public async Task<IoTMonitering.Domain.Entity.User?> RegisterUser(UserRegisterDto request)
        {
            var user = new IoTMonitering.Domain.Entity.User { UserName = request.Username };
            var result = await _userManager.CreateAsync(user, request.Password);
            return result.Succeeded ? user : null;
        }
    }

    public interface IAuthService
    {
        Task<(string, string)> AuthenticateUser(UserLoginDto request);
        Task<(string, string)> GetToken(string request);
        Task<IoTMonitering.Domain.Entity.User?> RegisterUser(UserRegisterDto request);
    }
}
