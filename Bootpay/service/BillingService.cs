using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.models;
using Newtonsoft.Json;

namespace Bootpay.service
{
    public class BillingService
    {
        public static async Task<ResDefault> GetBillingKey(BootpayObject bootpay, Subscribe subsribe)
        {
            return await bootpay.SendAsync<ResDefault>("request/card_rebill", HttpMethod.Post, System.Text.Json.JsonSerializer.Serialize(subsribe));
        }

        public static async Task<ResDefault> DestroyBillingKey(BootpayObject bootpay, string billingKey)
        {
            return await bootpay.SendAsync<ResDefault>("subscribe/billing/" + billingKey, HttpMethod.Delete);
        }

        public static async Task<ResDefault> RequestSubscribe(BootpayObject bootpay, SubscribePayload payload)
        {
            return await bootpay.SendAsync<ResDefault>("subscribe/billing", HttpMethod.Post, System.Text.Json.JsonSerializer.Serialize(payload));
        }

        public static async Task<ResDefault> ReserveSubscribe(BootpayObject bootpay, SubscribePayload payload)
        {
            return await bootpay.SendAsync<ResDefault>("subscribe/billing/reserve", HttpMethod.Post, System.Text.Json.JsonSerializer.Serialize(payload));
        }

        public static async Task<ResDefault> ReserveCancelSubscribe(BootpayObject bootpay, string reserveId)
        {
            return await bootpay.SendAsync<ResDefault>("subscribe/billing/reserve" + reserveId, HttpMethod.Delete);
        }
    }
}
