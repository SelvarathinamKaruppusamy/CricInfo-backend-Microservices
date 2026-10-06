namespace AdminService.Application.DTOs.Auth;

public class RegisterDto
{
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public string MobileNo { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public DateTime Dob { get; set; }
}