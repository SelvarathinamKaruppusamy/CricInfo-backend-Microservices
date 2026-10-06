namespace AdminService.Domain.Entities;

public class Admin
{
    public int Id { get; set; }

    public string UserName { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public string MobileNo { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public DateTime Dob { get; set; }

    public bool FirstLogin { get; set; }

    public bool IsLoggedIn { get; set; }

    public DateTime? TokenExpiry { get; set; }
}