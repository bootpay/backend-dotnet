using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 정기구독 조정 서비스
    /// </summary>
    public class OrderSubscriptionAdjustmentService
    {
        /// <summary>
        /// 정기구독 조정 생성
        /// </summary>
        public static async Task<HttpResponseMessage> Create(BootpayCommerceObject bootpay, string orderSubscriptionId, CommerceOrderSubscriptionAdjustment adjustment)
        {
            return await bootpay.SendAsync($"order_subscriptions/{orderSubscriptionId}/adjustments", HttpMethod.Post, adjustment);
        }

        /// <summary>
        /// 정기구독 조정 수정
        /// </summary>
        public static async Task<HttpResponseMessage> Update(BootpayCommerceObject bootpay, OrderSubscriptionAdjustmentUpdateParams updateParams)
        {
            return await bootpay.SendAsync($"order_subscriptions/{updateParams.OrderSubscriptionId}/adjustments", HttpMethod.Put, updateParams);
        }

        /// <summary>
        /// 정기구독 조정 삭제
        /// </summary>
        public static async Task<HttpResponseMessage> Delete(BootpayCommerceObject bootpay, string orderSubscriptionId, string orderSubscriptionAdjustmentId)
        {
            return await bootpay.SendAsync($"order_subscriptions/{orderSubscriptionId}/adjustments?order_subscription_adjustment_id={orderSubscriptionAdjustmentId}", HttpMethod.Delete);
        }
    }
}
