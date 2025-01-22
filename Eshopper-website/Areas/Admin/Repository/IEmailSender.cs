using Eshopper_website.Areas.Admin.Models.SendEmail;

namespace Eshopper_website.Areas.Admin.Repository;

public interface IEmailSender
{
    //Task<MailResponse> SendEmailAsync(string email, string subject, string body);
    Task SendEmailAsync(string email, string subject, string body);
}
