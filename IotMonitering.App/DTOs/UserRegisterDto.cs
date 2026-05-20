namespace IoTMonitoring.DTOs
{
    public class UserRegisterDto
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class UserRegisterResponseDto
    {
        public string Username { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
