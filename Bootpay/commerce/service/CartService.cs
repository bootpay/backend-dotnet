using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 장바구니 / 주문 미리보기 서비스
    /// </summary>
    public class CartService
    {
        /// <summary>
        /// 주문 미리보기 (배송비/할인 권위적 계산)
        /// </summary>
        public static async Task<HttpResponseMessage> OrderPreview(BootpayCommerceObject bootpay, OrderPreviewParams previewParams = null)
        {
            return await bootpay.SendAsync("cart/order-preview", HttpMethod.Post, previewParams ?? new OrderPreviewParams());
        }
    }
}
