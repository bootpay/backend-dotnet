using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 적립금 서비스
    /// </summary>
    public class PointService
    {
        /// <summary>
        /// 적립금 잔액 조회
        /// </summary>
        public static async Task<HttpResponseMessage> Balance(BootpayCommerceObject bootpay)
        {
            return await bootpay.SendAsync("point/balance", HttpMethod.Get);
        }

        /// <summary>
        /// 적립금 내역 조회
        /// </summary>
        public static async Task<HttpResponseMessage> Transactions(BootpayCommerceObject bootpay, PointTransactionsParams listParams = null)
        {
            var query = BuildListQuery(listParams);
            return await bootpay.SendAsync($"point/transactions{query}", HttpMethod.Get);
        }

        private static string BuildListQuery(PointTransactionsParams listParams)
        {
            if (listParams == null) return "";

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (listParams.Page.HasValue) queryParams["page"] = listParams.Page.ToString();
            if (listParams.Limit.HasValue) queryParams["limit"] = listParams.Limit.ToString();
            if (listParams.TransactionType.HasValue) queryParams["transaction_type"] = listParams.TransactionType.ToString();

            var query = queryParams.ToString();
            return string.IsNullOrEmpty(query) ? "" : $"?{query}";
        }
    }
}
