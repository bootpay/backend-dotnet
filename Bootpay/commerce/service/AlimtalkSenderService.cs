using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 알림톡 발신프로필(카카오채널) 생명주기 — GET /v1/alimtalk/categories · /senders 계열
    ///
    /// 카테고리 조회 → OTP 발송 → 발신프로필 등록 → 목록/상세 → 연동 해지 순으로 쓴다.
    /// 등록이 끝나면 서버가 그룹키 등록까지 자동으로 하므로, 공식 템플릿은 별도 채택 없이 바로 발송된다.
    ///
    /// ⚠️ 실제 부작용: Otp() 는 채널 관리자 휴대폰으로 문자를 실제 발송하고,
    ///    Create() 는 카카오에 발신프로필을 실제 등록한다. 샌드박스가 없다.
    /// </summary>
    public class AlimtalkSenderService
    {
        /// <summary>
        /// 카카오 카테고리 목록 조회 (GET /v1/alimtalk/categories)
        /// 발신프로필 등록 시 필요한 category_code 후보다. 벤더 응답을 그대로 프록시한다.
        /// </summary>
        public static async Task<HttpResponseMessage> Categories(BootpayCommerceObject bootpay)
        {
            return await bootpay.SendAsync("alimtalk/categories", HttpMethod.Get, null, CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 채널 관리자폰으로 OTP 발송 (POST /v1/alimtalk/senders/otp)
        /// ⚠️ 실제로 문자가 나간다. 여기서 받은 인증번호를 Create() 의 Otp 로 넘긴다.
        /// </summary>
        public static async Task<HttpResponseMessage> Otp(BootpayCommerceObject bootpay, AlimtalkSenderOtpParams otpParams)
        {
            return await bootpay.SendAsync("alimtalk/senders/otp", HttpMethod.Post, otpParams, CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 발신프로필 등록 (POST /v1/alimtalk/senders)
        /// ⚠️ 카카오에 발신프로필이 실제 등록된다. 같은 yellow_id 를 다시 등록하면 기존 프로필을 재사용한다(dedup).
        /// 등록 성공 시 그룹키 등록까지 서버가 수행하므로 공식 카탈로그 전체를 바로 발송할 수 있다.
        /// </summary>
        public static async Task<HttpResponseMessage> Create(BootpayCommerceObject bootpay, AlimtalkSenderCreateParams createParams)
        {
            return await bootpay.SendAsync("alimtalk/senders", HttpMethod.Post, createParams, CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 연동한 채널 목록 조회 (GET /v1/alimtalk/senders)
        /// 자체 DB 만 조회하며 벤더를 호출하지 않는다.
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay)
        {
            return await bootpay.SendAsync("alimtalk/senders", HttpMethod.Get, null, CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 채널 상세 조회 (GET /v1/alimtalk/senders/{ksp_id})
        /// sync 가 true 면 벤더에서 채널 상태를 다시 읽어 반영한다(느리다). 미지정이면 자체 DB 만 본다.
        /// ⚠️ 미연동/미존재 채널은 404, 다른 프로젝트의 채널은 403 으로 오며 둘 다 error_code 는 3024 다.
        /// </summary>
        /// <param name="bootpay">Bootpay Commerce 객체</param>
        /// <param name="kspId">채널 ID</param>
        /// <param name="sync">벤더 동기화 여부 (선택)</param>
        public static async Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string kspId, bool? sync = null)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (sync.HasValue) queryParams["sync"] = sync.Value ? "true" : "false";

            return await bootpay.SendAsync($"alimtalk/senders/{kspId}{AlimtalkQuery.Suffix(queryParams)}", HttpMethod.Get, null, CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 채널 연동 해지 (DELETE /v1/alimtalk/senders/{ksp_id})
        /// 이 프로젝트와의 연동만 끊는다 — 채널 모델과 템플릿은 보존된다. 성공 시 본문은 null 이다.
        /// </summary>
        public static async Task<HttpResponseMessage> Release(BootpayCommerceObject bootpay, string kspId)
        {
            return await bootpay.SendAsync($"alimtalk/senders/{kspId}", HttpMethod.Delete, null, CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 채널 변수 예문 사전 갱신 (PUT /v1/alimtalk/senders/{ksp_id}/variable_examples)
        /// 템플릿 미리보기에서 #{user_name} 대신 "홍길동" 처럼 읽히게 하는 표시용 값이다.
        /// ⚠️ 발송값이 아니다 — 벤더로 전송되지 않으므로 검수 상태와 무관하다. 보낸 키만 덮어쓴다(부분 갱신).
        /// </summary>
        /// <param name="bootpay">Bootpay Commerce 객체</param>
        /// <param name="kspId">채널 ID</param>
        /// <param name="examples">{ "user_name": "홍길동" } — 키에 '.' 이나 선행 '$' 는 쓸 수 없다</param>
        public static async Task<HttpResponseMessage> VariableExamples(BootpayCommerceObject bootpay, string kspId, Dictionary<string, string> examples)
        {
            return await bootpay.SendAsync($"alimtalk/senders/{kspId}/variable_examples", HttpMethod.Put, new { examples }, CommerceRequestHeaders.Alimtalk());
        }
    }
}
