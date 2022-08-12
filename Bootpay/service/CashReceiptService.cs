using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.models;
using Newtonsoft.Json;

namespace Bootpay.service
{
    public class CashReceiptService
    { 
        public static async Task<HttpResponseMessage> RequestCashReceiptByBootpay(BootpayObject bootpay, CashReceipt cashReceipt)
        {
            string json = JsonConvert.SerializeObject(cashReceipt,
                        Newtonsoft.Json.Formatting.None,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

            return await bootpay.SendAsync("request/receipt/cash/publish.json", HttpMethod.Post, json);
        }

        public static async Task<HttpResponseMessage> RequestCashReceiptCancelByBootpay(BootpayObject bootpay, Cancel cancel)
        {
            string json = JsonConvert.SerializeObject(cancel,
                        Newtonsoft.Json.Formatting.None,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

            return await bootpay.SendAsync("request/receipt/cash/cancel/" + cancel.receiptId + ".json", HttpMethod.Delete, json);
        }

        public static async Task<HttpResponseMessage> RequestCashReceipt(BootpayObject bootpay, CashReceipt cashReceipt)
        {
            string json = JsonConvert.SerializeObject(cashReceipt,
                        Newtonsoft.Json.Formatting.None,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

            return await bootpay.SendAsync("request/cash/receipt.json", HttpMethod.Post, json);
        }

        public static async Task<HttpResponseMessage> RequestCashReceiptCancel(BootpayObject bootpay, Cancel cancel)
        {
            string json = JsonConvert.SerializeObject(cancel,
                        Newtonsoft.Json.Formatting.None,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });

            return await bootpay.SendAsync("request/cash/receipt/" + cancel.receiptId + ".json", HttpMethod.Delete, json);
        }
    }
}
