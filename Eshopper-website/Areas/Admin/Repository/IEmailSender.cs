namespace Eshopper_website.Areas.Admin.Repository
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email,string subkect,string message); // Subject : tiêu đề 
    }
}
