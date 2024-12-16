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

        public int ACSL_Status { get; set; }

        [Required(ErrorMessage = "Please enter datetime account login!")]
        [DisplayName("Datetime Login")]
        public required DateTime ACSL_DatetimeLogin { get; set; } = DateTime.Now;

        [DisplayName("Datetime Login")]
        public DateTime? ACSL_ExpiredDatetimeLogin { get; set; } = null;

        [ForeignKey("ACC_ID")]
        public virtual Account? Account { get; set; }
    }
}
