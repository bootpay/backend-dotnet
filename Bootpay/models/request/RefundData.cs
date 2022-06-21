using System;
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class RefundData
    {

        [JsonProperty("bank_account")]
        public string bankAccount { get; set; }
        [JsonProperty("bank_username")]
        public string bankUsername { get; set; } 
        public string bankcode { get; set; }
    }
}
