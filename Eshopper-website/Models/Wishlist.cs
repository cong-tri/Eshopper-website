using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models
{
	[Table("Wishlists")]
	public class Wishlist : BaseModel
	{
		[Key]
		public int WISH_ID { get; set; }

		[Required(ErrorMessage = "Please enter product id!")]
		[DisplayName("Product ID")]
		public required int PRO_ID {  get; set; }

		[Required(ErrorMessage = "Please enter user id!"), MaxLength(450)]
		[DisplayName("User ID")]
		public required string USER_ID { get; set; }

		[ForeignKey("PRO_ID")]
		public virtual Product? Product { get; set; }

	}
}
