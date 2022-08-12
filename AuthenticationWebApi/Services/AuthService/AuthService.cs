using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AuthenticationWebApi.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(DataContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthResponseDto> Login(UserDto request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return new AuthResponseDto { Message = "User not found." };
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new AuthResponseDto { Message = "Wrong Password." };
            }

            string token = CreateToken(user);
            var refreshToken = CreateRefreshToken();
            SetRefreshToken(refreshToken, user);

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken.Token,
                TokenExpires = refreshToken.Expires,
                User = new UserData { Name=user.Name,Email=user.Email,Id=user.Id},
            };
        }

        public async Task<AuthResponseDto> RegisterUser(UserDto request)
        {  
            
            if (UserExistsByEmail(request.Email))
            {
                return new AuthResponseDto { Message = "Email already existed", Success = false };
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
          
            var user = new User
            {
                Name = request.Name,
                Email= request.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthResponseDto { Message = "Register Successfully", Success = true, User = new UserData { Name=user.Name,Email=user.Email,Id=user.Id}  };
        }

        public async Task<AuthResponseDto> RefreshToken()
        {
            var refreshToken = _httpContextAccessor?.HttpContext?.Request.Cookies["refreshToken"];
            var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            if(user == null)
            {
                return new AuthResponseDto { Message = "Invalid Refresh Token" };
            }
            else if(user.TokenExpires < DateTime.Now)
            {
                return new AuthResponseDto { Message = "Token expired." };
            }

            string token = CreateToken(user);
            var newRefreshToken = CreateRefreshToken();
            SetRefreshToken(newRefreshToken, user);

            return new AuthResponseDto
            {
                Success = true,
                Token = token,
                RefreshToken = newRefreshToken.Token,
                TokenExpires = newRefreshToken.Expires
            };
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role)
               
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private RefreshToken CreateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };

            return refreshToken;
        }

        private async void SetRefreshToken(RefreshToken refreshToken, User user)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires,
                SameSite = SameSiteMode.None,
                Secure = true
            };
            _httpContextAccessor?.HttpContext?.Response
                .Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

            user.RefreshToken = refreshToken.Token;
            user.TokenCreated = refreshToken.Created;
            user.TokenExpires = refreshToken.Expires;

            await _context.SaveChangesAsync();
        }

        public bool UserExistsByEmail(string email)
        {
            return (
                _context.Users?.Any(e => e.Email == email && e.Deleted == false)
            ).GetValueOrDefault();
        }
      
        public async Task<AuthResponseDto> GetMe()
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            var userId = currentUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

          User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == int.Parse(userId));

         if (user == null)
            {
                    return new AuthResponseDto { Message = "User not found", Success = false };
            }
            return new AuthResponseDto { Message = "User found", Success = true, User = new UserData { Name = user.Name, Email = user.Email, Id = user.Id } };

        }

        public async Task<AuthResponseDto> Logout()
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies["refreshToken"] != null)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTime.Now.AddDays(-1),
                    SameSite = SameSiteMode.None,
                    Secure = true,
                };
                _httpContextAccessor?.HttpContext?.Response
                .Cookies.Append("refreshToken", "", cookieOptions);
            }

            return new AuthResponseDto { Message = "Logout" };
        }
        
       
        public  User? GetUserById(int id)
        {
            var user =  _context.Users.FirstOrDefault(u => u.Id == id);
            return user;
        }

         public int? GetCurrentUserId()
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            var userId = currentUser.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value;

            if(userId == null)
            {
                return null;
            }

            return int.Parse(userId);

        }


        public string? GetUsername()
        {
            var currentUser = _httpContextAccessor.HttpContext.User;
            var username = currentUser.Claims.Where(c => c.Type == ClaimTypes.Name).FirstOrDefault().Value;

            if (username == null)
            {
                return null;
            }

            return username;
        }
    }
}
