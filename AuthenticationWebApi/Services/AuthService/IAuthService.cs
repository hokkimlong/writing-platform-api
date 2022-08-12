namespace AuthenticationWebApi.Services.AuthService
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterUser(UserDto request);
        Task<AuthResponseDto> Login(UserDto request);
        Task<AuthResponseDto> RefreshToken();
        Task<AuthResponseDto> GetMe();
        Task<AuthResponseDto> Logout();

        int? GetCurrentUserId();
    }
}
