using System.ComponentModel.DataAnnotations;

namespace Eshopper_website.Utils.Enum
{
    public enum CategoryStatusEnum
    {
        Active = 1,
        [Display(Name = "In Active")]
        Inactive = 2,
    }
}
