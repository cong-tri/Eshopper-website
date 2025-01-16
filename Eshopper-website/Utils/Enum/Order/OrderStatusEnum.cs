using System.ComponentModel.DataAnnotations;

namespace Eshopper_website.Utils.Enum.Order
{
    public enum OrderStatusEnum
    {
        Pending = 0,
        Processing = 1,
        Completed = 2,
        Failed = 3,
        Cancelled = 4,
        WaitingForPayment = 5
    }
}
