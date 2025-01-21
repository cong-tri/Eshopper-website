using Humanizer;

namespace Eshopper_website.Utils.Enum.Order
{
    public enum OrderStatusEnum
    {
        Pending = 0,
        WaitingForPayment = 1,
        Processing = 2,
        Confirmed = 3,
        Completed = 4,
        Cancelled = 5,
        Failed = 6,
        OnHold = 7,
        AwaitingShipment = 8,
        Shipped = 9,
        InTransit = 10,
        Delivered = 11,
        Returned = 12,
        Lost = 13,
        Paid = 14,
        Unpaid = 15,
        Refunded = 16,
    }
}
