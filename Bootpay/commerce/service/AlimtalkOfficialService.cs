using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 부트페이 공식 알림톡 템플릿 카탈로그 — GET/POST /v1/alimtalk/official 계열
    ///
    /// 부트페이가 미리 카카오 승인을 받아 둔 템플릿이라, 그룹키가 등록된 채널이면 검수 없이 즉시 발송된다.
    /// AlimtalkSenderCreate() 로 채널을 등록하면 그룹 등록이 함께 끝나므로 따로 채택할 것이 없다.
    /// (채택 endpoint 는 서버에서 비활성화되어 SDK 에도 두지 않는다)
    ///
    /// 전부 조회 계열이라 부작용이 없다(자체 DB 만 본다).
    /// </summary>
    public class AlimtalkOfficialService
    {
        /// <summary>
        /// 공식 템플릿 검색 (GET /v1/alimtalk/official)
        /// keyword 는 본문·이름·분류를 부분일치(대소문자 무시)로 훑는다.
        /// msg_type 은 BA(기본형)·EX(부가정보형)만 존재한다 — 그룹 템플릿이라 AD/MI 는 쓸 수 없다.
        /// ksp_id 를 주면 그 채널의 변수 예문 사전으로 variable_examples 를 채워 준다(표시용).
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, AlimtalkOfficialListParams listParams = null)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (listParams != null)
            {
                // 서버는 q 를 먼저 보고 없으면 keyword 를 본다 — 정본 키인 q 로 보낸다
                if (!string.IsNullOrEmpty(listParams.Keyword)) queryParams["q"] = listParams.Keyword;
                if (!string.IsNullOrEmpty(listParams.Category)) queryParams["category"] = listParams.Category;
                if (!string.IsNullOrEmpty(listParams.MsgType)) queryParams["msg_type"] = listParams.MsgType;
                if (listParams.Page.HasValue) queryParams["page"] = listParams.Page.ToString();
                if (listParams.Per.HasValue) queryParams["per"] = listParams.Per.ToString();
                if (!string.IsNullOrEmpty(listParams.KspId)) queryParams["ksp_id"] = listParams.KspId;
            }

            return await bootpay.SendAsync($"alimtalk/official{AlimtalkQuery.Suffix(queryParams)}", HttpMethod.Get, null, CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 보내려는 문구로 공식 템플릿 추천받기 (POST /v1/alimtalk/official/recommend)
        /// 유사도 score(0~1) 내림차순으로 돌려준다.
        /// </summary>
        public static async Task<HttpResponseMessage> Recommend(BootpayCommerceObject bootpay, AlimtalkOfficialRecommendParams recommendParams)
        {
            return await bootpay.SendAsync("alimtalk/official/recommend", HttpMethod.Post, recommendParams, CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 공식 템플릿 상세 조회 (GET /v1/alimtalk/official/{code})
        /// code 는 서버 채번 코드(슬래시를 포함하지 않는다). 없거나 미노출이면 404(3015).
        /// </summary>
        /// <param name="bootpay">Bootpay Commerce 객체</param>
        /// <param name="code">공식 템플릿 코드</param>
        /// <param name="kspId">변수 예문을 채워 볼 채널 ID (선택)</param>
        public static async Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string code, string kspId = null)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(kspId)) queryParams["ksp_id"] = kspId;

            return await bootpay.SendAsync($"alimtalk/official/{code}{AlimtalkQuery.Suffix(queryParams)}", HttpMethod.Get, null, CommerceRequestHeaders.Alimtalk());
        }
    }
}
