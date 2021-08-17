using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class Extra
    {
        public bool escrow { get; set; }

        [JsonProperty("expire_month")]
        public int expireMonth { get; set; }
        public List<int> quota { get; set; }

        [JsonProperty("subscribe_test_payment")]
        public bool subscribeTestPayment { get; set; }

        [JsonProperty("disp_cash_result")]
        public bool dispCashResult { get; set; }

        [JsonProperty("offer_period")]
        public bool offerPeriod { get; set; }

        [JsonProperty("sellerName")]
        public string seller_name { get; set; }
        public string theme { get; set; }

        [JsonProperty("custom_background")]
        public string customBackground { get; set; }

        [JsonProperty("custom_font_color")]
        public string customFontColor { get; set; }
    }    
}
