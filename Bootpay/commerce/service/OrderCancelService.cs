using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 주문 취소 서비스
    /// </summary>
    public class OrderCancelService
    {
        /// <summary>
        /// 취소 요청 목록 조회 (GET /v1/order/cancel)
        /// approve / reject / withdraw 에 넘길 order_cancellation_request_id 를 여기서 얻는다.
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, OrderCancelListParams listParams = null, string idempotencyKey = null)
        {
            var query = BuildListQuery(listParams);
            return await bootpay.SendAsync($"order/cancel{query}", HttpMethod.Get, null, CommerceRequestHeaders.User(idempotencyKey));
        }

        /// <summary>
        /// 취소 요청
        /// </summary>
        public static async Task<HttpResponseMessage> Request(BootpayCommerceObject bootpay, OrderCancelParams cancelParams)
        {
            return await bootpay.SendAsync("order/cancel", HttpMethod.Post, cancelParams);
        }

        /// <summary>
        /// (구매자) 취소 요청 철회 (PUT /v1/order/cancel/{order_cancellation_request_id}/withdraw)
        /// 정식 인자명은 order_cancellation_request_id 이며 구 이름과 같은 값이다.
        /// </summary>
        public static async Task<HttpResponseMessage> Withdraw(BootpayCommerceObject bootpay, string orderCancelRequestHistoryId, string idempotencyKey = null)
        {
            return await bootpay.SendAsync($"order/cancel/{orderCancelRequestHistoryId}/withdraw", HttpMethod.Put, new { }, CommerceRequestHeaders.User(idempotencyKey));
        }

        /// <summary>
        /// (관리자) 취소 요청 승인 (PUT /v1/order/cancel/{order_cancellation_request_id}/approve) — supervisor scope
        /// </summary>
        public static async Task<HttpResponseMessage> Approve(BootpayCommerceObject bootpay, OrderCancelActionParams actionParams, string idempotencyKey = null)
        {
            return await bootpay.SendAsync($"order/cancel/{CancellationId(actionParams)}/approve", HttpMethod.Put, actionParams, CommerceRequestHeaders.Supervisor(idempotencyKey));
        }

        /// <summary>
        /// (관리자) 취소 요청 반려 (PUT /v1/order/cancel/{order_cancellation_request_id}/reject) — supervisor scope
        /// </summary>
        public static async Task<HttpResponseMessage> Reject(BootpayCommerceObject bootpay, OrderCancelActionParams actionParams, string idempotencyKey = null)
        {
            return await bootpay.SendAsync($"order/cancel/{CancellationId(actionParams)}/reject", HttpMethod.Put, actionParams, CommerceRequestHeaders.Supervisor(idempotencyKey));
        }

        /// <summary>
        /// 취소 요청 이력 ID — 정식 이름은 order_cancellation_request_id 이며, 구 이름도 계속 받는다.
        /// </summary>
        private static string CancellationId(OrderCancelActionParams actionParams)
        {
            return string.IsNullOrEmpty(actionParams.OrderCancellationRequestId)
                ? actionParams.OrderCancelRequestHistoryId
                : actionParams.OrderCancellationRequestId;
        }

        private static string BuildListQuery(OrderCancelListParams listParams)
        {
            if (listParams == null) return "";

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(listParams.OrderNumber)) queryParams["order_number"] = listParams.OrderNumber;
            if (!string.IsNullOrEmpty(listParams.OrderId)) queryParams["order_id"] = listParams.OrderId;

            var query = queryParams.ToString();
            return string.IsNullOrEmpty(query) ? "" : $"?{query}";
        }
    }
}
