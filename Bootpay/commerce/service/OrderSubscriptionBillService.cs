using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 정기구독 청구 서비스
    /// </summary>
    public class OrderSubscriptionBillService
    {
        /// <summary>
        /// 정기구독 빌(회차) 목록 조회 (GET /v1/order_subscription_bills) — user scope
        /// ⚠️ 경로가 order_subscription_bills — 언더스코어다 (하이픈 아님).
        /// page/limit 미지정시 각각 1 / 20 이 적용된다.
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, OrderSubscriptionBillListParams listParams = null, string idempotencyKey = null)
        {
            var query = BuildListQuery(listParams);
            return await bootpay.SendAsync($"order_subscription_bills{query}", HttpMethod.Get, null, CommerceRequestHeaders.User(idempotencyKey));
        }

        /// <summary>
        /// 정기구독 청구 상세 조회
        /// </summary>
        public static async Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string orderSubscriptionBillId)
        {
            return await bootpay.SendAsync($"order_subscription_bills/{orderSubscriptionBillId}", HttpMethod.Get);
        }

        /// <summary>
        /// 정기구독 청구 수정
        /// </summary>
        public static async Task<HttpResponseMessage> Update(BootpayCommerceObject bootpay, CommerceOrderSubscriptionBill orderSubscriptionBill)
        {
            return await bootpay.SendAsync($"order_subscription_bills/{orderSubscriptionBill.OrderSubscriptionBillId}", HttpMethod.Put, orderSubscriptionBill);
        }

        private static string BuildListQuery(OrderSubscriptionBillListParams listParams)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(listParams?.OrderSubscriptionId))
                queryParams["order_subscription_id"] = listParams.OrderSubscriptionId;
            queryParams["page"] = (listParams?.Page ?? 1).ToString();
            queryParams["limit"] = (listParams?.Limit ?? 20).ToString();
            if (!string.IsNullOrEmpty(listParams?.Keyword)) queryParams["keyword"] = listParams.Keyword;
            if (listParams?.Status != null && listParams.Status.Count > 0)
                queryParams["status"] = string.Join(",", listParams.Status);

            return $"?{queryParams}";
        }
    }
}
