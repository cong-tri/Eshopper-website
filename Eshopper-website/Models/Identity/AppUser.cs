using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models.Identity
{
    [Table("AspNetUsers")]
    public class AppUser : IdentityUser
    {
        //[Key]
        //[MaxLength(450)]
        //public required string Id { get; set; }

        [MaxLength(255)]
        public string? Occupation { get; set; } = string.Empty;

        //[MaxLength(256)]
        //public string? UserName { get; set; } = string.Empty;

        //[MaxLength(256)]
        //[DisplayName("Normalized UserName")]
        //public string? NormalizedUserName { get; set; } = string.Empty;

        //[MaxLength(256)]
        //public string? Email { get; set; } = string.Empty;

        //[MaxLength(256)]
        //[DisplayName("Normalized Email")]
        //public string? NormalizedEmail { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Please enter email confirmed!")]
        //[DisplayName("Email Confirmed")]
        //public required bool EmailConfirmed { get; set; }

        //[DisplayName("Password Hash")]
        //public string? PasswordHash { get; set; } = string.Empty;

        //[DisplayName("Security Stamp")]
        //public string? SecurityStamp { get; set; } = string.Empty;

        //[DisplayName("Concurrency Stamp")]
        //public string? ConcurrencyStamp { get; set; } = string.Empty;

        //[DisplayName("Phone Number")]
        //public string? PhoneNumber { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Please enter phone number confirmed!")]
        //[DisplayName("Phone Number Confirmed")]
        //public required bool PhoneNumberConfirmed { get; set; }

        //[Required(ErrorMessage = "Please enter two factor enabled!")]
        //[DisplayName("Two Factor Enabled")]
        //public required bool TwoFactorEnabled { get; set; }

        //[DisplayName("Lockout End")]
        //public DateTimeOffset? LockoutEnd { get; set; }

        //[Required(ErrorMessage = "Please enter lockout enabled!")]
        //[DisplayName("Lockout Enabled")]
        //public required bool LockoutEnabled { get; set; }

        //[Required(ErrorMessage = "Please enter access failed count!")]
        //[DisplayName("Access Failed Count")]
        //public required int AccessFailedCount { get; set; }

        //public virtual ICollection<NetUserLogin>? NetUserLogins { get; set; }
        //public virtual ICollection<NetUserRole>? NetUserRoles { get; set; }
        //public virtual ICollection<NetUserToken>? NetUserTokens { get; set; }
        //public virtual ICollection<NetUserClaim>? NetUserClaims { get; set; }
    }
}
