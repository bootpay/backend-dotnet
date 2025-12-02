using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 정기구독 서비스
    /// </summary>
    public class OrderSubscriptionService
    {
        /// <summary>
        /// 정기구독 목록 조회
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, OrderSubscriptionListParams listParams = null)
        {
            var query = BuildListQuery(listParams);
            return await bootpay.SendAsync($"order_subscriptions{query}", HttpMethod.Get);
        }

        /// <summary>
        /// 정기구독 상세 조회
        /// </summary>
        public static async Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string orderSubscriptionId)
        {
            return await bootpay.SendAsync($"order_subscriptions/{orderSubscriptionId}", HttpMethod.Get);
        }

        /// <summary>
        /// 정기구독 수정
        /// </summary>
        public static async Task<HttpResponseMessage> Update(BootpayCommerceObject bootpay, OrderSubscriptionUpdateParams updateParams)
        {
            return await bootpay.SendAsync($"order_subscriptions/{updateParams.OrderSubscriptionId}", HttpMethod.Put, updateParams);
        }

        /// <summary>
        /// 정기구독 일시정지
        /// </summary>
        public static async Task<HttpResponseMessage> Pause(BootpayCommerceObject bootpay, OrderSubscriptionPauseParams pauseParams)
        {
            return await bootpay.SendAsync("order_subscriptions/requests/ing/pause", HttpMethod.Post, pauseParams);
        }

        /// <summary>
        /// 정기구독 재개
        /// </summary>
        public static async Task<HttpResponseMessage> Resume(BootpayCommerceObject bootpay, OrderSubscriptionResumeParams resumeParams)
        {
            return await bootpay.SendAsync("order_subscriptions/requests/ing/resume", HttpMethod.Put, resumeParams);
        }

        /// <summary>
        /// 해지 수수료 계산
        /// </summary>
        public static async Task<HttpResponseMessage> CalculateTerminationFee(BootpayCommerceObject bootpay, string orderSubscriptionId = null, string orderNumber = null)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(orderSubscriptionId))
                queryParams["order_subscription_id"] = orderSubscriptionId;
            if (!string.IsNullOrEmpty(orderNumber))
                queryParams["order_number"] = orderNumber;

            return await bootpay.SendAsync($"order_subscriptions/requests/ing/calculate_termination_fee?{queryParams}", HttpMethod.Get);
        }

        /// <summary>
        /// 주문번호로 해지 수수료 계산
        /// </summary>
        public static async Task<HttpResponseMessage> CalculateTerminationFeeByOrderNumber(BootpayCommerceObject bootpay, string orderNumber)
        {
            return await CalculateTerminationFee(bootpay, null, orderNumber);
        }

        /// <summary>
        /// 정기구독 해지
        /// </summary>
        public static async Task<HttpResponseMessage> Termination(BootpayCommerceObject bootpay, OrderSubscriptionTerminationParams terminationParams)
        {
            return await bootpay.SendAsync("order_subscriptions/requests/ing/termination", HttpMethod.Post, terminationParams);
        }

        private static string BuildListQuery(OrderSubscriptionListParams listParams)
        {
            if (listParams == null) return "";

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (listParams.Page.HasValue) queryParams["page"] = listParams.Page.ToString();
            if (listParams.Limit.HasValue) queryParams["limit"] = listParams.Limit.ToString();
            if (!string.IsNullOrEmpty(listParams.Keyword)) queryParams["keyword"] = listParams.Keyword;
            if (!string.IsNullOrEmpty(listParams.SAt)) queryParams["s_at"] = listParams.SAt;
            if (!string.IsNullOrEmpty(listParams.EAt)) queryParams["e_at"] = listParams.EAt;
            if (!string.IsNullOrEmpty(listParams.RequestType)) queryParams["request_type"] = listParams.RequestType;
            if (!string.IsNullOrEmpty(listParams.UserGroupId)) queryParams["user_group_id"] = listParams.UserGroupId;
            if (!string.IsNullOrEmpty(listParams.UserId)) queryParams["user_id"] = listParams.UserId;

            var query = queryParams.ToString();
            return string.IsNullOrEmpty(query) ? "" : $"?{query}";
        }
    }
}
