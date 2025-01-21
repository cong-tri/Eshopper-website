using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models.GHN
{
	[Table("OrderGHN")]
	public class OrderGHN : BaseModel
	{
		[Key]
		public int Id { get; set; }

		[DisplayName("Order Code")]
		public required string OrderCode { get; set; }

		[DisplayName("Sort Code")]
		public required string SortCode { get; set; }

		[DisplayName("Total Fee")]
		public int TotalFee { get; set; }

		[DisplayName("Expected Delivery Time")]
		public DateTime ExpectedDeliveryTime { get; set; }

		[DisplayName("Trans Type")]
		public required string TransType { get; set; }
	}
}
