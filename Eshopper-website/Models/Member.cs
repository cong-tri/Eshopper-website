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

		[MaxLength(255, ErrorMessage = "Lastname cannot exceed 255 characters!")]
		[DisplayName("Lastname")]
        public string? MEM_LastName { get; set; } = string.Empty;

		[MaxLength(255, ErrorMessage = "Firstname cannot exceed 255 characters!")]
		[DisplayName("Firstname")]
        public string? MEM_FirstName {  get; set; } = string.Empty;

        [Column(TypeName = "INT")]
        [DisplayName("Gender")]
        public MemberGenderEnum MEM_Gender { get; set; } = MemberGenderEnum.Other;

        [Required(ErrorMessage = "Please enter phone number!")]
		[DataType(DataType.PhoneNumber, ErrorMessage = "Please enter the correct format phone number!")]
		[MinLength(10, ErrorMessage = "Phone must be at least 10 characters long!")]
		[MaxLength(11, ErrorMessage = "Phone cannot exceed 11 characters!")]
		[DisplayName("Phone")]
        public required string MEM_Phone { get; set; }

        [Required(ErrorMessage = "Please enter email!")]
		[DataType(DataType.EmailAddress, ErrorMessage = "Please enter the correct format email!")]
		[MinLength(10, ErrorMessage = "Email must be at least 10 characters long!")]
		[MaxLength(255, ErrorMessage = "Email cannot exceed 255 characters!")]
		[DisplayName("Email")]
        public required string MEM_Email {  get; set; }

		[MaxLength(255, ErrorMessage = "Address cannot exceed 255 characters!")]
		[DisplayName("Address")]
        public string? MEM_Address {  get; set; } = string.Empty;

        [Column(TypeName = "INT")]
        [DisplayName("Status")]
        public MemberStatusEnum MEM_Status { get; set; } = MemberStatusEnum.Active;

        [ForeignKey("ACC_ID")]
        public Account? Account { get; set; }

        [ForeignKey("ACR_ID")]
        public AccountRole? AccountRole { get; set; }

        public virtual ICollection<Wishlist>? Wishlists { get; set; }
        public virtual ICollection<Compare>? Compares { get; set; }
        public virtual ICollection<account>? Orders { get; set; }
        
    }
}
