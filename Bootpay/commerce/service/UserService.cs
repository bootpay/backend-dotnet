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
        /// 회원 로그인 (V1 Mall API — POST /v1/users/login)
        /// ⚠️ 서버(LoginService)는 login_id/password 만 읽는다. corporate_type 은 미지정시 0 으로 전송된다.
        /// </summary>
        public static async Task<HttpResponseMessage> MallLogin(BootpayCommerceObject bootpay, MallUserLoginParams loginParams, string idempotencyKey = null)
        {
            var payload = new MallUserLoginParams
            {
                LoginId = loginParams.LoginId,
                Password = loginParams.Password,
                CorporateType = loginParams.CorporateType ?? 0
            };
            return await bootpay.SendAsync("users/login", HttpMethod.Post, payload, CommerceRequestHeaders.Mall(null, idempotencyKey));
        }

        /// <summary>
        /// 회원 세션 조회 (V1 Mall API — GET /v1/users/session)
        /// 회원 JWT 는 Bootpay-User-JWT 헤더로 전달된다 (값이 있을 때만 부착).
        /// </summary>
        public static async Task<HttpResponseMessage> Session(BootpayCommerceObject bootpay, string userJwt = null, string idempotencyKey = null)
        {
            return await bootpay.SendAsync("users/session", HttpMethod.Get, null, CommerceRequestHeaders.Mall(userJwt, idempotencyKey));
        }

        /// <summary>
        /// 회원 로그아웃 (V1 Mall API — DELETE /v1/users/session)
        /// </summary>
        public static async Task<HttpResponseMessage> Logout(BootpayCommerceObject bootpay, string userJwt, string idempotencyKey = null)
        {
            return await bootpay.SendAsync("users/session", HttpMethod.Delete, null, CommerceRequestHeaders.Mall(userJwt, idempotencyKey));
        }

        /// <summary>
        /// 회원가입 (V1 Mall API — POST /v1/users/join) — 일반 회원가입용
        /// ⚠️ Join(user) 과 같은 엔드포인트를 부르지만 용도가 다르다 —
        ///    이쪽은 password/corporate_type/group 을 쓰는 일반 회원가입, 저쪽은 uid 연동 가입이다.
        ///    corporate_type 미지정시 0, 나머지 null 값은 전송하지 않는다.
        /// </summary>
        public static async Task<HttpResponseMessage> MallJoin(BootpayCommerceObject bootpay, MallUserJoinParams joinParams, string idempotencyKey = null)
        {
            var payload = new MallUserJoinParams
            {
                LoginId = joinParams.LoginId,
                Password = joinParams.Password,
                Name = joinParams.Name,
                Email = joinParams.Email,
                Phone = joinParams.Phone,
                Nickname = joinParams.Nickname,
                Gender = joinParams.Gender,
                Birth = joinParams.Birth,
                CorporateType = joinParams.CorporateType ?? 0,
                Group = joinParams.Group
            };
            return await bootpay.SendAsync("users/join", HttpMethod.Post, payload, CommerceRequestHeaders.Mall(null, idempotencyKey));
        }

        /// <summary>
        /// 회원가입 중복 확인 (V1 Mall API — GET /v1/users/join/{type}?pk={pk})
        /// type: email-exist, id-exist, phone-exist, uid-exist, group-business-number-exist
        /// </summary>
        public static async Task<HttpResponseMessage> JoinCheck(BootpayCommerceObject bootpay, string type, string pk, string idempotencyKey = null)
        {
            var encodedValue = HttpUtility.UrlEncode(pk);
            return await bootpay.SendAsync($"users/join/{type}?pk={encodedValue}", HttpMethod.Get, null, CommerceRequestHeaders.Mall(null, idempotencyKey));
        }

        /// <summary>
        /// 외부 uid(ex_uid) 중복 검사 (GET /v1/users/join/uid-exist?pk={uid}) — user scope
        /// </summary>
        public static async Task<HttpResponseMessage> UidExist(BootpayCommerceObject bootpay, string uid, string idempotencyKey = null)
        {
            var encodedValue = HttpUtility.UrlEncode(uid);
            return await bootpay.SendAsync($"users/join/uid-exist?pk={encodedValue}", HttpMethod.Get, null, CommerceRequestHeaders.User(idempotencyKey));
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
            // 서버가 읽는 이름은 membership_type — MemberType 은 하위호환 별칭으로 같은 키에 실어 보낸다.
#pragma warning disable CS0618
            var membershipType = listParams.MembershipType ?? listParams.MemberType;
#pragma warning restore CS0618
            if (membershipType.HasValue) queryParams["membership_type"] = membershipType.ToString();
            if (!string.IsNullOrEmpty(listParams.Type)) queryParams["type"] = listParams.Type;

            var query = queryParams.ToString();
            return string.IsNullOrEmpty(query) ? "" : $"?{query}";
        }
    }
}
