using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// V1 OrderSubscription Request 조회/승인 서비스
    ///
    /// 본인 모드 (user role): project_id 없이 호출
    /// 슈퍼바이저 모드 (supervisor role): project_id 포함 + update (승인/거절)
    /// </summary>
    public class OrderSubscriptionRequestService
    {
        /// <summary>
        /// 요청 목록 조회
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, OrderSubscriptionRequestListParams listParams = null)
        {
            var query = BuildListQuery(listParams);
            return await bootpay.SendAsync($"order-subscription-requests{query}", HttpMethod.Get);
        }

        /// <summary>
        /// 요청 단건 조회
        /// </summary>
        public static async Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string orderSubscriptionRequestHistoryId, string projectId = null)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(projectId)) queryParams["project_id"] = projectId;
            var queryString = queryParams.ToString();
            var query = string.IsNullOrEmpty(queryString) ? "" : $"?{queryString}";

            return await bootpay.SendAsync($"order-subscription-requests/{orderSubscriptionRequestHistoryId}{query}", HttpMethod.Get);
        }

        /// <summary>
        /// 요청 승인/거절 (supervisor 전용)
        /// </summary>
        public static async Task<HttpResponseMessage> Update(BootpayCommerceObject bootpay, OrderSubscriptionRequestUpdateParams updateParams)
        {
            return await bootpay.SendAsync($"order-subscription-requests/{updateParams.OrderSubscriptionRequestHistoryId}", HttpMethod.Put, updateParams);
        }

        private static string BuildListQuery(OrderSubscriptionRequestListParams listParams)
        {
            if (listParams == null) return "";

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(listParams.ProjectId)) queryParams["project_id"] = listParams.ProjectId;
            if (listParams.Page.HasValue) queryParams["page"] = listParams.Page.ToString();
            if (listParams.Limit.HasValue) queryParams["limit"] = listParams.Limit.ToString();
            if (listParams.RequestType.HasValue) queryParams["request_type"] = listParams.RequestType.ToString();
            if (listParams.Status.HasValue) queryParams["status"] = listParams.Status.ToString();
            if (!string.IsNullOrEmpty(listParams.SAt)) queryParams["s_at"] = listParams.SAt;
            if (!string.IsNullOrEmpty(listParams.EAt)) queryParams["e_at"] = listParams.EAt;
            if (!string.IsNullOrEmpty(listParams.Keyword)) queryParams["keyword"] = listParams.Keyword;

            var query = queryParams.ToString();
            return string.IsNullOrEmpty(query) ? "" : $"?{query}";
        }
    }
}
