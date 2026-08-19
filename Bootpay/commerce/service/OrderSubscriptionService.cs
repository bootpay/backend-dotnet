using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 정기구독 서비스
    /// </summary>
    public class OrderSubscriptionService
    {
        /// <summary>
        /// 정기구독 목록 조회
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, OrderSubscriptionListParams listParams = null)
        {
            var query = BuildListQuery(listParams);
            return await bootpay.SendAsync($"order_subscriptions{query}", HttpMethod.Get);
        }

        /// <summary>
        /// 정기구독 상세 조회
        /// </summary>
        public static async Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string orderSubscriptionId)
        {
            return await bootpay.SendAsync($"order_subscriptions/{orderSubscriptionId}", HttpMethod.Get);
        }

        /// <summary>
        /// 구독 계약 내용 변경 (PUT /v1/order_subscriptions/{order_subscription_id}) — supervisor scope
        /// 바뀐 값만 보내면 된다 (나머지는 서버가 그대로 유지한다).
        /// </summary>
        public static async Task<HttpResponseMessage> Update(BootpayCommerceObject bootpay, OrderSubscriptionUpdateParams updateParams, string idempotencyKey = null)
        {
            return await bootpay.SendAsync($"order_subscriptions/{updateParams.OrderSubscriptionId}", HttpMethod.Put, updateParams, CommerceRequestHeaders.Supervisor(idempotencyKey));
        }

        /// <summary>
        /// 정기구독 일시정지 요청 (POST /v1/order_subscriptions/requests/ing/pause) — user scope
        /// </summary>
        public static async Task<HttpResponseMessage> Pause(BootpayCommerceObject bootpay, OrderSubscriptionPauseParams pauseParams, string idempotencyKey = null)
        {
            return await bootpay.SendAsync("order_subscriptions/requests/ing/pause", HttpMethod.Post, pauseParams, CommerceRequestHeaders.User(idempotencyKey));
        }

        /// <summary>
        /// 정기구독 재개 요청 (PUT /v1/order_subscriptions/requests/ing/resume) — user scope
        /// ⚠️ requests/ing 계열 중 유일하게 PUT 이다. 오타로 보고 POST 로 바꾸지 말 것.
        /// </summary>
        public static async Task<HttpResponseMessage> Resume(BootpayCommerceObject bootpay, OrderSubscriptionResumeParams resumeParams, string idempotencyKey = null)
        {
            return await bootpay.SendAsync("order_subscriptions/requests/ing/resume", HttpMethod.Put, resumeParams, CommerceRequestHeaders.User(idempotencyKey));
        }

        /// <summary>
        /// 중도인수 요청 (POST /v1/order_subscriptions/requests/ing/purchase) — user scope
        /// </summary>
        public static async Task<HttpResponseMessage> Purchase(BootpayCommerceObject bootpay, OrderSubscriptionPurchaseParams purchaseParams, string idempotencyKey = null)
        {
            return await bootpay.SendAsync("order_subscriptions/requests/ing/purchase", HttpMethod.Post, purchaseParams, CommerceRequestHeaders.User(idempotencyKey));
        }

        /// <summary>
        /// 구독 이전/승계 요청 (POST /v1/order_subscriptions/requests/ing/transfer) — user scope
        /// </summary>
        public static async Task<HttpResponseMessage> Transfer(BootpayCommerceObject bootpay, OrderSubscriptionTransferParams transferParams, string idempotencyKey = null)
        {
            return await bootpay.SendAsync("order_subscriptions/requests/ing/transfer", HttpMethod.Post, transferParams, CommerceRequestHeaders.User(idempotencyKey));
        }

        /// <summary>
        /// 해지 수수료 계산 — user scope
        /// </summary>
        public static async Task<HttpResponseMessage> CalculateTerminationFee(BootpayCommerceObject bootpay, string orderSubscriptionId = null, string orderNumber = null, string idempotencyKey = null)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(orderSubscriptionId))
                queryParams["order_subscription_id"] = orderSubscriptionId;
            if (!string.IsNullOrEmpty(orderNumber))
                queryParams["order_number"] = orderNumber;

            return await bootpay.SendAsync($"order_subscriptions/requests/ing/calculate_termination_fee?{queryParams}", HttpMethod.Get, null, CommerceRequestHeaders.User(idempotencyKey));
        }

        /// <summary>
        /// 주문번호로 해지 수수료 계산
        /// </summary>
        public static async Task<HttpResponseMessage> CalculateTerminationFeeByOrderNumber(BootpayCommerceObject bootpay, string orderNumber)
        {
            return await CalculateTerminationFee(bootpay, null, orderNumber);
        }

        /// <summary>
        /// 정기구독 해지 (POST /v1/order_subscriptions/requests/ing/termination) — user scope
        /// </summary>
        public static async Task<HttpResponseMessage> Termination(BootpayCommerceObject bootpay, OrderSubscriptionTerminationParams terminationParams, string idempotencyKey = null)
        {
            return await bootpay.SendAsync("order_subscriptions/requests/ing/termination", HttpMethod.Post, terminationParams, CommerceRequestHeaders.User(idempotencyKey));
        }

        public static async Task<HttpResponseMessage> SupervisorApprove(BootpayCommerceObject bootpay, string orderSubscriptionId, SupervisorOrderSubscriptionApproveParams approveParams = null)
        {
            return await bootpay.SendAsync($"order_subscriptions/{orderSubscriptionId}/approve", HttpMethod.Put, approveParams ?? new SupervisorOrderSubscriptionApproveParams());
        }

        public static async Task<HttpResponseMessage> SupervisorReject(BootpayCommerceObject bootpay, string orderSubscriptionId, SupervisorOrderSubscriptionRejectParams rejectParams = null)
        {
            return await bootpay.SendAsync($"order_subscriptions/{orderSubscriptionId}/reject", HttpMethod.Put, rejectParams ?? new SupervisorOrderSubscriptionRejectParams());
        }

        public static async Task<HttpResponseMessage> SupervisorTerminate(BootpayCommerceObject bootpay, string orderSubscriptionId, SupervisorOrderSubscriptionTerminateParams terminateParams = null)
        {
            return await bootpay.SendAsync($"order_subscriptions/{orderSubscriptionId}/terminate", HttpMethod.Put, terminateParams ?? new SupervisorOrderSubscriptionTerminateParams());
        }

        public static async Task<HttpResponseMessage> SupervisorPause(BootpayCommerceObject bootpay, string orderSubscriptionId, SupervisorOrderSubscriptionPauseParams pauseParams)
        {
            return await bootpay.SendAsync($"order_subscriptions/{orderSubscriptionId}/pause", HttpMethod.Put, pauseParams);
        }

        public static async Task<HttpResponseMessage> SupervisorResume(BootpayCommerceObject bootpay, string orderSubscriptionId, SupervisorOrderSubscriptionResumeParams resumeParams = null)
        {
            return await bootpay.SendAsync($"order_subscriptions/{orderSubscriptionId}/resume", HttpMethod.Put, resumeParams ?? new SupervisorOrderSubscriptionResumeParams());
        }

        /// <summary>
        /// 수시결제(온디맨드) charge_key 즉시 결제 (POST /v1/order_subscriptions/charge) — supervisor scope
        /// charge_key 는 body 로만 전송한다 (URL/query 금지 — 액세스 로그 노출 방지)
        /// </summary>
        public static async Task<HttpResponseMessage> SupervisorCharge(BootpayCommerceObject bootpay, SupervisorOrderSubscriptionChargeParams chargeParams, string idempotencyKey = null)
        {
            return await bootpay.SendAsync("order_subscriptions/charge", HttpMethod.Post, chargeParams, CommerceRequestHeaders.Supervisor(idempotencyKey));
        }

        /// <summary>
        /// 수시결제(온디맨드) charge_key 해지 (DELETE /v1/order_subscriptions/charge) — supervisor scope
        /// 해지 이후 해당 키로의 재결제는 불가능하다. 대상 charge_key 는 body 로 전송한다.
        /// </summary>
        public static async Task<HttpResponseMessage> SupervisorChargeRevoke(BootpayCommerceObject bootpay, SupervisorOrderSubscriptionChargeRevokeParams revokeParams, string idempotencyKey = null)
        {
            return await bootpay.SendAsync("order_subscriptions/charge", HttpMethod.Delete, revokeParams, CommerceRequestHeaders.Supervisor(idempotencyKey));
        }

        private static string BuildListQuery(OrderSubscriptionListParams listParams)
        {
            if (listParams == null) return "";

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (listParams.Page.HasValue) queryParams["page"] = listParams.Page.ToString();
            if (listParams.Limit.HasValue) queryParams["limit"] = listParams.Limit.ToString();
            if (!string.IsNullOrEmpty(listParams.Keyword)) queryParams["keyword"] = listParams.Keyword;
            if (!string.IsNullOrEmpty(listParams.SearchDateFrom)) queryParams["search_date_from"] = listParams.SearchDateFrom;
            if (!string.IsNullOrEmpty(listParams.SearchDateTo)) queryParams["search_date_to"] = listParams.SearchDateTo;
            if (!string.IsNullOrEmpty(listParams.SAt)) queryParams["s_at"] = listParams.SAt;
            if (!string.IsNullOrEmpty(listParams.EAt)) queryParams["e_at"] = listParams.EAt;
            if (!string.IsNullOrEmpty(listParams.RequestType)) queryParams["request_type"] = listParams.RequestType;
            if (!string.IsNullOrEmpty(listParams.UserGroupId)) queryParams["user_group_id"] = listParams.UserGroupId;
            if (listParams.Status.HasValue) queryParams["status"] = listParams.Status.ToString();
            if (!string.IsNullOrEmpty(listParams.UserId)) queryParams["user_id"] = listParams.UserId;

            var query = queryParams.ToString();
            return string.IsNullOrEmpty(query) ? "" : $"?{query}";
        }
    }
}
