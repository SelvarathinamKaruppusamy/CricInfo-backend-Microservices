using AdminService.Application.Interfaces.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace AdminService.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(
        IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendMail(
        string toEmail,
        string userName,
        string password)
    {
        string from =
            _configuration[
                "EmailSettings:From"]!;

        string host =
            _configuration[
                "EmailSettings:Host"]!;

        int port =
            int.Parse(
                _configuration[
                    "EmailSettings:Port"]!);

        string smtpUser =
            _configuration[
                "EmailSettings:Username"]!;

        string smtpPassword =
            _configuration[
                "EmailSettings:Password"]!;

        var email =
            new MimeMessage();

        email.From.Add(
            MailboxAddress.Parse(from));

        email.To.Add(
            MailboxAddress.Parse(toEmail));

        email.Subject =
            "CricInfo Admin Account Created";

        email.Body =
            new TextPart("plain")
            {
                Text = $@"
Hello {userName},

Your CricInfo Admin account has been created.

-----------------------------------

Username : {userName}

Password : {password}

-----------------------------------

Login URL:
http://localhost:4200/admin

Please change your password after your first login.

Regards,
CricInfo Team
"
            };

        using var smtp =
            new SmtpClient();

        Console.WriteLine(
            $"Connecting to {host}:{port} using STARTTLS...");

        await smtp.ConnectAsync(
            host,
            port,
            SecureSocketOptions.StartTls);

        Console.WriteLine(
            "SMTP connection established.");

        await smtp.AuthenticateAsync(
            smtpUser,
            smtpPassword);

        Console.WriteLine(
            "SMTP authentication successful.");

        await smtp.SendAsync(
            email);

        Console.WriteLine(
            "Email sent successfully.");

        await smtp.DisconnectAsync(
            true);
    }
}