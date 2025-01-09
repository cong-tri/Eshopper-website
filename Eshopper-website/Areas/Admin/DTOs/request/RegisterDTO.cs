namespace Eshopper_website.Areas.Admin.DTOs.request
{
    public class RegisterDTO
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string ConfirmedPassword { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
        public required string DisplayName { get; set; }
    }
}
