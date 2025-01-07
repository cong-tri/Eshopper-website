using Eshopper_website.Utils.Enum;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models
{
    [Table("AccountStatusLogins")]
    public class AccountStatusLogin : BaseModel
    {
        [Key]
        public int ACSL_ID { get; set; }

        [Required(ErrorMessage = "Please enter account id!")]
        [DisplayName("Account ID")]
        public int ACC_ID { get; set; }

        [Required(ErrorMessage = "Please enter jwt token!")]
        [MinLength(5, ErrorMessage = "JWT Token must be at least 5 characters long!")]
        [DisplayName("JWT Token")]
        public required string ACSL_JwtToken { get; set; }

        [Required(ErrorMessage = "Please enter account status login!"), Column(TypeName = "INT")]
        [DisplayName("Status")]
        public required AccountStatusLoginEnum ACSL_Status { get; set; } = AccountStatusLoginEnum.Active;

        [Required(ErrorMessage = "Please enter datetime account login!")]
        [DisplayName("Datetime Login")]
        public required DateTime ACSL_DatetimeLogin { get; set; } = DateTime.Now;

        [DisplayName("Expired Datetime Login")]
        public DateTime? ACSL_ExpiredDatetimeLogin { get; set; } = null;

        [ForeignKey("ACC_ID")]
        public virtual Account? Account { get; set; }
    }
}
