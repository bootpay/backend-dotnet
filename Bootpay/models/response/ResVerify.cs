using System; 
using Newtonsoft.Json;

namespace Bootpay.models.response
{
    public class ResEasy : ResDefault
    {
        public new ResCancelData data { get; set; }
    }

    public class ResEasyData
    {
        [JsonProperty("user_token")]
        public string userToken { get; set; }

        [JsonProperty("expired_unixtime")]
        public long expiredUnixtime { get; set; }

        [JsonProperty("expired_localtime")]
        public string expiredLocaltime { get; set; }
    }
}
