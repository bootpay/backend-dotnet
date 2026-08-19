using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 사용자 그룹 서비스
    /// </summary>
    public class UserGroupService
    {
        /// <summary>
        /// 사용자 그룹 생성
        /// </summary>
        public static async Task<HttpResponseMessage> Create(BootpayCommerceObject bootpay, CommerceUserGroup userGroup)
        {
            return await bootpay.SendAsync("user-groups", HttpMethod.Post, userGroup);
        }

        /// <summary>
        /// 사용자 그룹 목록 조회
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, UserGroupListParams listParams = null)
        {
            var query = BuildListQuery(listParams);
            return await bootpay.SendAsync($"user-groups{query}", HttpMethod.Get);
        }

        /// <summary>
        /// 사용자 그룹 상세 조회
        /// </summary>
        public static async Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string userGroupId)
        {
            return await bootpay.SendAsync($"user-groups/{userGroupId}", HttpMethod.Get);
        }

        /// <summary>
        /// 사용자 그룹 수정
        /// </summary>
        public static async Task<HttpResponseMessage> Update(BootpayCommerceObject bootpay, CommerceUserGroup userGroup)
        {
            return await bootpay.SendAsync($"user-groups/{userGroup.UserGroupId}", HttpMethod.Put, userGroup);
        }

        /// <summary>
        /// 그룹에 사용자 추가
        /// </summary>
        public static async Task<HttpResponseMessage> UserCreate(BootpayCommerceObject bootpay, string userGroupId, string userId)
        {
            var data = new { user_id = userId };
            return await bootpay.SendAsync($"user-groups/{userGroupId}/user", HttpMethod.Post, data);
        }

        /// <summary>
        /// 그룹에서 사용자 제거
        /// </summary>
        public static async Task<HttpResponseMessage> UserDelete(BootpayCommerceObject bootpay, string userGroupId, string userId)
        {
            return await bootpay.SendAsync($"user-groups/{userGroupId}/user/{userId}", HttpMethod.Delete);
        }

        /// <summary>
        /// 그룹 구매 한도 설정 (PUT /v1/user-groups/{user_group_id}/limit) — manager scope
        /// ⚠️ 한도는 이 전용 라우트로만 바뀐다 (update 로는 반영되지 않는다).
        /// </summary>
        public static async Task<HttpResponseMessage> Limit(BootpayCommerceObject bootpay, UserGroupLimitParams limitParams, string idempotencyKey = null)
        {
            return await bootpay.SendAsync($"user-groups/{limitParams.UserGroupId}/limit", HttpMethod.Put, limitParams, CommerceRequestHeaders.Manager(idempotencyKey));
        }

        /// <summary>
        /// 그룹 구독 합산청구(정산주기) 설정 (PUT /v1/user-groups/{user_group_id}/aggregate-transaction) — manager scope
        /// </summary>
        public static async Task<HttpResponseMessage> AggregateTransaction(BootpayCommerceObject bootpay, UserGroupAggregateTransactionParams aggregateParams, string idempotencyKey = null)
        {
            return await bootpay.SendAsync($"user-groups/{aggregateParams.UserGroupId}/aggregate-transaction", HttpMethod.Put, aggregateParams, CommerceRequestHeaders.Manager(idempotencyKey));
        }

        private static string BuildListQuery(UserGroupListParams listParams)
        {
            if (listParams == null) return "";

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (listParams.Page.HasValue) queryParams["page"] = listParams.Page.ToString();
            if (listParams.Limit.HasValue) queryParams["limit"] = listParams.Limit.ToString();
            if (!string.IsNullOrEmpty(listParams.Keyword)) queryParams["keyword"] = listParams.Keyword;
            if (listParams.CorporateType.HasValue) queryParams["corporate_type"] = listParams.CorporateType.ToString();

            var query = queryParams.ToString();
            return string.IsNullOrEmpty(query) ? "" : $"?{query}";
        }
    }
}
