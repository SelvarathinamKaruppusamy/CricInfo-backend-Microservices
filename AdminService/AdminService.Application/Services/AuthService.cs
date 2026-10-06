using AdminService.Application.DTOs.Auth;
using AdminService.Application.Interfaces.Repositories;
using AdminService.Application.Interfaces.Services;
using AdminService.Domain.Entities;
using BCrypt.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdminService.Application.Services;

public class AuthService : IAuthService
{
    private readonly IAdminRepository _repository;

    private readonly IConfiguration _configuration;

    private readonly IEmailService _emailService;

    public AuthService(
        IAdminRepository repository,
        IConfiguration configuration,
        IEmailService emailService)
    {
        _repository = repository;

        _configuration = configuration;

        _emailService = emailService;
    }

    public async Task<LoginResponseDto> Login(
        LoginDto dto)
    {
        var user =
            await _repository
                .GetByUserNameAsync(
                    dto.UserName);

        if (user == null)
        {
            return new LoginResponseDto
            {
                Success = false,

                Message =
                    "Invalid username or password."
            };
        }

        if (user.IsLoggedIn)
        {
            bool sessionExpired =
                user.TokenExpiry.HasValue &&
                user.TokenExpiry <
                DateTime.UtcNow;

            if (!sessionExpired)
            {
                return new LoginResponseDto
                {
                    Success = false,

                    Message =
                        "You are already signed in on another device."
                };
            }
        }

        bool valid =
            BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.PasswordHash);

        if (!valid)
        {
            return new LoginResponseDto
            {
                Success = false,

                Message =
                    "Invalid username or password."
            };
        }

        var tokenLifetime =
            TimeSpan.FromMinutes(
                double.Parse(
                    _configuration[
                        "Jwt:ExpiresInMinutes"]!));

        user.IsLoggedIn = true;

        user.TokenExpiry =
            DateTime.UtcNow
            .Add(tokenLifetime);

        await _repository.UpdateAsync(user);

        return new LoginResponseDto
        {
            Success = true,

            Message =
                "Login successful.",

            Token =
                GenerateToken(user),

            UserName =
                user.UserName,

            Role =
                user.Role,

            FirstLogin =
                user.FirstLogin,

            FirstName =
                user.FirstName,

            LastName =
                user.LastName
        };
    }

    public async Task<bool> Logout(
        string username)
    {
        var user =
            await _repository
                .GetByUserNameAsync(
                    username);

        if (user == null)
        {
            return false;
        }

        user.IsLoggedIn = false;

        user.TokenExpiry = null;

        await _repository.UpdateAsync(user);

        return true;
    }

    private string GenerateToken(
        Admin user)
    {
        var claims = new[]
        {
            new Claim(
                ClaimTypes.Name,
                user.UserName),

            new Claim(
                ClaimTypes.Role,
                user.Role)
        };

        var key =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration[
                        "Jwt:Key"]!));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

        var token =
            new JwtSecurityToken(
                issuer:
                    _configuration[
                        "Jwt:Issuer"],

                audience:
                    _configuration[
                        "Jwt:Audience"],

                claims:
                    claims,

                expires:
                    DateTime.UtcNow.AddMinutes(
                        double.Parse(
                            _configuration[
                                "Jwt:ExpiresInMinutes"]!)),

                signingCredentials:
                    credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }

    public async Task<bool> ResetPassword(
        ResetPasswordDto dto)
    {
        var user =
            await _repository
                .GetByUserNameAsync(
                    dto.UserName);

        if (user == null)
        {
            return false;
        }

        bool valid =
            BCrypt.Net.BCrypt.Verify(
                dto.CurrentPassword,
                user.PasswordHash);

        if (!valid)
        {
            return false;
        }

        if (!user.FirstLogin)
        {
            return false;
        }

        user.PasswordHash =
            BCrypt.Net.BCrypt.HashPassword(
                dto.NewPassword);

        user.FirstLogin = false;

        await _repository.UpdateAsync(user);

        return true;
    }

    public async Task<ProfileDto?> GetProfile(
        string username)
    {
        var user =
            await _repository
                .GetByUserNameAsync(
                    username);

        if (user == null)
        {
            return null;
        }

        return new ProfileDto
        {
            Id =
                user.Id,

            FirstName =
                user.FirstName,

            LastName =
                user.LastName,

            Email =
                user.Email,

            MobileNo =
                user.MobileNo,

            Gender =
                user.Gender,

            Dob =
                user.Dob,

            Address =
                user.Address,

            Role =
                user.Role
        };
    }

    public async Task<bool> UpdateProfile(
        int id,
        ProfileDto dto)
    {
        var user =
            await _repository
                .GetByIdAsync(id);

        if (user == null)
        {
            return false;
        }

        user.FirstName =
            dto.FirstName;

        user.LastName =
            dto.LastName;

        user.Email =
            dto.Email;

        user.MobileNo =
            dto.MobileNo;

        user.Gender =
            dto.Gender;

        user.Dob =
            dto.Dob;

        user.Address =
            dto.Address;

        await _repository.UpdateAsync(user);

        return true;
    }

    public async Task<bool> Register(
        RegisterDto dto)
    {
        try
        {
            var exists =
                await _repository
                    .ExistsByUserNameAsync(
                        dto.UserName);

            if (exists)
            {
                return false;
            }

            var admin = new Admin
            {
                UserName =
                    dto.UserName,

                PasswordHash =
                    BCrypt.Net.BCrypt.HashPassword(
                        dto.Password),

                FirstName =
                    dto.FirstName,

                LastName =
                    dto.LastName,

                Email =
                    dto.Email,

                Gender =
                    dto.Gender,

                MobileNo =
                    dto.MobileNo,

                Role =
                    dto.Role,

                Address =
                    dto.Address,

                Dob =
                    dto.Dob,

                FirstLogin = true,

                IsLoggedIn = false
            };

            await _repository
                .AddAsync(admin);

            try
            {
                await _emailService.SendMail(
                    dto.Email,
                    dto.UserName,
                    dto.Password);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    "MAIL FAILED");

                Console.WriteLine(
                    ex.Message);
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                ex.Message);

            return false;
        }
    }
}