using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models.Identity
{
    //[Table("AspNetUserTokens")]
    public class NetUserToken
    {
        //[Key, Column(Order = 0)]
        //[Required(ErrorMessage = "Please enter user id!"), MaxLength(450)]
        //[DisplayName("User ID")]
        //public required string UserId { get; set; }

        //[Key, Column(Order = 1)]
        //[Required(ErrorMessage = "Please enter login provider!"), MaxLength(450)]
        //[DisplayName("Login Provider")]
        //public required string LoginProvider { get; set; }

        //[Key, Column(Order = 2)]
        //[Required(ErrorMessage = "Please enter name!"), MaxLength(450)]
        //public required string Name { get; set; }

        //public string? Value { get; set; } = string.Empty;

        //[ForeignKey("UserId")]
        //public virtual AppUser? AppUser { get; set; }
    }
}
