using System.Net;
using System.Net.Mail;
using Eshopper_website.Utils.Constant;
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

    public async Task SendEmailAsync(string email, string subject, string body)
    {
        try
        {
            var client = new SmtpClient(EShopperConstant.serverMailHost, EShopperConstant.serverMailPort)
            {
                Port = EShopperConstant.serverMailPort,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(EShopperConstant.sendEmail, EShopperConstant.sendAppPassword)
            };

            var msg = new MailMessage()
            {
                From = new MailAddress(EShopperConstant.sendEmail, "EShopper"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            msg.To.Add(email);

            await client.SendMailAsync(msg);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to send email: {ex.Message}");
        }
    }
}
