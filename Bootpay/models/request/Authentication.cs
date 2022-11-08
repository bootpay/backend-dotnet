using System;
using Newtonsoft.Json;

using System.Collections.Generic;

namespace Bootpay.models
{
    public class AuthenticationParams
    { 

        [JsonProperty("receipt_id")]
        public string receiptId { get; set; }

        public string otp { get; set; } 
    }
}

 