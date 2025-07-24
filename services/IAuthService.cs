using Health.Entities;
using Health.Models;

namespace Health.services
{
    public interface IAuthService
    {
        Task<List<UserListDto>> GetAllUsersAsync();

        Task<UserListDto?> GetUserById(Guid id);
        
        Task<TokenResponseDto?> LoginAsync(UserLoginDto request);

        Task<UserListDto?> RegisterAsync(UserCreateDto request);

        Task<TokenResponseDto?> RefreshTokensAsync(RefreshTokenRequestDto request);

        Task<User?> UpdateUserAsync(Guid id, UserUpdateDto request);

        Task<User?> DeleteUserAsync(Guid id);

        Task<User?> ResetPasswordAsync(ResetPasswordDto request);

        Task<bool?> SendOtpAsync(SendOtpDto request);
        
    }
}
