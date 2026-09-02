using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 알림톡 수신거부 — /v1/alimtalk/optouts 계열 (가맹점 CRM 수신거부 동기화용)
    ///
    /// 발송 판정과 같은 기준으로 다룬다 — 부트페이 전역(global) + 내 프로젝트.
    /// ⚠️ 전역 건은 조회는 되지만 해제할 수 없다(releasable: false).
    ///    이걸 노출하지 않으면 "화면엔 수신거부가 아닌데 발송은 3021 로 막히는" 상태가 된다.
    /// </summary>
    public class AlimtalkOptoutService
    {
        /// <summary>
        /// 수신거부 목록 조회 (GET /v1/alimtalk/optouts)
        /// phone 은 숫자만 남겨 부분일치로 찾는다(정확 매칭이 아니다). 50건 단위로 페이징된다.
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, AlimtalkOptoutListParams listParams = null)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (listParams != null)
            {
                if (!string.IsNullOrEmpty(listParams.Phone)) queryParams["phone"] = listParams.Phone;
                if (listParams.Page.HasValue) queryParams["page"] = listParams.Page.ToString();
            }

            return await bootpay.SendAsync($"alimtalk/optouts{AlimtalkQuery.Suffix(queryParams)}", HttpMethod.Get, null, CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 수신거부 등록 (POST /v1/alimtalk/optouts)
        /// 내 프로젝트 스코프로 등록된다(source: api). 같은 번호를 다시 등록해도 멱등이다.
        /// </summary>
        public static async Task<HttpResponseMessage> Create(BootpayCommerceObject bootpay, AlimtalkOptoutCreateParams createParams)
        {
            return await bootpay.SendAsync("alimtalk/optouts", HttpMethod.Post, createParams, CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 발송 전 수신거부 사전 확인 (POST /v1/alimtalk/optouts/check)
        /// 발송 판정과 같은 축으로 대조하므로, 벌크에서 skipped 로 낭비될 건을 미리 뺄 수 있다.
        /// 단건(Phone)·다건(Phones) 모두 받는다.
        /// ⚠️ 1회 최대 1,000건이고 넘으면 -48 이다(중복은 서버가 제거).
        /// </summary>
        public static async Task<HttpResponseMessage> Check(BootpayCommerceObject bootpay, AlimtalkOptoutCheckParams checkParams)
        {
            return await bootpay.SendAsync("alimtalk/optouts/check", HttpMethod.Post, checkParams, CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 수신거부 해제 (DELETE /v1/alimtalk/optouts/{phone})
        /// 내 프로젝트 스코프 건만 해제되며 멱등이다(없어도 성공).
        /// ⚠️ 전역 차단은 해제되지 않고 global_blocked: true 로 알려 준다 —
        ///    "지웠는데 여전히 막히는" 상태를 응답으로 드러내기 위함이다.
        /// </summary>
        public static async Task<HttpResponseMessage> Release(BootpayCommerceObject bootpay, string phone)
        {
            return await bootpay.SendAsync($"alimtalk/optouts/{phone}", HttpMethod.Delete, null, CommerceRequestHeaders.Alimtalk());
        }
    }
}
