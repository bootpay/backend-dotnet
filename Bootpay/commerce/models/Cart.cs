using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bootpay.Commerce.Models
{
    /// <summary>
    /// 장바구니 아이템 페이로드
    /// </summary>
    public class CartItemPayload
    {
        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        [JsonProperty("product_option_id")]
        public string ProductOptionId { get; set; }

        [JsonProperty("quantity")]
        public int? Quantity { get; set; }

        [JsonProperty("is_subscription")]
        public bool? IsSubscription { get; set; }

        [JsonProperty("subscription_period_id")]
        public string SubscriptionPeriodId { get; set; }
    }

    /// <summary>
    /// 배송지 페이로드
    /// </summary>
    public class ShippingAddressPayload
    {
        [JsonProperty("zipcode")]
        public string Zipcode { get; set; }
    }

    /// <summary>
    /// 주문 미리보기 파라미터
    /// </summary>
    public class OrderPreviewParams
    {
        [JsonProperty("member_mode")]
        public string MemberMode { get; set; }

        [JsonProperty("cart_items")]
        public List<CartItemPayload> CartItems { get; set; }

        [JsonProperty("shipping_address")]
        public ShippingAddressPayload ShippingAddress { get; set; }

        [JsonProperty("coupon_ids")]
        public List<string> CouponIds { get; set; }

        [JsonProperty("point_amount")]
        public int? PointAmount { get; set; }

        [JsonProperty("user_group_id")]
        public string UserGroupId { get; set; }
    }

    /// <summary>
    /// 배송 그룹 아이템
    /// </summary>
    public class DeliveryGroupItem
    {
        [JsonProperty("cart_item_id")]
        public string CartItemId { get; set; }

        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        [JsonProperty("product_option_id")]
        public string ProductOptionId { get; set; }

        [JsonProperty("product_name")]
        public string ProductName { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("price")]
        public int Price { get; set; }

        [JsonProperty("subtotal")]
        public int? Subtotal { get; set; }
    }

    /// <summary>
    /// 배송 그룹
    /// </summary>
    public class DeliveryGroup
    {
        [JsonProperty("group_key")]
        public string GroupKey { get; set; }

        [JsonProperty("seller_id")]
        public string SellerId { get; set; }

        [JsonProperty("delivery_shipping_id")]
        public string DeliveryShippingId { get; set; }

        [JsonProperty("delivery_shipping_bundle_id")]
        public string DeliveryShippingBundleId { get; set; }

        [JsonProperty("bundle_id")]
        public string BundleId { get; set; }

        [JsonProperty("items")]
        public List<DeliveryGroupItem> Items { get; set; }

        [JsonProperty("total_price")]
        public int TotalPrice { get; set; }

        [JsonProperty("total_quantity")]
        public int TotalQuantity { get; set; }

        [JsonProperty("delivery_fee")]
        public int DeliveryFee { get; set; }

        [JsonProperty("delivery_extra_fee_jeju")]
        public int? DeliveryExtraFeeJeju { get; set; }

        [JsonProperty("delivery_extra_fee_remote")]
        public int? DeliveryExtraFeeRemote { get; set; }

        [JsonProperty("shipping_available")]
        public bool? ShippingAvailable { get; set; }
    }

    /// <summary>
    /// 적용된 쿠폰 스냅샷
    /// </summary>
    public class AppliedCouponSnapshot
    {
        [JsonProperty("coupon_id")]
        public string CouponId { get; set; }

        [JsonProperty("coupon_template_id")]
        public string CouponTemplateId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("discount_type")]
        public int? DiscountType { get; set; }

        [JsonProperty("discount_value")]
        public int? DiscountValue { get; set; }

        [JsonProperty("actual_discount_amount")]
        public int? ActualDiscountAmount { get; set; }
    }

    /// <summary>
    /// 주문 미리보기 요약
    /// </summary>
    public class OrderPreviewSummary
    {
        [JsonProperty("total_items")]
        public int TotalItems { get; set; }

        [JsonProperty("total_quantity")]
        public int TotalQuantity { get; set; }

        [JsonProperty("total_product_price")]
        public int TotalProductPrice { get; set; }

        [JsonProperty("total_delivery_fee")]
        public int TotalDeliveryFee { get; set; }

        [JsonProperty("total_delivery_extra_fee")]
        public int TotalDeliveryExtraFee { get; set; }

        [JsonProperty("coupon_discount_amount")]
        public int CouponDiscountAmount { get; set; }

        [JsonProperty("applied_coupons")]
        public List<AppliedCouponSnapshot> AppliedCoupons { get; set; }

        [JsonProperty("point_use_amount")]
        public int PointUseAmount { get; set; }

        [JsonProperty("point_max_usable")]
        public int PointMaxUsable { get; set; }

        [JsonProperty("point_balance_after")]
        public int PointBalanceAfter { get; set; }

        [JsonProperty("total_order_price")]
        public int TotalOrderPrice { get; set; }
    }

    /// <summary>
    /// 주문 미리보기 불가 아이템
    /// </summary>
    public class OrderPreviewUnavailableItem
    {
        [JsonProperty("cart_item_id")]
        public string CartItemId { get; set; }

        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        [JsonProperty("product_name")]
        public string ProductName { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }
    }

    /// <summary>
    /// 주문 미리보기 응답
    /// </summary>
    public class OrderPreviewResponse
    {
        [JsonProperty("cart_id")]
        public string CartId { get; set; }

        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("delivery_groups")]
        public List<DeliveryGroup> DeliveryGroups { get; set; }

        [JsonProperty("summary")]
        public OrderPreviewSummary Summary { get; set; }

        [JsonProperty("unavailable_items")]
        public List<OrderPreviewUnavailableItem> UnavailableItems { get; set; }
    }
}
