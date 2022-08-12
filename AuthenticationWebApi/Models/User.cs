using System.ComponentModel.DataAnnotations;

namespace AuthenticationWebApi.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;

        public byte[] PasswordHash { get; set; } = new byte[32];
        public byte[] PasswordSalt { get; set; } = new byte[32];
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool Deleted { get; set; } = false;

        public List<Article> Articles { get; set; } 
        
    }
}
