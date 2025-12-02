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
            return await bootpay.SendAsync($"user-groups/{userGroupId}/add_user", HttpMethod.Post, data);
        }

        /// <summary>
        /// 그룹에서 사용자 제거
        /// </summary>
        public static async Task<HttpResponseMessage> UserDelete(BootpayCommerceObject bootpay, string userGroupId, string userId)
        {
            return await bootpay.SendAsync($"user-groups/{userGroupId}/remove_user?user_id={userId}", HttpMethod.Delete);
        }

        /// <summary>
        /// 그룹 제한 설정
        /// </summary>
        public static async Task<HttpResponseMessage> Limit(BootpayCommerceObject bootpay, UserGroupLimitParams limitParams)
        {
            return await bootpay.SendAsync($"user-groups/{limitParams.UserGroupId}/limit", HttpMethod.Put, limitParams);
        }

        /// <summary>
        /// 그룹 거래 집계 설정
        /// </summary>
        public static async Task<HttpResponseMessage> AggregateTransaction(BootpayCommerceObject bootpay, UserGroupAggregateTransactionParams aggregateParams)
        {
            return await bootpay.SendAsync($"user-groups/{aggregateParams.UserGroupId}/aggregate-transaction", HttpMethod.Put, aggregateParams);
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
