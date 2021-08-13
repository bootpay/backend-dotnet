using System;
using System.Text.Json.Serialization;

namespace Bootpay.models.response
{
    public class ResEasy : ResDefault
    {
        public ResCancelData data { get; set; }
    }

    public class ResEasyData
    {
        [JsonPropertyName("user_token")]
        public string userToken { get; set; }

        [JsonPropertyName("expired_unixtime")]
        public long expiredUnixtime { get; set; }

        [JsonPropertyName("expired_localtime")]
        public string expiredLocaltime { get; set; }
    }
}
