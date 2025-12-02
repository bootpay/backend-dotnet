using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 주문 서비스
    /// </summary>
    public class OrderService
    {
        /// <summary>
        /// 주문 목록 조회
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, OrderListParams listParams = null)
        {
            var query = BuildListQuery(listParams);
            return await bootpay.SendAsync($"orders{query}", HttpMethod.Get);
        }

        /// <summary>
        /// 주문 상세 조회
        /// </summary>
        public static async Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string orderId)
        {
            return await bootpay.SendAsync($"orders/{orderId}", HttpMethod.Get);
        }

        /// <summary>
        /// 월별 주문 조회
        /// </summary>
        public static async Task<HttpResponseMessage> Month(BootpayCommerceObject bootpay, string userGroupId, string searchDate)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            queryParams["user_group_id"] = userGroupId;
            queryParams["search_date"] = searchDate;
            return await bootpay.SendAsync($"orders/month?{queryParams}", HttpMethod.Get);
        }

        private static string BuildListQuery(OrderListParams listParams)
        {
            if (listParams == null) return "";

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (listParams.Page.HasValue) queryParams["page"] = listParams.Page.ToString();
            if (listParams.Limit.HasValue) queryParams["limit"] = listParams.Limit.ToString();
            if (!string.IsNullOrEmpty(listParams.Keyword)) queryParams["keyword"] = listParams.Keyword;
            if (!string.IsNullOrEmpty(listParams.UserId)) queryParams["user_id"] = listParams.UserId;
            if (!string.IsNullOrEmpty(listParams.UserGroupId)) queryParams["user_group_id"] = listParams.UserGroupId;
            if (!string.IsNullOrEmpty(listParams.CsType)) queryParams["cs_type"] = listParams.CsType;
            if (!string.IsNullOrEmpty(listParams.CssAt)) queryParams["css_at"] = listParams.CssAt;
            if (!string.IsNullOrEmpty(listParams.CseAt)) queryParams["cse_at"] = listParams.CseAt;
            if (listParams.SubscriptionBillingType.HasValue)
                queryParams["subscription_billing_type"] = listParams.SubscriptionBillingType.ToString();
            if (listParams.Status != null && listParams.Status.Count > 0)
                queryParams["status"] = string.Join(",", listParams.Status);
            if (listParams.PaymentStatus != null && listParams.PaymentStatus.Count > 0)
                queryParams["payment_status"] = string.Join(",", listParams.PaymentStatus);
            if (listParams.OrderSubscriptionIds != null && listParams.OrderSubscriptionIds.Count > 0)
                queryParams["order_subscription_ids"] = string.Join(",", listParams.OrderSubscriptionIds);

            var query = queryParams.ToString();
            return string.IsNullOrEmpty(query) ? "" : $"?{query}";
        }
    }
}
