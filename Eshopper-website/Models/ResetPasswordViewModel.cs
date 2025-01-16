using System.ComponentModel.DataAnnotations;

namespace Eshopper_website.Models;

public class ResetPasswordViewModel
{
    public required string Token { get; set; }

    [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới")]
    [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
    public required string NewPassword { get; set; } = string.Empty;

    [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp")]
    public required string ConfirmPassword { get; set; } = string.Empty;
} 