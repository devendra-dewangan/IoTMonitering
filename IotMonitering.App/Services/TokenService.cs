using IoTMonitering.Domain.Entity;
using IoTMonitoring.App.Config;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace IoTMonitoring.App.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtConfiguration _jwtConf;

        public TokenService(IOptions<JwtConfiguration> option)
        {
            _jwtConf = option.Value;
        }

        private string GenerateToken(IEnumerable<Claim> claims ,int ExpiryTime)
        {
            var key =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(_jwtConf.Key));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token =
                new JwtSecurityToken(
                    issuer:
                        _jwtConf.Issuer,

                    audience:
                        _jwtConf.Audience,

                    claims:
                        claims,

                    expires:
                        DateTime.UtcNow
                            .AddHours(ExpiryTime),

                    signingCredentials:
                        credentials);

            return new
                JwtSecurityTokenHandler()
                .WriteToken(token);
        }

        public string GenerateToken(User user)
        {
            return GenerateToken(new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.Id),

                new Claim(
                    ClaimTypes.Name,
                    user.UserName)

            },
            1);
            
        }

        public string GenerateToken(Device device)
        {
            return GenerateToken(new[]
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    device.Id.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    device.Name)

            },
            240);
        }

        public string GenerateRefreshToken()
        {
            var bytes =
                new byte[64];

            using var rng =
                RandomNumberGenerator
                .Create();

            rng.GetBytes(bytes);

            return Convert
                .ToBase64String(bytes);
        }

    }

    public interface ITokenService
    {
        string GenerateToken(User user);
        string GenerateToken(Device device);
        string GenerateRefreshToken();
    }
}
