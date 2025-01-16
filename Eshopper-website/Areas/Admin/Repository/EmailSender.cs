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
            catch (Exception ex) {
                throw new Exception($"Error: " + ex.Message);
            }
        }
    }
}
