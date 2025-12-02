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
        /// 취소 요청 목록 조회
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, OrderCancelListParams listParams = null)
        {
            var query = BuildListQuery(listParams);
            return await bootpay.SendAsync($"order/cancel{query}", HttpMethod.Get);
        }

        /// <summary>
        /// 취소 요청
        /// </summary>
        public static async Task<HttpResponseMessage> Request(BootpayCommerceObject bootpay, OrderCancelParams cancelParams)
        {
            return await bootpay.SendAsync("order/cancel", HttpMethod.Post, cancelParams);
        }

        /// <summary>
        /// 취소 요청 철회
        /// </summary>
        public static async Task<HttpResponseMessage> Withdraw(BootpayCommerceObject bootpay, string orderCancelRequestHistoryId)
        {
            return await bootpay.SendAsync($"order/cancel/{orderCancelRequestHistoryId}/withdraw", HttpMethod.Put, new { });
        }

        /// <summary>
        /// 취소 승인
        /// </summary>
        public static async Task<HttpResponseMessage> Approve(BootpayCommerceObject bootpay, OrderCancelActionParams actionParams)
        {
            return await bootpay.SendAsync($"order/cancel/{actionParams.OrderCancelRequestHistoryId}/approve", HttpMethod.Put, actionParams);
        }

        /// <summary>
        /// 취소 거절
        /// </summary>
        public static async Task<HttpResponseMessage> Reject(BootpayCommerceObject bootpay, OrderCancelActionParams actionParams)
        {
            return await bootpay.SendAsync($"order/cancel/{actionParams.OrderCancelRequestHistoryId}/reject", HttpMethod.Put, actionParams);
        }

        private static string BuildListQuery(OrderCancelListParams listParams)
        {
            if (listParams == null) return "";

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(listParams.OrderId)) queryParams["order_id"] = listParams.OrderId;
            if (!string.IsNullOrEmpty(listParams.OrderNumber)) queryParams["order_number"] = listParams.OrderNumber;

            var query = queryParams.ToString();
            return string.IsNullOrEmpty(query) ? "" : $"?{query}";
        }
    }
}
