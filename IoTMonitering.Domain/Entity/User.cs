
using Microsoft.AspNetCore.Identity;

namespace IoTMonitering.Domain.Entity
{
    public class User : IdentityUser
    {
        public IEnumerable<Device> Devices { get; set; } = [];
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
