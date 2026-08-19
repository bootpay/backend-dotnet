using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 몰 설정 서비스 (supervisor scope 전용)
    /// </summary>
    public class MallSettingService
    {
        /// <summary>
        /// 몰 설정 조회 (GET /v1/mall-setting)
        /// </summary>
        public static async Task<HttpResponseMessage> GetMallSetting(BootpayCommerceObject bootpay, string idempotencyKey = null)
        {
            return await bootpay.SendAsync("mall-setting", HttpMethod.Get, null, CommerceRequestHeaders.Supervisor(idempotencyKey));
        }

        /// <summary>
        /// 몰 설정 수정 (PUT /v1/mall-setting)
        /// 요청 바디는 flatten 형식이며, null 값은 전송되지 않는다.
        /// </summary>
        public static async Task<HttpResponseMessage> UpdateMallSetting(BootpayCommerceObject bootpay, MallSettingUpdateParams updateParams, string idempotencyKey = null)
        {
            return await bootpay.SendAsync("mall-setting", HttpMethod.Put, updateParams ?? new MallSettingUpdateParams(), CommerceRequestHeaders.Supervisor(idempotencyKey));
        }
    }
}
