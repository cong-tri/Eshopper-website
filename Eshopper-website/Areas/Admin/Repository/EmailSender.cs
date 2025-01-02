using System.Net.Mail;
using System.Net;
using Eshopper_website.Utils.Constant;

namespace Eshopper_website.Areas.Admin.Repository
{
    public class EmailSender : IEmailSender
    {
        
        public EmailSender() { }
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var constant = new EShopperConstant();

            var client = new SmtpClient(constant.serverMailHost, constant.serverMailPort)
            {
                EnableSsl = true, //bật bảo mật
                UseDefaultCredentials = false,
                
                Credentials = new NetworkCredential(constant.sendEmail, constant.sendAppPassword)
                // Thay đổi email, pass 
            };

            var msg = new MailMessage(
                from: constant.sendEmail,
                to: email,
                subject,
                message
            );

            msg.IsBodyHtml = true;
            return client.SendMailAsync(
                msg
            );
        }
    }
}
