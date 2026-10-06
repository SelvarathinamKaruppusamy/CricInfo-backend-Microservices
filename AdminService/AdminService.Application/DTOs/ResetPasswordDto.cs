namespace AdminService.Application.DTOs.Auth;

public class ResetPasswordDto
{
    public string UserName { get; set; } = string.Empty;

    public string CurrentPassword { get; set; } = string.Empty;

    public string NewPassword { get; set; } = string.Empty;
}