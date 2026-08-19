using System.Net.Http;
using System.Threading.Tasks;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 스토어 서비스
    /// </summary>
    public class StoreService
    {
        /// <summary>
        /// 가맹점 기본 정보 조회
        /// </summary>
        public static async Task<HttpResponseMessage> GetStore(BootpayCommerceObject bootpay, string idempotencyKey = null)
        {
            return await bootpay.SendAsync("store", HttpMethod.Get, null, CommerceRequestHeaders.Idempotency(idempotencyKey));
        }

        public static async Task<HttpResponseMessage> Info(BootpayCommerceObject bootpay, string idempotencyKey = null)
        {
            return await GetStore(bootpay, idempotencyKey);
        }

        /// <summary>
        /// 가맹점 상세 정보 조회
        /// </summary>
        public static async Task<HttpResponseMessage> GetStoreDetail(BootpayCommerceObject bootpay, string idempotencyKey = null)
        {
            return await bootpay.SendAsync("store/detail", HttpMethod.Get, null, CommerceRequestHeaders.Idempotency(idempotencyKey));
        }

        public static async Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string idempotencyKey = null)
        {
            return await GetStoreDetail(bootpay, idempotencyKey);
        }
    }
}
