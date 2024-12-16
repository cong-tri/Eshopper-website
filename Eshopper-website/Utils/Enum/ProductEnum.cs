using System.ComponentModel.DataAnnotations;

namespace Eshopper_website.Utils.Enum
{
	public enum ProductStatusEnum
	{
		/* FOR PRO_Status: 1: In Stock; 2: Out of Stock; 3: Low Stock; 4: Back Order; 5: Pre-Order*/
		[Display(Name = "In Stock")]
		InStock = 1,

		[Display(Name = "Out of Stock")]
		OutOfStock = 2,

		[Display(Name = "Low Stock")]
		LowStock = 3,

		[Display(Name = "Back Order")]
		BackOrder = 4,

		[Display(Name = "Pre-Order")]
		PreOrder = 5
	}
}
