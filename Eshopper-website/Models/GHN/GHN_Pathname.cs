namespace Eshopper_website.Models.GHN
{
    public class GHN_Pathname
    {
        public const string ghn_shipping_create_order = "shiip/public-api/v2/shipping-order/create";
        public const string ghn_shipping_get_district = "shiip/public-api/master-data/district";
        public const string ghn_shipping_get_ward = "shiip/public-api/master-data/ward?district_id";
        public const string ghn_shipping_get_provice = "shiip/public-api/master-data/province";
        public const string ghn_shipping_get_service = "shiip/public-api/v2/shipping-order/available-services";
        public const string ghn_shipping_pick_shift = "shiip/public-api/v2/shift/date";
    }
}
