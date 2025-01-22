using Humanizer;

namespace Eshopper_website.Utils.Enum.Order
{
    public enum OrderStatusEnum
    {
        Pending = 1,
        WaitingForPayment = 2,
        Processing = 3,
        Confirmed = 4,
        Completed = 5,
        Cancelled = 6,
        Failed = 7,
        OnHold = 8,
        AwaitingShipment = 9,
        Shipped = 10,
        InTransit = 11,
        Delivered = 12,
        Returned = 13,
        Lost = 14,
        Paid = 15,
        Unpaid = 16,
        //Refunded = 17,
    }
}
