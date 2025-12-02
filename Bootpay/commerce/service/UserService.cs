using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 사용자 서비스
    /// </summary>
    public class UserService
    {
        /// <summary>
        /// 사용자 토큰 발급
        /// </summary>
        public static async Task<HttpResponseMessage> Token(BootpayCommerceObject bootpay, string userId)
        {
            var data = new { user_id = userId };
            return await bootpay.SendAsync("users/login/token", HttpMethod.Post, data);
        }

        /// <summary>
        /// 회원가입
        /// </summary>
        public static async Task<HttpResponseMessage> Join(BootpayCommerceObject bootpay, CommerceUser user)
        {
            return await bootpay.SendAsync("users/join", HttpMethod.Post, user);
        }

        /// <summary>
        /// 중복 체크
        /// </summary>
        public static async Task<HttpResponseMessage> CheckExist(BootpayCommerceObject bootpay, string key, string value)
        {
            var encodedValue = HttpUtility.UrlEncode(value);
            return await bootpay.SendAsync($"users/join/{key}?pk={encodedValue}", HttpMethod.Get);
        }

        /// <summary>
        /// 본인인증 데이터 조회
        /// </summary>
        public static async Task<HttpResponseMessage> AuthenticationData(BootpayCommerceObject bootpay, string standId)
        {
            return await bootpay.SendAsync($"users/authenticate/{standId}", HttpMethod.Get);
        }

        /// <summary>
        /// 로그인
        /// </summary>
        public static async Task<HttpResponseMessage> Login(BootpayCommerceObject bootpay, string loginId, string loginPw)
        {
            var data = new { login_id = loginId, login_pw = loginPw };
            return await bootpay.SendAsync("users/login", HttpMethod.Post, data);
        }

        /// <summary>
        /// 사용자 목록 조회
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, UserListParams listParams = null)
        {
            var query = BuildListQuery(listParams);
            return await bootpay.SendAsync($"users{query}", HttpMethod.Get);
        }

        /// <summary>
        /// 사용자 상세 조회
        /// </summary>
        public static async Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string userId)
        {
            return await bootpay.SendAsync($"users/{userId}", HttpMethod.Get);
        }

        /// <summary>
        /// 사용자 정보 수정
        /// </summary>
        public static async Task<HttpResponseMessage> Update(BootpayCommerceObject bootpay, CommerceUser user)
        {
            return await bootpay.SendAsync($"users/{user.UserId}", HttpMethod.Put, user);
        }

        /// <summary>
        /// 사용자 삭제 (회원탈퇴)
        /// </summary>
        public static async Task<HttpResponseMessage> Delete(BootpayCommerceObject bootpay, string userId)
        {
            return await bootpay.SendAsync($"users/{userId}", HttpMethod.Delete);
        }

        private static string BuildListQuery(UserListParams listParams)
        {
            if (listParams == null) return "";

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (listParams.Page.HasValue) queryParams["page"] = listParams.Page.ToString();
            if (listParams.Limit.HasValue) queryParams["limit"] = listParams.Limit.ToString();
            if (!string.IsNullOrEmpty(listParams.Keyword)) queryParams["keyword"] = listParams.Keyword;
            if (listParams.MemberType.HasValue) queryParams["member_type"] = listParams.MemberType.ToString();
            if (!string.IsNullOrEmpty(listParams.Type)) queryParams["type"] = listParams.Type;

            var query = queryParams.ToString();
            return string.IsNullOrEmpty(query) ? "" : $"?{query}";
        }
    }
}
