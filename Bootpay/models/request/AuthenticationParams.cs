using System;
using Newtonsoft.Json;

using System.Collections.Generic;

namespace Bootpay.models
{
    public class Authentication
    {
        public string pg { get; set; }
        public string method { get; set; }
        public string username { get; set; }

        [JsonProperty("identity_no")]
        public string identityNo { get; set; }

        public string carrier { get; set; }
        public string phone { get; set; }


        [JsonProperty("site_url")]
        public string siteUrl { get; set; }


        [JsonProperty("order_name")]
        public string orderName { get; set; }

        [JsonProperty("authentication_id")]
        public string authenticationId { get; set; }

        [JsonProperty("client_ip")]
        public string clientIp { get; set; }

        [JsonProperty("authenticate_type")]
        public string authenticateType { get; set; }

        public Extra extra { get; set; }
        public User user { get; set; }
        public Dictionary<string, object> metadata { get; set; }
    }
}

 