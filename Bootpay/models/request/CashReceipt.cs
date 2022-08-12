using System; 
using Newtonsoft.Json;

using System.Collections.Generic;
 
namespace Bootpay.models
{
    public class CashReceipt
    {
        [JsonProperty("receipt_id")]
        public string receiptId { get; set; }


        [JsonProperty("order_id")]
        public string orderId { get; set; }


        [JsonProperty("order_name")]
        public string orderName { get; set; }


        [JsonProperty("identity_no")]
        public string identityNo { get; set; }


        [JsonProperty("purchased_at")]
        public string purchasedAt { get; set; }



        [JsonProperty("cash_receipt_type")]
        public string cashReceiptType { get; set; }


        public double price { get; set; }
        [JsonProperty("tax_free")]
        public double taxFree { get; set; }

        public Dictionary<string, object> metadata { get; set; }
        public Dictionary<string, object> extra { get; set; }

        //구매자 정보
        public string username { get; set; }
        public string email { get; set; }
        public string phone { get; set; }


        public string pg { get; set; }
        public User user { get; set; }

    }
}
