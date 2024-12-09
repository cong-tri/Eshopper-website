using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models.Identity
{
    //[Table("AspNetUserRoles")]
    public class NetUserRole
    {
        //[Key, Column(Order = 0)]
        //[Required(ErrorMessage = "Please enter user id!"), MaxLength(450)]
        //[DisplayName("User ID")]
        //public required string UserId { get; set; }

        //[Key, Column(Order = 1)]
        //[Required(ErrorMessage = "Please enter role id!"), MaxLength(450)]
        //[DisplayName("Role ID")]
        //public required string RoleId { get; set; }

        //[ForeignKey("UserId")]
        //public virtual AppUser? AppUser { get; set; }

        //[ForeignKey("RoleId")]
        //public virtual NetRole? NetRole { get; set; }
    }
}
