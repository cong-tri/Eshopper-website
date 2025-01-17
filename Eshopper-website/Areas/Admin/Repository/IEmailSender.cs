namespace Eshopper_website.Areas.Admin.Repository;

public interface IEmailSender
{
    Task SendEmailAsync(string email, string subject, string body);
}
