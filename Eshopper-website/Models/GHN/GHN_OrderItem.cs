using Newtonsoft.Json;

namespace Eshopper_website.Models.GHN
{
    public class GHN_OrderItem
    {
        [JsonProperty("name")]
        public required string Name { get; set; }

        [JsonProperty("code")]
        public string? Code { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("weight ")]
        public int Weight { get; set; }

        [JsonProperty("level1")]
        public string? Level1 { get; set; }
    }
}
