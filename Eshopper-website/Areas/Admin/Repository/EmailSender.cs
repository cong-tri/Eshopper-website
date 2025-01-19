using System.Net;
using System.Net.Mail;
using Eshopper_website.Areas.Admin.Models.SendEmail;
using Eshopper_website.Utils.Constant;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Eshopper_website.Areas.Admin.Repository;

public class EmailSender : IEmailSender
{
    private readonly IOptions<MailConfig> _options;
    private readonly string _smtpServer;
    private readonly int _port;
    private readonly bool _enableSsl;
    private readonly string _userName;
    private readonly string _password;
    private readonly string _displayName = "EShopper Electronics";

    public EmailSender(IConfiguration configuration, IOptions<MailConfig> options)
    {
        _options = options;

        _smtpServer = options.Value.MailHost;
            //configuration["EmailConfiguration:SmtpServer"] ?? "smtp.gmail.com";

        _port = options.Value.MailPort;
        //int.Parse(configuration["EmailConfiguration:Port"] ?? "587");

        _enableSsl = true;
        //bool.Parse(configuration["EmailConfiguration:EnableSsl"] ?? "true");

        _userName = options.Value.Sender;
            //configuration["EmailConfiguration:UserName"] ?? 
            //throw new ArgumentNullException("EmailConfiguration:UserName");

        _password = options.Value.AppPassword;
            //configuration["EmailConfiguration:Password"] ?? 
            //throw new ArgumentNullException("EmailConfiguration:Password");
    }

    public async Task<MailResponse> SendEmailAsync(string email, string subject, string body)
    {
        try
        {
            var client = new SmtpClient(_smtpServer, _port)
            {
                Port = _port,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_userName, _password)
            };

            var msg = new MailMessage()
            {
                From = new MailAddress(_userName, _displayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            msg.To.Add(email);

            await client.SendMailAsync(msg);

            return new MailResponse
            {
                Code = 200,
                Message = "Send Maill Successfully!"
            };
        }
        catch (Exception ex)
        {
            return new MailResponse
            {
                Code = 404,
                Message = $"Failed to send email: {ex.Message}"
            };
        }
    }

    //public async Task<bool> TrySendEmailAsync(string email, string subject, string body)
    //{
    //    try
    //    {
    //        var client = new SmtpClient(EShopperConstant.serverMailHost, EShopperConstant.serverMailPort)
    //        {
    //            Port = EShopperConstant.serverMailPort,
    //            EnableSsl = true,
    //            UseDefaultCredentials = false,
    //            Credentials = new NetworkCredential(EShopperConstant.sendEmail, EShopperConstant.sendAppPassword)
    //        };

    //        var msg = new MailMessage()
    //        {
    //            From = new MailAddress(EShopperConstant.sendEmail, "EShopper"),
    //            Subject = subject,
    //            Body = body,
    //            IsBodyHtml = true,
    //        };
    //        msg.To.Add(email);

    //        await client.SendMailAsync(msg);
    //        return true;
    //    }
    //    catch (Exception)
    //    {
    //        return false;
    //    }
    //}
}
