namespace Eshopper_website.Areas.Admin.DTOs.request
{
    public class NewPassDTO
    {
        public required string Email { get; set; }
        public required string Token { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }

    }
}
