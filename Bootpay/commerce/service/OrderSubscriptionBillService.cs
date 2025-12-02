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
        /// 정기구독 청구 목록 조회
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, OrderSubscriptionBillListParams listParams = null)
        {
            var query = BuildListQuery(listParams);
            return await bootpay.SendAsync($"order_subscription_bills{query}", HttpMethod.Get);
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
            if (listParams == null) return "";

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (listParams.Page.HasValue) queryParams["page"] = listParams.Page.ToString();
            if (listParams.Limit.HasValue) queryParams["limit"] = listParams.Limit.ToString();
            if (!string.IsNullOrEmpty(listParams.Keyword)) queryParams["keyword"] = listParams.Keyword;
            if (!string.IsNullOrEmpty(listParams.OrderSubscriptionId))
                queryParams["order_subscription_id"] = listParams.OrderSubscriptionId;
            if (listParams.Status != null && listParams.Status.Count > 0)
                queryParams["status"] = string.Join(",", listParams.Status);

            var query = queryParams.ToString();
            return string.IsNullOrEmpty(query) ? "" : $"?{query}";
        }
    }
}
