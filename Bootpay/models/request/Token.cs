using System;
using System.Text.Json.Serialization;

namespace Bootpay.models
{
    public class Token
    {
        [JsonPropertyName("application_id")]
        public string applicationId { get; set; }

        [JsonPropertyName("private_key")]
        public string privateKey { get; set; }
    }
}
