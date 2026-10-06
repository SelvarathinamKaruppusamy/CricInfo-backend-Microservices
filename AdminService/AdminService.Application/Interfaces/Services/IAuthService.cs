using AdminService.Application.DTOs.Auth;

namespace AdminService.Application.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponseDto> Login(LoginDto dto);

    Task<bool> Logout(string username);

    Task<bool> ResetPassword(ResetPasswordDto dto);

    Task<ProfileDto?> GetProfile(string username);

    Task<bool> UpdateProfile(int id, ProfileDto dto);

    Task<bool> Register(RegisterDto dto);
}