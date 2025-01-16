using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Eshopper_website.Models
{
	[Table("Statisticals")]
	public class Statistical : BaseModel
	{
		[Key]
		public int STA_ID { get; set; }

		[DisplayName("Quantity")]
		public int? STA_Quantity { get; set; } //Số lượng bán 

		[DisplayName("Status")]
		public int STA_Status { get; set; }

		[DisplayName("Revenue")]
		[Column(TypeName = "decimal(18,2)")]
		public int? STA_Revenue { get; set; }

		[DisplayName("Profit")]
		[Column(TypeName = "decimal(18,2)")]
		public int? STA_Profit { get; set; }

		[DisplayName("Sold")]
		public int? STA_Sold { get; set; } // So lg don hang

	}
}
