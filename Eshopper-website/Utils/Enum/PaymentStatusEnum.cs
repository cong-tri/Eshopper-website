using System.ComponentModel.DataAnnotations;

namespace Eshopper_website.Utils.Enum
{
	public enum PaymentStatusEnum
	{
		Active = 1,

		[Display(Name = "In Active")]
		Inactive = 2,
		Maintenance = 3
	}
}
