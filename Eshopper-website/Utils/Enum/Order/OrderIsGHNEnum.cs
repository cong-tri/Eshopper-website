using System.ComponentModel.DataAnnotations;

namespace Eshopper_website.Utils.Enum.Order
{
    public enum OrderIsGHNEnum
    {
        Active = 1,

        [Display(Name = "In Active")]
        Inactive = 2,
    }
}
