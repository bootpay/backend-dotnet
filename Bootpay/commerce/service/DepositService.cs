using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.Commerce.Models;
using Newtonsoft.Json.Linq;

namespace Bootpay.Commerce.Service
{
    /// <summary>뱅크알람 입금대기 — 커머스 /v1/sdk/deposits, supervisor 전용.</summary>
    public class DepositService
    {
        // 기존 시그니처(바이너리 호환) — 이미 컴파일된 소비자가 MissingMethodException 을 내지 않도록 남긴다.
        public static Task<HttpResponseMessage> Accounts(BootpayCommerceObject bootpay) => Accounts(bootpay, null);

        public static Task<HttpResponseMessage> Accounts(BootpayCommerceObject bootpay, string idempotencyKey) =>
            bootpay.SendAsync("sdk/deposits/accounts", HttpMethod.Get, null, CommerceRequestHeaders.Supervisor(idempotencyKey));

        public static Task<HttpResponseMessage> WebhookSecret(BootpayCommerceObject bootpay, string idempotencyKey = null) =>
            bootpay.SendAsync("sdk/deposits/webhook_secret", HttpMethod.Post, new JObject(), CommerceRequestHeaders.Supervisor(idempotencyKey));

        public static Task<HttpResponseMessage> Create(BootpayCommerceObject bootpay, DepositCreateParams createParams) =>
            bootpay.SendAsync("sdk/deposits", HttpMethod.Post, createParams, CommerceRequestHeaders.Supervisor(createParams?.IdempotencyKey));

        public static Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string depositId) => Detail(bootpay, depositId, null);

        public static Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string depositId, string idempotencyKey) =>
            bootpay.SendAsync($"sdk/deposits/{Uri.EscapeDataString(depositId)}", HttpMethod.Get, null, CommerceRequestHeaders.Supervisor(idempotencyKey));

        public static Task<HttpResponseMessage> Cancel(BootpayCommerceObject bootpay, string depositId, string idempotencyKey = null) =>
            bootpay.SendAsync($"sdk/deposits/{Uri.EscapeDataString(depositId)}", HttpMethod.Delete, null, CommerceRequestHeaders.Supervisor(idempotencyKey));
    }
}
