using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 쿠폰 서비스
    /// </summary>
    public class CouponService
    {
        /// <summary>
        /// 사용자 보유 쿠폰 목록
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, CouponListParams listParams = null)
        {
            var query = BuildListQuery(listParams);
            return await bootpay.SendAsync($"coupon{query}", HttpMethod.Get);
        }

        /// <summary>
        /// 다운로드 가능한 쿠폰 목록
        /// </summary>
        public static async Task<HttpResponseMessage> Available(BootpayCommerceObject bootpay)
        {
            return await bootpay.SendAsync("coupon/available", HttpMethod.Get);
        }

        /// <summary>
        /// 쿠폰 다운로드
        /// </summary>
        public static async Task<HttpResponseMessage> Download(BootpayCommerceObject bootpay, CouponDownloadParams downloadParams)
        {
            return await bootpay.SendAsync("coupon/download", HttpMethod.Post, downloadParams);
        }

        private static string BuildListQuery(CouponListParams listParams)
        {
            if (listParams == null) return "";

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (!string.IsNullOrEmpty(listParams.Status)) queryParams["status"] = listParams.Status;
            if (listParams.Page.HasValue) queryParams["page"] = listParams.Page.ToString();
            if (listParams.Limit.HasValue) queryParams["limit"] = listParams.Limit.ToString();

            var query = queryParams.ToString();
            return string.IsNullOrEmpty(query) ? "" : $"?{query}";
        }
    }
}
