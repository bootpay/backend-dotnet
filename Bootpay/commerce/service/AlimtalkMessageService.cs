using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 알림톡 발송내역·집계 — GET /v1/alimtalk/messages 계열
    ///
    /// 유료 알림톡만 조회된다(무료 커머스 알림톡은 포함되지 않는다).
    /// 상태는 벤더 결과 동기화로 확정되므로 접수 직후에는 requested 로 보인다.
    /// </summary>
    public class AlimtalkMessageService
    {
        /// <summary>
        /// 발송내역 목록 조회 (GET /v1/alimtalk/messages)
        /// ⚠️ 기간 기본값은 최근 30일이고 최대 조회 폭은 92일이다 — 초과분은 거부하지 않고 시작일을 당겨 잘라낸다.
        ///    실제 적용된 구간은 응답의 period 로 확인한다.
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, AlimtalkMessageListParams listParams = null)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (listParams != null)
            {
                if (!string.IsNullOrEmpty(listParams.TemplateCode)) queryParams["template_code"] = listParams.TemplateCode;
                if (!string.IsNullOrEmpty(listParams.Status)) queryParams["status"] = listParams.Status;
                if (!string.IsNullOrEmpty(listParams.RefId)) queryParams["ref_id"] = listParams.RefId;
                if (!string.IsNullOrEmpty(listParams.To)) queryParams["to"] = listParams.To;
                if (!string.IsNullOrEmpty(listParams.SAt)) queryParams["s_at"] = listParams.SAt;
                if (!string.IsNullOrEmpty(listParams.EAt)) queryParams["e_at"] = listParams.EAt;
                if (listParams.Page.HasValue) queryParams["page"] = listParams.Page.ToString();
                if (listParams.Limit.HasValue) queryParams["limit"] = listParams.Limit.ToString();
            }

            return await bootpay.SendAsync($"alimtalk/messages{AlimtalkQuery.Suffix(queryParams)}", HttpMethod.Get, null, CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 기간 집계 조회 (GET /v1/alimtalk/messages/stats)
        /// 일자별 집계 원장에서 읽으므로 응답이 빠르다.
        /// ⚠️ billing.unit_price_source 가 'default' 면 잠정 단가다(확정 청구액이 아니다).
        /// ⚠️ billing.billable_count 는 성공 − 폴백이다 — 폴백분은 LMS 단가로 따로 계산된다.
        /// </summary>
        public static async Task<HttpResponseMessage> Stats(BootpayCommerceObject bootpay, AlimtalkMessageStatsParams statsParams = null)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (statsParams != null)
            {
                if (!string.IsNullOrEmpty(statsParams.SAt)) queryParams["s_at"] = statsParams.SAt;
                if (!string.IsNullOrEmpty(statsParams.EAt)) queryParams["e_at"] = statsParams.EAt;
            }

            return await bootpay.SendAsync($"alimtalk/messages/stats{AlimtalkQuery.Suffix(queryParams)}", HttpMethod.Get, null, CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 단건 발송 결과 조회 (GET /v1/alimtalk/messages/{receipt_id})
        /// 실패 사유는 error_code·error_message 에 담긴다.
        /// fallback_type 은 폴백이 꺼진 건이면 null, 켜진 건이면 LMS 다.
        /// 다른 프로젝트의 건이거나 없으면 404(3025).
        /// </summary>
        public static async Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string receiptId)
        {
            return await bootpay.SendAsync($"alimtalk/messages/{receiptId}", HttpMethod.Get, null, CommerceRequestHeaders.Alimtalk());
        }
    }
}
