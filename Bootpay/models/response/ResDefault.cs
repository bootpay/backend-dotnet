using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class ResDefault
    {
        public int status { get; set; }
        public int code { get; set; }
        public string message { get; set; }

        [JsonProperty("data", NullValueHandling = NullValueHandling.Ignore)]
        public Dictionary<string, object> data { get; set; }
    }
}
