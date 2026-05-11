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
        /// 카테고리 생성
        /// </summary>
        public static async Task<HttpResponseMessage> Create(BootpayCommerceObject bootpay, CategoryCreateParams createParams)
        {
            return await bootpay.SendAsync("categories", HttpMethod.Post, createParams);
        }

        /// <summary>
        /// 카테고리 수정
        /// </summary>
        public static async Task<HttpResponseMessage> Update(BootpayCommerceObject bootpay, CategoryUpdateParams updateParams)
        {
            return await bootpay.SendAsync($"categories/{updateParams.CategoryId}", HttpMethod.Put, updateParams);
        }

        /// <summary>
        /// 카테고리 삭제
        /// </summary>
        public static async Task<HttpResponseMessage> Destroy(BootpayCommerceObject bootpay, string categoryId)
        {
            return await bootpay.SendAsync($"categories/{categoryId}", HttpMethod.Delete);
        }
    }
}
