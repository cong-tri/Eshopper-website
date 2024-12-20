using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models
{
	[Table("Contact")]
	public class Contact : BaseModel
	{
		[Key]
		public int CONT_ID { get; set; }

		[Required(ErrorMessage = "Please enter the name"), MinLength(5), MaxLength(255)]
		[DisplayName("Name")]
		public required string CONT_Name { get; set; }

		[Required(ErrorMessage = "Please enter the description"), MinLength(5), MaxLength(255)]
		[DisplayName("Description")]
		public required string CONT_Description { get; set; }

		[Required(ErrorMessage = "Please enter the phone"), MinLength(5), MaxLength(255)]
		[DisplayName("Phone")]
		public required string CONT_Phone { get; set; }

		[Required(ErrorMessage = "Please enter the email"), MinLength(5), MaxLength(255)]
		[DisplayName("Email")]
		public required string CONT_Email { get; set; }

		[Required(ErrorMessage = "Please enter the map"), MinLength(5)]
		[DisplayName("Map")]
		public required string CONT_Map { get; set; }

		[MaxLength(255)]
		[DisplayName("Logo Img")]
		public string? CONT_LogoImg { get; set; } = string.Empty;

		[Required(ErrorMessage = "Please enter the address"), MinLength(5), MaxLength(255)]
		[DisplayName("Address")]
		public required string CONT_Address { get; set; }
	}
}
