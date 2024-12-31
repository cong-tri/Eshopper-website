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

        [Required(ErrorMessage = "Please enter username!")]
        [MinLength(5, ErrorMessage = "Username must be at least 5 characters long!")]
        [MaxLength(30, ErrorMessage = "Username cannot exceed 30 characters!")]
        [DisplayName("Username")]
        public required string ACC_Username { get; set; }

        [Required(ErrorMessage = "Please enter password!"), DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters long!")]
        [MaxLength(20, ErrorMessage = "Password cannot exceed 20 characters!")]
        [DisplayName("Password")]
        public required string ACC_Password { get; set; }

        [Required(ErrorMessage = "Please enter display name!")]
        [MinLength(5, ErrorMessage = "Display name must be at least 5 characters long!")]
        [MaxLength(50, ErrorMessage = "Display name cannot exceed 50 characters!")]
        [DisplayName("Display Name")]
        public required string ACC_DisplayName { get; set; }

        [Required(ErrorMessage = "Please enter email!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please enter correct format email!")]
        [MinLength(10, ErrorMessage = "Email must be at least 5 characters long!")]
        [MaxLength(30, ErrorMessage = "Email cannot exceed 30 characters!")]
        [DisplayName("Email")]
        public required string ACC_Email { get; set; }

        [Required(ErrorMessage = "Please enter phone number!")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "Please enter the correct format phone number!")]
        [MinLength(10, ErrorMessage = "Phone must be at least 10 characters long!")]
        [MaxLength(12, ErrorMessage = "Phone cannot exceed 12 characters!")]
        [DisplayName("Phone")]
        public required string ACC_Phone { get; set; }

        [Required(ErrorMessage = "Please enter status!")]
        [DisplayName("Status")]
        public required AccountStatusEnum ACC_Status { get; set; } = AccountStatusEnum.Inactive;

        public virtual ICollection<Member>? Members { get; set; }
        public virtual ICollection<AccountStatusLogin>? AccountStatusLogins { get; set; }
    }
}
