using System.Net.Mail;
using System.Net;
using Eshopper_website.Utils.Constant;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Eshopper_website.Areas.Admin.Repository
{
    public class EmailSender : IEmailSender
    {
        
        public EmailSender() { }
        public async Task SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                var constant = new EShopperConstant();

                var client = new SmtpClient(constant.serverMailHost, constant.serverMailPort)
                {
                    Port = constant.serverMailPort,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(constant.sendEmail, constant.sendAppPassword)
                };

                var msg = new MailMessage()
                {
                    From = new MailAddress(constant.sendEmail, "EShopper"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                msg.To.Add(email);

                await client.SendMailAsync(msg);
            }
            catch (Exception ex) {
                throw new Exception($"Error: " + ex.Message);
            }
        }
    }
}
