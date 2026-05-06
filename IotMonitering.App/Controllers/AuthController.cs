using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IoTMonitoring.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        // In-memory session store
        private static readonly Dictionary<string, LoginSession> Sessions = [];

        private readonly string clientId =
            "";

        private readonly string jwtKey =
            "THIS_IS_MY_SUPER_SECRET_JWT_KEY_123456789";

        [HttpPost("start")]
        public IActionResult StartLogin()
        {
            var sessionId = Guid.NewGuid().ToString();

            var session = new LoginSession
            {
                SessionId = sessionId,
                IsAuthenticated = false
            };

            Sessions[sessionId] = session;

            _ = Task.Run(async () =>
            {
                try
                {
                    var app = PublicClientApplicationBuilder
                        .Create(clientId)
                        .WithAuthority(
                            AzureCloudInstance.AzurePublic,
                            "common")
                        .Build();

                    string[] scopes = { "User.Read" };

                    var result = await app
                        .AcquireTokenWithDeviceCode(
                            scopes,
                            deviceCodeResult =>
                            {
                                session.UserCode =
                                    deviceCodeResult.UserCode;

                                session.VerificationUrl =
                                    deviceCodeResult.VerificationUrl;

                                return Task.CompletedTask;
                            })
                        .ExecuteAsync();

                    session.IsAuthenticated = true;

                    session.Username =
                        result.Account.Username;

                    session.JwtToken =
                        GenerateJwt(result.Account.Username);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            });

            return Ok(new
            {
                sessionId
            });
        }

        [HttpGet("status/{sessionId}")]
        public IActionResult GetStatus(string sessionId)
        {
            if (!Sessions.ContainsKey(sessionId))
            {
                return NotFound();
            }

            var session = Sessions[sessionId];

            return Ok(new
            {
                session.UserCode,
                session.VerificationUrl,
                session.IsAuthenticated,
                session.Username,
                session.JwtToken
            });
        }

        private string GenerateJwt(string username)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey));

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username)
            };

            var token = new JwtSecurityToken(
                issuer: "IoTMonitoring",
                audience: "IoTMonitoringUsers",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }

    public class LoginSession
    {
        public string SessionId { get; set; }

        public string UserCode { get; set; }

        public string VerificationUrl { get; set; }

        public bool IsAuthenticated { get; set; }

        public string Username { get; set; }

        public string JwtToken { get; set; }
    }
}