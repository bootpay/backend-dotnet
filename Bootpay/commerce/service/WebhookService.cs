using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 웹훅 서비스
    /// </summary>
    public class WebhookService
    {
        /// <summary>
        /// 테스트 웹훅 발송 (POST /v1/webhook/test)
        /// 등록된 웹훅 URL 로 테스트 페이로드를 보내 연동을 확인할 때 쓴다.
        /// </summary>
        public static async Task<HttpResponseMessage> SendTest(BootpayCommerceObject bootpay, SendTestWebhookParams sendParams = null, string idempotencyKey = null)
        {
            return await bootpay.SendAsync("webhook/test", HttpMethod.Post, sendParams ?? new SendTestWebhookParams(), CommerceRequestHeaders.Idempotency(idempotencyKey));
        }
    }
}
