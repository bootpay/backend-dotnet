using Newtonsoft.Json;

namespace Bootpay.Commerce.Models
{
    /// <summary>
    /// 테스트 웹훅 발송 파라미터 (POST /v1/webhook/test)
    /// </summary>
    public class SendTestWebhookParams
    {
        /// <summary>
        /// 웹훅 본문 Content-Type (미지정시 서버 기본값)
        /// </summary>
        [JsonProperty("header_content_type")]
        public int? HeaderContentType { get; set; }
    }
}
