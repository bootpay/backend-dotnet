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
        public static async Task<HttpResponseMessage> GetStore(BootpayCommerceObject bootpay)
        {
            return await bootpay.SendAsync("store", HttpMethod.Get);
        }

        public static async Task<HttpResponseMessage> Info(BootpayCommerceObject bootpay)
        {
            return await GetStore(bootpay);
        }

        /// <summary>
        /// 가맹점 상세 정보 조회
        /// </summary>
        public static async Task<HttpResponseMessage> GetStoreDetail(BootpayCommerceObject bootpay)
        {
            return await bootpay.SendAsync("store/detail", HttpMethod.Get);
        }

        public static async Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay)
        {
            return await GetStoreDetail(bootpay);
        }
    }
}
