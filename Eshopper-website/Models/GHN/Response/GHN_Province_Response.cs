namespace Eshopper_website.Models.GHN.Response
{
    public class GHN_Province_Response
    {
        public int code { get; set; }
        public string? message { get; set; }
        public List<GHNProvinceResponseData>? data { get; set; }
    }

    public class GHNProvinceResponseData
    {
        public int ProvinceID { get; set; }
        public required string ProvinceName { get; set; }
        public int? CountryID { get; set; }
        public object? Code { get; set; }
        public int Status { get; set; }
    }
}
