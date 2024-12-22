using System.ComponentModel.DataAnnotations;

namespace Eshopper_website.Utils.Enum.Order
{
    public enum OrderStatusEnum
    {
        Pending = 1,

        Confirmed = 2,

        Processing = 3,

        Completed = 4,

        Canceled = 5,

        Failed = 6,

        [Display(Name = "On Hold")]
        OnHold = 7,

        [Display(Name = "Awaiting Shipment")]
        AwaitingShipment = 8,

        Shipped = 9,

        [Display(Name = "In Transit")]
        InTransit = 10,

        Delivered = 11,

        Returned = 12,

        Lost = 13,

        Paid = 14,

        Unpaid = 15,

        Refunded = 16
    }
}
