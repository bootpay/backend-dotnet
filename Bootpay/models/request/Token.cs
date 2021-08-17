using System;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class Token
    {
        [JsonProperty("application_id")]
        public string applicationId { get; set; }

        [JsonProperty("private_key")]
        public string privateKey { get; set; }
    }
}
