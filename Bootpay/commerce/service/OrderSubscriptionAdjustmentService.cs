using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 정기구독 조정 서비스
    ///
    /// ⚠️ /adjustments 한 경로에 POST · PUT · DELETE 세 동사가 걸려 있다.
    ///    경로만 보고 메서드를 유추하지 말 것. 서버가 supervisor scope 를 요구한다.
    /// </summary>
    public class OrderSubscriptionAdjustmentService
    {
        /// <summary>
        /// 가감산 조정항목 추가 (POST /v1/order_subscriptions/{order_subscription_id}/adjustments)
        /// type 미전달시 서버가 price &gt; 0 이면 SETUP_PRICE, 아니면 PERIOD_DISCOUNT 로 자동 판정한다.
        /// price/duration/tax_free_price 미지정시 각각 0 / 1 / 0 이 전송된다.
        /// </summary>
        public static async Task<HttpResponseMessage> Create(BootpayCommerceObject bootpay, string orderSubscriptionId, CommerceOrderSubscriptionAdjustment adjustment, string idempotencyKey = null)
        {
            var payload = new CommerceOrderSubscriptionAdjustment
            {
                OrderSubscriptionAdjustmentId = adjustment?.OrderSubscriptionAdjustmentId,
                Duration = adjustment?.Duration ?? 1,
                Price = adjustment?.Price ?? 0,
                TaxFreePrice = adjustment?.TaxFreePrice ?? 0,
                Name = adjustment?.Name,
                Type = adjustment?.Type,
                CreatedAt = adjustment?.CreatedAt
            };
            return await bootpay.SendAsync($"order_subscriptions/{orderSubscriptionId}/adjustments", HttpMethod.Post, payload, CommerceRequestHeaders.Supervisor(idempotencyKey));
        }

        /// <summary>
        /// 특정 회차의 조정항목을 통째로 교체 (PUT /v1/order_subscriptions/{order_subscription_id}/adjustments)
        /// 서버는 duration(회차) 단위로 adjustments 배열을 갈아끼운다. duration 미지정시 1 이 전송된다.
        /// </summary>
        public static async Task<HttpResponseMessage> Update(BootpayCommerceObject bootpay, OrderSubscriptionAdjustmentUpdateParams updateParams, string idempotencyKey = null)
        {
            var payload = new OrderSubscriptionAdjustmentUpdateParams
            {
                Duration = updateParams.Duration ?? 1,
                Adjustments = updateParams.Adjustments,
                OrderSubscriptionAdjustmentId = updateParams.OrderSubscriptionAdjustmentId,
                Price = updateParams.Price,
                TaxFreePrice = updateParams.TaxFreePrice,
                Name = updateParams.Name,
                Type = updateParams.Type
            };
            return await bootpay.SendAsync($"order_subscriptions/{updateParams.OrderSubscriptionId}/adjustments", HttpMethod.Put, payload, CommerceRequestHeaders.Supervisor(idempotencyKey));
        }

        /// <summary>
        /// 조정항목 삭제 (DELETE /v1/order_subscriptions/{order_subscription_id}/adjustments)
        /// ⚠️ 대상 ID 는 query 가 아니라 body 로 보낸다.
        /// </summary>
        public static async Task<HttpResponseMessage> Delete(BootpayCommerceObject bootpay, string orderSubscriptionId, string orderSubscriptionAdjustmentId, string idempotencyKey = null)
        {
            var data = new { order_subscription_adjustment_id = orderSubscriptionAdjustmentId };
            return await bootpay.SendAsync($"order_subscriptions/{orderSubscriptionId}/adjustments", HttpMethod.Delete, data, CommerceRequestHeaders.Supervisor(idempotencyKey));
        }
    }
}
