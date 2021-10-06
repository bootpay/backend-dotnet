using System;
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class UserToken
    {
        [JsonProperty("user_id")]
        public string userId { get; set; }        
        public string email { get; set; }
        public string name { get; set; }
        public int gender { get; set; }
        public string birth { get; set; }
        public string phone { get; set; }
    }
}
