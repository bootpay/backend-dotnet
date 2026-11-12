using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>입점사 지급 요청 — 커머스 /v1/sdk/payouts, supervisor 전용.</summary>
    public class PayoutService
    {
        public static Task<HttpResponseMessage> Create(BootpayCommerceObject bootpay, PayoutCreateParams createParams) =>
            bootpay.SendAsync("sdk/payouts", HttpMethod.Post, createParams, CommerceRequestHeaders.Supervisor(createParams?.IdempotencyKey));

        public static Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string payoutId, string idempotencyKey = null) =>
            bootpay.SendAsync($"sdk/payouts/{Uri.EscapeDataString(payoutId)}", HttpMethod.Get, null, CommerceRequestHeaders.Supervisor(idempotencyKey));

        public static Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, PayoutListParams listParams = null) =>
            bootpay.SendAsync($"sdk/payouts{BuildListQuery(listParams)}", HttpMethod.Get, null, CommerceRequestHeaders.Supervisor(listParams?.IdempotencyKey));

        private static string BuildListQuery(PayoutListParams listParams)
        {
            if (listParams == null) return "";

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(listParams.Status)) queryParams["status"] = listParams.Status;
            if (!string.IsNullOrEmpty(listParams.SubSellerId)) queryParams["sub_seller_id"] = listParams.SubSellerId;
            if (!string.IsNullOrEmpty(listParams.From)) queryParams["from"] = listParams.From;
            if (!string.IsNullOrEmpty(listParams.To)) queryParams["to"] = listParams.To;
            if (listParams.Page.HasValue) queryParams["page"] = listParams.Page.ToString();
            if (listParams.Limit.HasValue) queryParams["limit"] = listParams.Limit.ToString();

            var query = queryParams.ToString();
            return string.IsNullOrEmpty(query) ? "" : $"?{query}";
        }
    }
}
