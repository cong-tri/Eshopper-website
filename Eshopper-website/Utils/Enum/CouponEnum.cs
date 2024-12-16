using System.ComponentModel.DataAnnotations;

namespace Eshopper_website.Utils.Enum
{
    public enum CouponStatusEnum
    {
       Active = 1,

       [Display(Name = "In Active")]
       Inactive = 2,

       Expired = 3,

       Redeemed = 4,

       [Display(Name = "Un Published")]
       Unpublished = 5,

       Limited = 6,

       [Display(Name = "Free Shipping")]
       FreeShipping = 7,

       [Display(Name = "Cash Back")]
       CashBack = 8
    }
}
