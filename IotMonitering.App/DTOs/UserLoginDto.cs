namespace IoTMonitoring.DTOs
{
    public class UserLoginDto
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class UserLoginResponseDto
    {
        public string Token { get; set; } = "";
        public string RefreshToken { get; set; } = "";
    }
}
