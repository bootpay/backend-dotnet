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
        public static async Task<ResBillingKey> GetBillingKey(BootpayObject bootpay, Subscribe subsribe)
        {
             
            string json = JsonConvert.SerializeObject(subsribe,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }); 
            return await bootpay.SendAsync<ResBillingKey>("request/card_rebill.json", HttpMethod.Post, json);
        }

        public static async Task<ResDefault> DestroyBillingKey(BootpayObject bootpay, string billingKey)
        {
            return await bootpay.SendAsync<ResDefault>("subscribe/billing/" + billingKey + ".json", HttpMethod.Delete);
        }

        public static async Task<ResDefault> RequestSubscribe(BootpayObject bootpay, SubscribePayload payload)
        {
            string json = JsonConvert.SerializeObject(payload,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
            return await bootpay.SendAsync<ResDefault>("subscribe/billing.json", HttpMethod.Post, json);
        }

        public static async Task<ResDefault> ReserveSubscribe(BootpayObject bootpay, SubscribePayload payload)
        {
            payload.schedulerType = "oneshot";

            string json = JsonConvert.SerializeObject(payload,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
            return await bootpay.SendAsync<ResDefault>("subscribe/billing/reserve.json", HttpMethod.Post, json);
        }

        public static async Task<ResDefault> ReserveCancelSubscribe(BootpayObject bootpay, string reserveId)
        {
            return await bootpay.SendAsync<ResDefault>("subscribe/billing/reserve/" + reserveId + ".json", HttpMethod.Delete);
        }
    }
}
