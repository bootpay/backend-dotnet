 
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class Item
    { 
        public string name { get; set; }
        public int qty { get; set; }
        public string id { get; set; }
        public double price { get; set; }
        public string cat1 { get; set; }
        public string cat2 { get; set; }
        public string cat3 { get; set; }
        

        [JsonProperty("category_type")]
        public string categoryType;

        [JsonProperty("category_code")]
        public string categoryCode;

        [JsonProperty("start_date")]
        public string startDate;

        [JsonProperty("end_date")]
        public string endDate;
    }
}
