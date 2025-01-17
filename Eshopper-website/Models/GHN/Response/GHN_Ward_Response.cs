namespace Eshopper_website.Models.GHN.Response
{
    public class GHN_Ward_Response
    {
        public int code { get; set; }
        public string? message { get; set; }
        public List<GHNWardResponseData>? data { get; set; }
    }
    public class GHNWardResponseData
    {
        public string WardCode { get; set; }
        public int DistrictID { get; set; }
        public string WardName { get; set; }
    }
}
