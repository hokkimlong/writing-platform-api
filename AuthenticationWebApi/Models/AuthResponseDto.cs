namespace AuthenticationWebApi.Models
{
    public class AuthResponseDto
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenExpires { get; set; }
       
        public UserData User { get; set; }
    }
    public class UserData
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int Id { get; set; }

    }
}
