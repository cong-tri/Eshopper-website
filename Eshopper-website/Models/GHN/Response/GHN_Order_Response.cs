namespace Eshopper_website.Models.GHN.Response
{
    public class GHN_Order_Response
    {
        public int code { get; set; }
        public string? message { get; set; }
        public GHNOrderResponseData? data { get; set; }
    }

    public class GHNOrderResponseData
    {
        public string? order_code { get; set; }
        public string? sort_code { get; set; }
        public double total_fee { get; set; }
        public DateTime expected_delivery_time { get; set; }
        public string? trans_type { get; set; }
    }
}
