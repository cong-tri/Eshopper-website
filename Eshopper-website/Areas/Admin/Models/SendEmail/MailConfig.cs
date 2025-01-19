namespace Eshopper_website.Areas.Admin.Models.SendEmail
{
    public class MailConfig
    {
        public string? MailHost { get; set; }
        public int MailPort { get; set; }
        public string? Sender { get; set; }
        public string? AppPassword { get; set; }
    }
}
