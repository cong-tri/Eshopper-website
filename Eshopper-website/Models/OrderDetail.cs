using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models
{
	[Table("OrderDetails")]
	public class OrderDetail : BaseModel
	{
		[Key]
		public int ORDE_ID { get; set; }

		[Required(ErrorMessage = "Please enter order id!")]
		[DisplayName("Order ID")]
		public required int ORD_ID { get; set; }

		[Required(ErrorMessage = "Please enter product id!")]
		[DisplayName("Product ID")]
		public required int PRO_ID { get; set; }

		[Required(ErrorMessage = "Please enter order details price!")]
		[Range(1, 1000000000, ErrorMessage = "Product price must be a positive number!")]
		[DisplayName("Price")]
		public required decimal ORDE_Price { get; set; }

		[Required(ErrorMessage = "Please enter order details quantity!")]
		[Range(1, 100, ErrorMessage = "Product price must be a positive number!")]
		[DisplayName("Quantity")]
		public required int ORDE_Quantity { get; set; }

		[ForeignKey("PRO_ID")]
		public virtual Product? Product { get; set; }

        [ForeignKey("ORD_ID")]
        public virtual account? Order { get; set; }
    }
}
