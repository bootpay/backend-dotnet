 
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class Subscribe
    {
        public string pg { get; set; }

        [JsonProperty("order_name")]
        public string orderName { get; set; }

        [JsonProperty("subscription_id")]
        public string subscriptionId { get; set; }


        [JsonProperty("card_no")]
        public string cardNo { get; set; }

        [JsonProperty("card_pw")]
        public string cardPw { get; set; }

        [JsonProperty("card_expire_year")]
        public string cardExpireYear { get; set; }

        [JsonProperty("card_expire_month")]
        public string cardExpireMonth { get; set; }

        [JsonProperty("card_identity_no")]
        public string cardIdentityNo { get; set; }

        public double price { get; set; }
        [JsonProperty("tax_free")]
        public double taxFree { get; set; }

        public Dictionary<string, object> metadata { get; set; }


        public User user { get; set; }

        public SubscribeExtra extra { get; set; }

        
        public string username { get; set; }

        [JsonProperty("auth_type")]
        public string authType { get; set; }

        [JsonProperty("bank_name")]
        public string bankName { get; set; }

        [JsonProperty("bank_account")]
        public string bankAccount { get; set; }

        [JsonProperty("identity_no")]
        public string identityNo { get; set; }

        [JsonProperty("cash_receipt_type")]
        public string cashReceiptType { get; set; }

        [JsonProperty("cash_receipt_identity_no")]
        public string cashReceiptIdentityNo { get; set; }

        public string phone { get; set; }
         
    }
}
