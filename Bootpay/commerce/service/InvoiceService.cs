using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 청구서 서비스
    /// </summary>
    public class InvoiceService
    {
        /// <summary>
        /// 청구서 목록 조회 (GET /v1/invoices)
        /// 응답은 { list: [...], count: N } 구조다 ({ items, total } 아님).
        /// limit 미지정시 서버 기본값과 동일한 24 를 보낸다.
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, ListParams listParams = null, string idempotencyKey = null)
        {
            var query = BuildListQuery(listParams);
            return await bootpay.SendAsync($"invoices{query}", HttpMethod.Get, null, CommerceRequestHeaders.User(idempotencyKey));
        }

        /// <summary>
        /// 청구서 생성
        /// </summary>
        public static async Task<HttpResponseMessage> Create(BootpayCommerceObject bootpay, CommerceInvoice invoice)
        {
            return await bootpay.SendAsync("invoices", HttpMethod.Post, invoice);
        }

        /// <summary>
        /// 청구서 알림 재발송 (POST /v1/invoices/{invoice_id}/notify)
        /// sendTypes 미전달시 서버가 빈 배열로 처리한다.
        /// ⚠️ 실제 고객에게 알림이 발송되므로 테스트 호출 주의.
        /// </summary>
        public static async Task<HttpResponseMessage> Notify(BootpayCommerceObject bootpay, string invoiceId, List<int> sendTypes = null, string idempotencyKey = null)
        {
            object data = sendTypes != null ? (object)new { send_types = sendTypes } : new { };
            return await bootpay.SendAsync($"invoices/{invoiceId}/notify", HttpMethod.Post, data, CommerceRequestHeaders.User(idempotencyKey));
        }

        /// <summary>
        /// 청구서 상세 조회
        /// </summary>
        public static async Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string invoiceId, string idempotencyKey = null)
        {
            return await bootpay.SendAsync($"invoices/{invoiceId}", HttpMethod.Get, null, CommerceRequestHeaders.User(idempotencyKey));
        }

        private static string BuildListQuery(ListParams listParams)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            queryParams["page"] = (listParams?.Page ?? 1).ToString();
            queryParams["limit"] = (listParams?.Limit ?? 24).ToString();
            if (!string.IsNullOrEmpty(listParams?.Keyword)) queryParams["keyword"] = listParams.Keyword;

            if (listParams is InvoiceListParams invoiceParams)
            {
                if (!string.IsNullOrEmpty(invoiceParams.CsType)) queryParams["cs_type"] = invoiceParams.CsType;
                if (!string.IsNullOrEmpty(invoiceParams.UserId)) queryParams["user_id"] = invoiceParams.UserId;
                if (invoiceParams.ProductType.HasValue) queryParams["product_type"] = invoiceParams.ProductType.ToString();
                if (!string.IsNullOrEmpty(invoiceParams.CssAt)) queryParams["css_at"] = invoiceParams.CssAt;
                if (!string.IsNullOrEmpty(invoiceParams.CseAt)) queryParams["cse_at"] = invoiceParams.CseAt;
            }

            return $"?{queryParams}";
        }
    }
}
