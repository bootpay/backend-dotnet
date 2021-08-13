using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Bootpay.models
{
    public class Extra
    {
        public bool escrow { get; set; }

        [JsonPropertyName("expire_month")]
        public int expireMonth { get; set; }
        public List<int> quota { get; set; }

        [JsonPropertyName("subscribe_test_payment")]
        public bool subscribeTestPayment { get; set; }

        [JsonPropertyName("disp_cash_result")]
        public bool dispCashResult { get; set; }

        [JsonPropertyName("offer_period")]
        public bool offerPeriod { get; set; }

        [JsonPropertyName("sellerName")]
        public string seller_name { get; set; }
        public string theme { get; set; }

        [JsonPropertyName("custom_background")]
        public string customBackground { get; set; }

        [JsonPropertyName("custom_font_color")]
        public string customFontColor { get; set; }
    }    
}
