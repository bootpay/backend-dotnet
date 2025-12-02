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
        /// 청구서 목록 조회
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, ListParams listParams = null)
        {
            var query = BuildListQuery(listParams);
            return await bootpay.SendAsync($"invoices{query}", HttpMethod.Get);
        }

        /// <summary>
        /// 청구서 생성
        /// </summary>
        public static async Task<HttpResponseMessage> Create(BootpayCommerceObject bootpay, CommerceInvoice invoice)
        {
            return await bootpay.SendAsync("invoices", HttpMethod.Post, invoice);
        }

        /// <summary>
        /// 청구서 알림 발송
        /// </summary>
        public static async Task<HttpResponseMessage> Notify(BootpayCommerceObject bootpay, string invoiceId, List<int> sendTypes)
        {
            var data = new { send_types = sendTypes };
            return await bootpay.SendAsync($"invoices/{invoiceId}/notify", HttpMethod.Post, data);
        }

        /// <summary>
        /// 청구서 상세 조회
        /// </summary>
        public static async Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string invoiceId)
        {
            return await bootpay.SendAsync($"invoices/{invoiceId}", HttpMethod.Get);
        }

        private static string BuildListQuery(ListParams listParams)
        {
            if (listParams == null) return "";

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (listParams.Page.HasValue) queryParams["page"] = listParams.Page.ToString();
            if (listParams.Limit.HasValue) queryParams["limit"] = listParams.Limit.ToString();
            if (!string.IsNullOrEmpty(listParams.Keyword)) queryParams["keyword"] = listParams.Keyword;

            var query = queryParams.ToString();
            return string.IsNullOrEmpty(query) ? "" : $"?{query}";
        }
    }
}
