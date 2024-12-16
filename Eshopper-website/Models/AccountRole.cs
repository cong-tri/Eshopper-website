using Eshopper_website.Utils.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models
{
    [Table("AccountRoles")]
    public class AccountRole : BaseModel
    {
        [Key]
        public int ACR_ID { get; set; }

        [Required(ErrorMessage = "Please enter role name!"), MinLength(4), MaxLength(255)]
        [DisplayName("Name")]
        public required string ACR_Name { get; set; }

        [Required(ErrorMessage = "Please enter role status!"), Column(TypeName = "INT")]
        [DisplayName("Status")]
        public required AccountRoleStatusEnum ACR_Status { get; set; } = AccountRoleStatusEnum.Active;

        public virtual ICollection<Member>? Members { get; set; }
    }
}
