using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 카테고리 서비스
    /// </summary>
    public class CategoryService
    {
        /// <summary>
        /// 카테고리 트리 조회
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay)
        {
            return await bootpay.SendAsync("categories", HttpMethod.Get);
        }

        /// <summary>
        /// 카테고리 단건 조회
        /// </summary>
        public static async Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string categoryId)
        {
            return await bootpay.SendAsync($"categories/{categoryId}", HttpMethod.Get);
        }

        /// <summary>
        /// 카테고리 생성 (POST /v1/categories) — supervisor scope
        /// ⚠️ 서버가 supervisor scope 를 요구한다 (scope_invalid!).
        /// </summary>
        public static async Task<HttpResponseMessage> Create(BootpayCommerceObject bootpay, CategoryCreateParams createParams, string idempotencyKey = null)
        {
            return await bootpay.SendAsync("categories", HttpMethod.Post, createParams, CommerceRequestHeaders.Supervisor(idempotencyKey));
        }

        /// <summary>
        /// 카테고리 수정 (PUT /v1/categories/{category_id}) — supervisor scope
        /// ⚠️ 서버가 supervisor scope 를 요구한다 (scope_invalid!).
        /// </summary>
        public static async Task<HttpResponseMessage> Update(BootpayCommerceObject bootpay, CategoryUpdateParams updateParams, string idempotencyKey = null)
        {
            return await bootpay.SendAsync($"categories/{updateParams.CategoryId}", HttpMethod.Put, updateParams, CommerceRequestHeaders.Supervisor(idempotencyKey));
        }

        /// <summary>
        /// 카테고리 삭제 (DELETE /v1/categories/{category_id}) — supervisor scope
        /// ⚠️ 서버가 supervisor scope 를 요구한다 (scope_invalid!).
        /// </summary>
        public static async Task<HttpResponseMessage> Destroy(BootpayCommerceObject bootpay, string categoryId, string idempotencyKey = null)
        {
            return await bootpay.SendAsync($"categories/{categoryId}", HttpMethod.Delete, null, CommerceRequestHeaders.Supervisor(idempotencyKey));
        }
    }
}
