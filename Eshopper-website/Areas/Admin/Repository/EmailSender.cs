using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace Eshopper_website.Areas.Admin.Repository;

public class EmailSender : IEmailSender
{
    private readonly string _smtpServer;
    private readonly int _port;
    private readonly bool _enableSsl;
    private readonly string _userName;
    private readonly string _password;

    public EmailSender(IConfiguration configuration)
    {
        _smtpServer = configuration["EmailConfiguration:SmtpServer"] ?? "smtp.gmail.com";
        _port = int.Parse(configuration["EmailConfiguration:Port"] ?? "587");
        _enableSsl = bool.Parse(configuration["EmailConfiguration:EnableSsl"] ?? "true");
        _userName = configuration["EmailConfiguration:UserName"] ?? 
            throw new ArgumentNullException("EmailConfiguration:UserName");
        _password = configuration["EmailConfiguration:Password"] ?? 
            throw new ArgumentNullException("EmailConfiguration:Password");
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        using var client = new SmtpClient(_smtpServer, _port)
        {
            EnableSsl = _enableSsl,
            Credentials = new NetworkCredential(_userName, _password)
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(_userName, "EShopper"),
            Subject = subject,
            Body = message,
            IsBodyHtml = true
        };
        mailMessage.To.Add(email);

        try 
        {
            await client.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            // Log error
            throw new Exception($"Failed to send email: {ex.Message}");
        }
    }
}
