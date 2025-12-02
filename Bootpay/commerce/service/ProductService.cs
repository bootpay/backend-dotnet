using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 상품 서비스
    /// </summary>
    public class ProductService
    {
        /// <summary>
        /// 상품 목록 조회
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, ProductListParams listParams = null)
        {
            var query = BuildListQuery(listParams);
            return await bootpay.SendAsync($"products{query}", HttpMethod.Get);
        }

        /// <summary>
        /// 상품 생성
        /// </summary>
        public static async Task<HttpResponseMessage> Create(BootpayCommerceObject bootpay, CommerceProduct product)
        {
            return await bootpay.SendAsync("products", HttpMethod.Post, product);
        }

        /// <summary>
        /// 상품 생성 (이미지 파일 포함)
        /// </summary>
        /// <param name="bootpay">Bootpay Commerce 객체</param>
        /// <param name="product">상품 정보</param>
        /// <param name="imagePaths">이미지 파일 경로 배열</param>
        public static async Task<HttpResponseMessage> CreateWithImages(BootpayCommerceObject bootpay, CommerceProduct product, List<string> imagePaths)
        {
            return await bootpay.SendMultipartAsync("products", product, imagePaths);
        }

        /// <summary>
        /// 상품 상세 조회
        /// </summary>
        public static async Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string productId)
        {
            return await bootpay.SendAsync($"products/{productId}", HttpMethod.Get);
        }

        /// <summary>
        /// 상품 수정
        /// </summary>
        public static async Task<HttpResponseMessage> Update(BootpayCommerceObject bootpay, CommerceProduct product)
        {
            return await bootpay.SendAsync($"products/{product.ProductId}", HttpMethod.Put, product);
        }

        /// <summary>
        /// 상품 상태 변경
        /// </summary>
        public static async Task<HttpResponseMessage> Status(BootpayCommerceObject bootpay, ProductStatusParams statusParams)
        {
            return await bootpay.SendAsync($"products/{statusParams.ProductId}/status", HttpMethod.Put, statusParams);
        }

        /// <summary>
        /// 상품 삭제
        /// </summary>
        public static async Task<HttpResponseMessage> Delete(BootpayCommerceObject bootpay, string productId)
        {
            return await bootpay.SendAsync($"products/{productId}", HttpMethod.Delete);
        }

        private static string BuildListQuery(ProductListParams listParams)
        {
            if (listParams == null) return "";

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (listParams.Page.HasValue) queryParams["page"] = listParams.Page.ToString();
            if (listParams.Limit.HasValue) queryParams["limit"] = listParams.Limit.ToString();
            if (!string.IsNullOrEmpty(listParams.Keyword)) queryParams["keyword"] = listParams.Keyword;
            if (listParams.Type.HasValue) queryParams["type"] = listParams.Type.ToString();
            if (!string.IsNullOrEmpty(listParams.PeriodType)) queryParams["period_type"] = listParams.PeriodType;
            if (!string.IsNullOrEmpty(listParams.SAt)) queryParams["s_at"] = listParams.SAt;
            if (!string.IsNullOrEmpty(listParams.EAt)) queryParams["e_at"] = listParams.EAt;
            if (!string.IsNullOrEmpty(listParams.CategoryCode)) queryParams["category_code"] = listParams.CategoryCode;

            var query = queryParams.ToString();
            return string.IsNullOrEmpty(query) ? "" : $"?{query}";
        }
    }
}
