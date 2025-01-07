using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models
{
    [Table("AccountLogins")]
    public class AccountLogin
    {
        [Key]
        public required string LoginProvider {  get; set; }

        [Key]
        public required string ProviderKey {  get; set; }
        public required string ProviderDisplayName {  get; set; }
        public required int ACC_ID { get; set; }

        [ForeignKey("ACC_ID")]
        public virtual Account? Account { get; set; }
    }
}
