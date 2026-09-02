using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 알림톡 발송 — POST /v1/alimtalk/send · /send/bulk · DELETE /send/{receipt_id}
    ///
    /// ⚠️ 실제로 카카오톡이 발송되고 과금된다. 샌드박스가 없다.
    ///
    /// 처리 순서: 멱등 확인 → 템플릿·채널 해석 → 발송권한 → 지갑 자격 → 발송제어 → 폴백 확정(발신번호 확보)
    ///   → 수신거부 대조 → 변수 치환·규격검증 → 접수(READY) → 워커 전송
    ///
    /// - 멱등: 같은 (프로젝트, ref_id) 로 재요청하면 기존 receipt 를 그대로 돌려준다. 실패한 건만 재발송된다.
    /// - 필수 변수: 템플릿 응답의 required_variables 를 모두 채워야 한다. 하나라도 비면 3017 로 거부된다.
    /// - 채널: sender_key(공개키)로 지정한다. 생략하면 프로젝트 연동 채널로 해석하며,
    ///   연동 채널이 둘 이상일 때만 필수다 (ksp_id 는 내부 문서 id 라 발송 API 에 쓰지 않는다).
    /// </summary>
    public class AlimtalkSendService
    {
        /// <summary>
        /// 단건 발송 (POST /v1/alimtalk/send)
        /// ⚠️ fallback 은 미지정(null)과 false 가 다르다 — 미지정이면 프로젝트 기본값을 따르고,
        ///    false 는 명시적으로 끈다. null 만 걷어내므로 false 는 그대로 전달된다.
        /// </summary>
        public static async Task<HttpResponseMessage> Send(BootpayCommerceObject bootpay, AlimtalkSendParams sendParams)
        {
            return await bootpay.SendAsync("alimtalk/send", HttpMethod.Post, sendParams, CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 벌크 발송 (POST /v1/alimtalk/send/bulk) — 1요청 = N수신자
        /// ⚠️ 수신자 수만큼 실제 발송되고 과금된다.
        /// - 쿼터를 넘으면 요청 시점에 전체 거부된다(3022) — 일부만 나가지 않는다.
        /// - 개별 수신자의 실패는 건별 rejected 로 표시되고 나머지는 정상 발송된다.
        /// - 수신거부 번호는 skipped 이며 과금되지 않고 발송 기록도 만들지 않는다.
        /// - fallback 은 요청 단위로 한 번만 판정한다 — 발신번호가 없으면 요청 전체가 3030 으로 거부된다.
        /// </summary>
        public static async Task<HttpResponseMessage> Bulk(BootpayCommerceObject bootpay, AlimtalkSendBulkParams bulkParams)
        {
            return await bootpay.SendAsync("alimtalk/send/bulk", HttpMethod.Post, bulkParams, CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 예약 발송 취소 (DELETE /v1/alimtalk/send/{receipt_id})
        /// 접수(READY) 상태의 예약 건만 취소할 수 있다 — 이미 전송에 들어갔으면 3023 이다.
        /// </summary>
        public static async Task<HttpResponseMessage> Cancel(BootpayCommerceObject bootpay, string receiptId)
        {
            return await bootpay.SendAsync($"alimtalk/send/{receiptId}", HttpMethod.Delete, null, CommerceRequestHeaders.Alimtalk());
        }
    }
}
