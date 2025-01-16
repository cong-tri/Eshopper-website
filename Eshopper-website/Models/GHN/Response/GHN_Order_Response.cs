namespace Eshopper_website.Models.GHN.Response
{
    public class GHN_Order_Response
    {
        public int Code { get; set; }
        public string? Message { get; set; }
        public GHNOrderResponseData? Data { get; set; }
    }

    public class GHNOrderResponseData
    {
        public string? OrderCode { get; set; }
        public string? SortCode { get; set; }
        public string? TransactionId { get; set; }
        public double TotalFee { get; set; }
        public double ExpectedDeliveryTime { get; set; }
        public string? OrderUrl { get; set; }
    }
}
