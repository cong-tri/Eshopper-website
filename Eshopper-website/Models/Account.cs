using Eshopper_website.Utils.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models
{
    [Table("Accounts")]
    public class Account : BaseModel
    {
        [Key]
        public int ACC_ID { get; set; }

        [Required(ErrorMessage = "Please enter username!"), MinLength(5), MaxLength(255)]
        [DisplayName("Username")]
        public required string ACC_Username { get; set; }

        [Required(ErrorMessage = "Please enter password!"), MinLength(8), MaxLength(255), DataType(DataType.Password)]
        [DisplayName("Password")]
        public required string ACC_Password { get; set; }

        [Required(ErrorMessage = "Please enter display name!"), MinLength(5), MaxLength(255)]
        [DisplayName("Display Name")]
        public required string ACC_DisplayName { get; set; }

        [Required(ErrorMessage = "Please enter email!"), MinLength(10), MaxLength(255), DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public required string ACC_Email { get; set; }

        [Required(ErrorMessage = "Please enter phone number!"), MinLength(10), MaxLength(255), DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number format.")]
        [DisplayName("Phone")]
        public required string ACC_Phone { get; set; }

        [Required(ErrorMessage = "Please enter status!")]
        [DisplayName("Status")]
        public required AccountStatusEnum ACC_Status { get; set; } = AccountStatusEnum.Inactive;

        public virtual ICollection<Member>? Members { get; set; }
        public virtual ICollection<AccountStatusLogin>? AccountStatusLogins { get; set; }
    }
}
