using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 상품 서비스
    /// 쓰기(등록/수정/삭제/상태변경)는 서버가 manager scope 를 요구한다.
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
        /// 상품 목록 조회 (V1 Mall API — GET /v1/products)
        /// page/limit 은 미지정시 각각 1 / 20 이 적용되고, 나머지 값은 지정된 것만 전송한다.
        /// ⚠️ keyword 는 서버가 읽지 않는다 — page/limit/category_id/sort 만 사용된다 (하위호환으로 인자는 유지).
        /// </summary>
        public static async Task<HttpResponseMessage> Products(BootpayCommerceObject bootpay, ProductListParams listParams = null, string idempotencyKey = null)
        {
            var mallParams = listParams as MallProductListParams;
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            queryParams["page"] = (listParams?.Page ?? 1).ToString();
            queryParams["limit"] = (listParams?.Limit ?? 20).ToString();
            if (!string.IsNullOrEmpty(mallParams?.CategoryId)) queryParams["category_id"] = mallParams.CategoryId;
            if (!string.IsNullOrEmpty(mallParams?.ExUid)) queryParams["ex_uid"] = mallParams.ExUid;
            if (!string.IsNullOrEmpty(mallParams?.Sort)) queryParams["sort"] = mallParams.Sort;
            if (!string.IsNullOrEmpty(listParams?.Keyword)) queryParams["keyword"] = listParams.Keyword;
            if (listParams?.Type != null) queryParams["type"] = listParams.Type.ToString();
            if (!string.IsNullOrEmpty(listParams?.PeriodType)) queryParams["period_type"] = listParams.PeriodType;
            if (!string.IsNullOrEmpty(listParams?.SAt)) queryParams["s_at"] = listParams.SAt;
            if (!string.IsNullOrEmpty(listParams?.EAt)) queryParams["e_at"] = listParams.EAt;
            if (!string.IsNullOrEmpty(listParams?.CategoryCode)) queryParams["category_code"] = listParams.CategoryCode;

            return await bootpay.SendAsync($"products?{queryParams}", HttpMethod.Get, null, CommerceRequestHeaders.Mall(mallParams?.UserJwt, idempotencyKey));
        }

        /// <summary>
        /// 상품 생성 (POST /v1/products) — manager scope
        /// </summary>
        public static async Task<HttpResponseMessage> Create(BootpayCommerceObject bootpay, CommerceProduct product, string idempotencyKey = null)
        {
            return await bootpay.SendAsync("products", HttpMethod.Post, product, CommerceRequestHeaders.Manager(idempotencyKey));
        }

        /// <summary>
        /// 상품 생성 (이미지 파일 포함) — manager scope
        /// imagePaths 가 있으면 multipart/form-data (images[0], images[1] ... 인덱싱), 없으면 JSON 으로 보낸다.
        /// </summary>
        /// <param name="bootpay">Bootpay Commerce 객체</param>
        /// <param name="product">상품 정보</param>
        /// <param name="imagePaths">이미지 파일 경로 배열</param>
        /// <param name="idempotencyKey">미지정시 자동 생성</param>
        public static async Task<HttpResponseMessage> CreateWithImages(BootpayCommerceObject bootpay, CommerceProduct product, List<string> imagePaths, string idempotencyKey = null)
        {
            if (imagePaths == null || imagePaths.Count == 0)
            {
                return await Create(bootpay, product, idempotencyKey);
            }
            return await bootpay.SendMultipartAsync("products", product, imagePaths, CommerceRequestHeaders.Manager(idempotencyKey));
        }

        /// <summary>
        /// 상품 상세 조회
        /// </summary>
        public static async Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string productId, string userJwt = null, string idempotencyKey = null)
        {
            return await bootpay.SendAsync($"products/{productId}", HttpMethod.Get, null, CommerceRequestHeaders.Mall(userJwt, idempotencyKey));
        }

        /// <summary>
        /// 상품 상세 조회 (V1 Mall API — GET /v1/products/{product_id})
        /// </summary>
        public static async Task<HttpResponseMessage> ProductDetail(BootpayCommerceObject bootpay, string productId, string userJwt = null, string idempotencyKey = null)
        {
            return await bootpay.SendAsync($"products/{productId}", HttpMethod.Get, null, CommerceRequestHeaders.Mall(userJwt, idempotencyKey));
        }

        /// <summary>
        /// 상품 수정 (PUT /v1/products/{product_id}) — manager scope
        /// 바뀐 값만 보내면 된다. ⚠️ category_id 는 키 존재 여부로 '해제 의사'를 판별하므로 주의.
        /// </summary>
        public static async Task<HttpResponseMessage> Update(BootpayCommerceObject bootpay, CommerceProduct product, string idempotencyKey = null)
        {
            return await bootpay.SendAsync($"products/{product.ProductId}", HttpMethod.Put, product, CommerceRequestHeaders.Manager(idempotencyKey));
        }

        /// <summary>
        /// 상품 상태 변경 (PUT /v1/products/{product_id}/status) — manager scope
        /// ⚠️ 재고(stock)는 여기가 아니라 update 로 바꾼다.
        /// </summary>
        public static async Task<HttpResponseMessage> Status(BootpayCommerceObject bootpay, ProductStatusParams statusParams, string idempotencyKey = null)
        {
            return await bootpay.SendAsync($"products/{statusParams.ProductId}/status", HttpMethod.Put, statusParams, CommerceRequestHeaders.Manager(idempotencyKey));
        }

        /// <summary>
        /// 상품 삭제 (DELETE /v1/products/{product_id}) — manager scope
        /// </summary>
        public static async Task<HttpResponseMessage> Delete(BootpayCommerceObject bootpay, string productId, string idempotencyKey = null)
        {
            return await bootpay.SendAsync($"products/{productId}", HttpMethod.Delete, null, CommerceRequestHeaders.Manager(idempotencyKey));
        }

        private static string BuildListQuery(ProductListParams listParams)
        {
            if (listParams == null) return "";

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (listParams.Page.HasValue) queryParams["page"] = listParams.Page.ToString();
            if (listParams.Limit.HasValue) queryParams["limit"] = listParams.Limit.ToString();
            if (!string.IsNullOrEmpty(listParams.Keyword)) queryParams["keyword"] = listParams.Keyword;
            if (!string.IsNullOrEmpty(listParams.CategoryId)) queryParams["category_id"] = listParams.CategoryId;
            if (!string.IsNullOrEmpty(listParams.ExUid)) queryParams["ex_uid"] = listParams.ExUid;
            if (!string.IsNullOrEmpty(listParams.Sort)) queryParams["sort"] = listParams.Sort;
            // 아래 4개는 서버가 읽지 않는다 — 기존 호출을 깨지 않으려고 전송만 유지한다
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
