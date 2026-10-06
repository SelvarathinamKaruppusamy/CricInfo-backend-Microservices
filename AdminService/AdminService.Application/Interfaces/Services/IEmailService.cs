namespace AdminService.Application.Interfaces.Services;

public interface IEmailService
{
    Task SendMail(
        string toEmail,
        string userName,
        string password);
}