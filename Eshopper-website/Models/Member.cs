using Eshopper_website.Utils.Enum.Member;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models
{
    [Table("Members")]
    public class Member : BaseModel
    {
        [Key]
        public int MEM_ID { get; set; }

        [Required(ErrorMessage = "Please enter account id!")]
        [DisplayName("Account ID")]
        public required int ACC_ID { get; set; }

        [Required(ErrorMessage = "Please enter role id!")]
        [DisplayName("Role ID")]
        public required int ACR_ID { get; set; } = 1;

        [MaxLength(255)]
        [DisplayName("Lastname")]
        public string? MEM_LastName { get; set; } = string.Empty;

        [MaxLength(255)]
        [DisplayName("Firstname")]
        public string? MEM_FirstName {  get; set; } = string.Empty;

        [Column(TypeName = "INT")]
        [DisplayName("Gender")]
        public MemberGenderEnum MEM_Gender { get; set; } = MemberGenderEnum.Orther;

        [Required(ErrorMessage = "Please enter phone number!"), MinLength(10), MaxLength(20), DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\+?[1-9]\d{1,14}$", ErrorMessage = "Invalid phone number format.")]
        [DisplayName("Phone")]
        public required string MEM_Phone { get; set; }

        [Required(ErrorMessage = "Please enter email!"), MinLength(10), MaxLength(255), DataType(DataType.EmailAddress)]
        [DisplayName("Email")]
        public required string MEM_Email {  get; set; }

        [MaxLength(255)]
        [DisplayName("Address")]
        public string? MEM_Address {  get; set; } = string.Empty;

        [Column(TypeName = "INT")]
        [DisplayName("Status")]
        public MemberStatusEnum MEM_Status { get; set; } = MemberStatusEnum.Active;

        [ForeignKey("ACC_ID")]
        public Account? Account { get; set; }

        [ForeignKey("ACR_ID")]
        public AccountRole? AcountRole { get; set; }

        public virtual ICollection<Wishlist>? Wishlists { get; set; }
        public virtual ICollection<Compare>? Compares { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }


    }
}
