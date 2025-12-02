using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bootpay.Commerce.Models
{
    /// <summary>
    /// Commerce 상품
    /// </summary>
    public class CommerceProduct
    {
        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        [JsonProperty("category_id")]
        public string CategoryId { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("seller_id")]
        public string SellerId { get; set; }

        [JsonProperty("subscription_setting_id")]
        public string SubscriptionSettingId { get; set; }

        [JsonProperty("delivery_shipping_id")]
        public string DeliveryShippingId { get; set; }

        [JsonProperty("brand_id")]
        public string BrandId { get; set; }

        [JsonProperty("manufacturer_id")]
        public string ManufacturerId { get; set; }

        [JsonProperty("ex_uid")]
        public string ExUid { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("images")]
        public List<string> Images { get; set; }

        [JsonProperty("type")]
        public int? Type { get; set; }

        [JsonProperty("tax_type")]
        public int? TaxType { get; set; }

        [JsonProperty("use_stock")]
        public bool? UseStock { get; set; }

        [JsonProperty("stock")]
        public int? Stock { get; set; }

        [JsonProperty("use_option_stock")]
        public bool? UseOptionStock { get; set; }

        [JsonProperty("use_stock_safe")]
        public bool? UseStockSafe { get; set; }

        [JsonProperty("stock_safe")]
        public int? StockSafe { get; set; }

        [JsonProperty("display_price")]
        public int? DisplayPrice { get; set; }

        [JsonProperty("tax_free_price")]
        public int? TaxFreePrice { get; set; }

        [JsonProperty("use_discount")]
        public bool? UseDiscount { get; set; }

        [JsonProperty("discount_price")]
        public int? DiscountPrice { get; set; }

        [JsonProperty("discount_price_type")]
        public int? DiscountPriceType { get; set; }

        [JsonProperty("use_discount_period")]
        public bool? UseDiscountPeriod { get; set; }

        [JsonProperty("discount_start_at")]
        public string DiscountStartAt { get; set; }

        [JsonProperty("discount_end_at")]
        public string DiscountEndAt { get; set; }

        [JsonProperty("use_accumulation")]
        public bool? UseAccumulation { get; set; }

        [JsonProperty("accumulation_point")]
        public int? AccumulationPoint { get; set; }

        [JsonProperty("accumulation_point_type")]
        public int? AccumulationPointType { get; set; }

        [JsonProperty("status_display")]
        public bool? StatusDisplay { get; set; }

        [JsonProperty("use_display_period")]
        public bool? UseDisplayPeriod { get; set; }

        [JsonProperty("display_start_at")]
        public string DisplayStartAt { get; set; }

        [JsonProperty("display_end_at")]
        public string DisplayEndAt { get; set; }

        [JsonProperty("status_sale")]
        public bool? StatusSale { get; set; }

        [JsonProperty("use_sale_period")]
        public bool? UseSalePeriod { get; set; }

        [JsonProperty("sale_start_at")]
        public string SaleStartAt { get; set; }

        [JsonProperty("sale_end_at")]
        public string SaleEndAt { get; set; }

        [JsonProperty("count_sale")]
        public int? CountSale { get; set; }

        [JsonProperty("count_qna")]
        public int? CountQna { get; set; }

        [JsonProperty("count_like")]
        public int? CountLike { get; set; }

        [JsonProperty("count_review")]
        public int? CountReview { get; set; }

        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        [JsonProperty("sku")]
        public string Sku { get; set; }

        [JsonProperty("search_tags")]
        public List<string> SearchTags { get; set; }

        [JsonProperty("event_tags")]
        public List<string> EventTags { get; set; }

        [JsonProperty("target_user_tags")]
        public List<string> TargetUserTags { get; set; }

        [JsonProperty("delivery_tags")]
        public List<string> DeliveryTags { get; set; }

        [JsonProperty("emotion_tags")]
        public List<string> EmotionTags { get; set; }

        [JsonProperty("use_coupon")]
        public bool? UseCoupon { get; set; }

        [JsonProperty("use_minor")]
        public bool? UseMinor { get; set; }

        [JsonProperty("use_free_gift")]
        public bool? UseFreeGift { get; set; }

        [JsonProperty("free_gift")]
        public string FreeGift { get; set; }

        [JsonProperty("use_bulk_purchase_discount")]
        public bool? UseBulkPurchaseDiscount { get; set; }

        [JsonProperty("bulk_purchase_discount")]
        public Dictionary<string, object> BulkPurchaseDiscount { get; set; }

        [JsonProperty("use_review_point")]
        public bool? UseReviewPoint { get; set; }

        [JsonProperty("review_point")]
        public Dictionary<string, object> ReviewPoint { get; set; }

        [JsonProperty("use_seo")]
        public bool? UseSeo { get; set; }

        [JsonProperty("seo_page_title")]
        public string SeoPageTitle { get; set; }

        [JsonProperty("seo_meta_description")]
        public string SeoMetaDescription { get; set; }

        [JsonProperty("seo_meta_tags")]
        public List<string> SeoMetaTags { get; set; }

        [JsonProperty("model_id")]
        public string ModelId { get; set; }

        [JsonProperty("model_name")]
        public string ModelName { get; set; }

        [JsonProperty("manufacturer_name")]
        public string ManufacturerName { get; set; }

        [JsonProperty("brand_name")]
        public string BrandName { get; set; }

        [JsonProperty("origin_code")]
        public string OriginCode { get; set; }

        [JsonProperty("origin_name")]
        public string OriginName { get; set; }

        [JsonProperty("importer")]
        public string Importer { get; set; }

        [JsonProperty("used")]
        public bool? Used { get; set; }

        [JsonProperty("expired_at")]
        public string ExpiredAt { get; set; }

        [JsonProperty("manufactured_at")]
        public string ManufacturedAt { get; set; }

        [JsonProperty("use_setup_fee")]
        public bool? UseSetupFee { get; set; }

        [JsonProperty("setup_fee_value")]
        public int? SetupFeeValue { get; set; }

        [JsonProperty("setup_fee_type")]
        public int? SetupFeeType { get; set; }

        [JsonProperty("setup_fee_name")]
        public string SetupFeeName { get; set; }

        [JsonProperty("setup_fee_text")]
        public string SetupFeeText { get; set; }

        [JsonProperty("use_delivery_shipping")]
        public bool? UseDeliveryShipping { get; set; }

        [JsonProperty("delivery_shipping_fee_type")]
        public int? DeliveryShippingFeeType { get; set; }

        [JsonProperty("use_overseas_shipping")]
        public bool? UseOverseasShipping { get; set; }

        [JsonProperty("use_delivery_shipping_bundle")]
        public bool? UseDeliveryShippingBundle { get; set; }

        [JsonProperty("delivery_shipping_bundle_id")]
        public string DeliveryShippingBundleId { get; set; }

        [JsonProperty("use_subscription")]
        public bool? UseSubscription { get; set; }

        [JsonProperty("use_subscription_times")]
        public bool? UseSubscriptionTimes { get; set; }

        [JsonProperty("use_product_price")]
        public bool? UseProductPrice { get; set; }

        [JsonProperty("use_cancel")]
        public bool? UseCancel { get; set; }

        [JsonProperty("use_able_refund")]
        public bool? UseAbleRefund { get; set; }

        [JsonProperty("use_able_cart")]
        public bool? UseAbleCart { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonProperty("options")]
        public List<CommerceProductOption> Options { get; set; }

        [JsonProperty("subscription_setting")]
        public CommerceSubscriptionSetting SubscriptionSetting { get; set; }
    }

    /// <summary>
    /// 상품 옵션
    /// </summary>
    public class CommerceProductOption
    {
        [JsonProperty("option_id")]
        public string OptionId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("price")]
        public int? Price { get; set; }

        [JsonProperty("stock")]
        public int? Stock { get; set; }
    }

    /// <summary>
    /// 구독 설정
    /// </summary>
    public class CommerceSubscriptionSetting
    {
        [JsonProperty("subscription_setting_id")]
        public string SubscriptionSettingId { get; set; }

        [JsonProperty("period_type")]
        public string PeriodType { get; set; }

        [JsonProperty("period_value")]
        public int? PeriodValue { get; set; }

        [JsonProperty("billing_day")]
        public int? BillingDay { get; set; }

        [JsonProperty("billing_count")]
        public int? BillingCount { get; set; }
    }

    /// <summary>
    /// 상품 목록 조회 파라미터
    /// </summary>
    public class ProductListParams : ListParams
    {
        [JsonProperty("type")]
        public int? Type { get; set; }

        [JsonProperty("period_type")]
        public string PeriodType { get; set; }

        [JsonProperty("s_at")]
        public string SAt { get; set; }

        [JsonProperty("e_at")]
        public string EAt { get; set; }

        [JsonProperty("category_code")]
        public string CategoryCode { get; set; }
    }

    /// <summary>
    /// 상품 상태 변경 파라미터
    /// </summary>
    public class ProductStatusParams
    {
        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        [JsonProperty("status")]
        public int Status { get; set; }

        [JsonProperty("status_display")]
        public bool? StatusDisplay { get; set; }

        [JsonProperty("status_sale")]
        public bool? StatusSale { get; set; }
    }
}
