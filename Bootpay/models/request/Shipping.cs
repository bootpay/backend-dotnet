 
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class Shipping
    {
        [JsonProperty("receipt_id")]
        public string receiptId { get; set; }
        [JsonProperty("redirect_url")]
        public string redirectUrl { get; set; }
        [JsonProperty("delivery_corp")]
        public string deliveryCorp { get; set; }
        [JsonProperty("tracking_number")]
        public string trackingNumber { get; set; }
        [JsonProperty("shipping_prepayment")]
        public bool shippingPrepayment;
        [JsonProperty("shipping_day")]
        public bool shippingDay;

        public ShippingUser user;
        public ShippingCompany company;
    }
}
