using System.ComponentModel.DataAnnotations;

namespace Eshopper_website.Utils.Enum.Member
{
    public enum MemberStatusEnum
    {
        Active = 1,
        [Display(Name = "In Active")]
        Inactive = 2,
    }
}
