using System.ComponentModel.DataAnnotations;

namespace Eshopper_website.Utils.Enum.Order
{
    public enum OrderPaymentMethodEnum
    {
        Cash = 1,
        Banking = 2,
        Installment = 3,
        Pending = 4,
        Completed = 5,
        Failed = 6,
        Refunded = 7,
        Canceled = 8,
        Verified = 9,
        Unverified = 10,

        [Display(Name = "Awaiting Confirmation")]
        AwaitingConfirmation = 11
    }
}
