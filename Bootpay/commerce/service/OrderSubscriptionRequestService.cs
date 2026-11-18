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
    ///
    /// ⚠️ 경로가 order-subscription-requests — 하이픈이다.
    ///    order_subscriptions · order_subscription_bills 는 언더스코어라 복사해 고칠 때 가장 흔히 틀리는 지점.
    /// </summary>
    public class OrderSubscriptionRequestService
    {
        /// <summary>
        /// 요청 목록 조회 — project_id 가 있으면 supervisor, 없으면 user scope.
        /// page/limit 미지정시 각각 1 / 20 이 적용된다.
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, OrderSubscriptionRequestListParams listParams = null, string idempotencyKey = null)
        {
            var query = BuildListQuery(listParams);
            return await bootpay.SendAsync($"order-subscription-requests{query}", HttpMethod.Get, null, RequestHeaders(listParams?.ProjectId, idempotencyKey));
        }

        /// <summary>
        /// 요청 단건 조회 — project_id 가 있으면 supervisor, 없으면 user scope.
        /// </summary>
        public static async Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string orderSubscriptionRequestHistoryId, string projectId = null, string idempotencyKey = null)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(projectId)) queryParams["project_id"] = projectId;
            var queryString = queryParams.ToString();
            var query = string.IsNullOrEmpty(queryString) ? "" : $"?{queryString}";

            return await bootpay.SendAsync($"order-subscription-requests/{orderSubscriptionRequestHistoryId}{query}", HttpMethod.Get, null, RequestHeaders(projectId, idempotencyKey));
        }

        /// <summary>
        /// 요청 승인/거절 (supervisor 전용)
        /// ⚠️ 승인과 반려는 별도 액션이 아니다. approval: "approve" | "reject" 파라미터로 갈린다.
        /// </summary>
        public static async Task<HttpResponseMessage> Update(BootpayCommerceObject bootpay, OrderSubscriptionRequestUpdateParams updateParams, string idempotencyKey = null)
        {
            return await bootpay.SendAsync($"order-subscription-requests/{updateParams.OrderSubscriptionRequestHistoryId}", HttpMethod.Put, updateParams, CommerceRequestHeaders.Supervisor(idempotencyKey));
        }

        private static System.Collections.Generic.Dictionary<string, string> RequestHeaders(string projectId, string idempotencyKey)
        {
            return string.IsNullOrEmpty(projectId)
                ? CommerceRequestHeaders.User(idempotencyKey)
                : CommerceRequestHeaders.Supervisor(idempotencyKey);
        }

        private static string BuildListQuery(OrderSubscriptionRequestListParams listParams)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(listParams?.ProjectId)) queryParams["project_id"] = listParams.ProjectId;
            if (!string.IsNullOrEmpty(listParams?.OrderSubscriptionId)) queryParams["order_subscription_id"] = listParams.OrderSubscriptionId;
            queryParams["page"] = (listParams?.Page ?? 1).ToString();
            queryParams["limit"] = (listParams?.Limit ?? 20).ToString();
            if (!string.IsNullOrEmpty(listParams?.Keyword)) queryParams["keyword"] = listParams.Keyword;
            if (!string.IsNullOrEmpty(listParams?.SAt)) queryParams["s_at"] = listParams.SAt;
            if (!string.IsNullOrEmpty(listParams?.EAt)) queryParams["e_at"] = listParams.EAt;
            if (listParams?.Status != null) queryParams["status"] = listParams.Status.ToString();
            if (listParams?.RequestType != null) queryParams["request_type"] = listParams.RequestType.ToString();
            if (!string.IsNullOrEmpty(listParams?.UserId)) queryParams["user_id"] = listParams.UserId;
            if (!string.IsNullOrEmpty(listParams?.UserGroupId)) queryParams["user_group_id"] = listParams.UserGroupId;

            return $"?{queryParams}";
        }
    }
}
