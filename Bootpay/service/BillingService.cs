using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.models;
using Bootpay.models.response;
using Newtonsoft.Json;

namespace Bootpay.service
{
    public class BillingService
    {
        public static async Task<HttpResponseMessage> GetBillingKey(BootpayObject bootpay, Subscribe subsribe)
        {
             
            string json = JsonConvert.SerializeObject(subsribe,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }); 
            return await bootpay.SendAsync("request/subscribe.json", HttpMethod.Post, json);
        }

        public static async Task<HttpResponseMessage> LookupBillingKey(BootpayObject bootpay, String receiptId)
        {

            //string json = JsonConvert.SerializeObject(subsribe,
            //                Newtonsoft.Json.Formatting.None,
            //                new JsonSerializerSettings
            //                {
            //                    NullValueHandling = NullValueHandling.Ignore
            //                });
            return await bootpay.SendAsync("subscribe/billing_key/" + receiptId, HttpMethod.Get);
        }

        public static async Task<HttpResponseMessage> DestroyBillingKey(BootpayObject bootpay, string billingKey)
        {
            return await bootpay.SendAsync("subscribe/billing_key/" + billingKey + ".json", HttpMethod.Delete);
        }

        public static async Task<HttpResponseMessage> RequestSubscribe(BootpayObject bootpay, SubscribePayload payload)
        {
            string json = JsonConvert.SerializeObject(payload,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
            return await bootpay.SendAsync("subscribe/payment.json", HttpMethod.Post, json);
        }

        public static async Task<HttpResponseMessage> ReserveSubscribe(BootpayObject bootpay, SubscribePayload payload)
        {
            //payload.schedulerType = "oneshot";

            string json = JsonConvert.SerializeObject(payload,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
            return await bootpay.SendAsync("subscribe/payment/reserve.json", HttpMethod.Post, json);
        }

        public static async Task<HttpResponseMessage> ReserveCancelSubscribe(BootpayObject bootpay, string reserveId)
        {
            return await bootpay.SendAsync("subscribe/payment/reserve/" + reserveId + ".json", HttpMethod.Delete);
        }
    }
}
