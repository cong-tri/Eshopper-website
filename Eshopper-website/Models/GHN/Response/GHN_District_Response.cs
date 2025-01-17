namespace Eshopper_website.Models.GHN.Response
{
    public class GHN_District_Response
    {
        public int code { get; set; }
        public string? message { get; set; }
        public List<GHNDistrictResponseData>? data { get; set; }
    }

    public class GHNDistrictResponseData
    {
        public int DistrictID { get; set; }
        public int ProvinceID {  get; set; }
        public string? DistrictName { get; set; }
        public string? Code { get; set; }
    }
}
