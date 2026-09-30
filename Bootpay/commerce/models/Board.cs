using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bootpay.Commerce.Models
{
    public class BoardImage
    {
        [JsonProperty("url")] public string Url { get; set; }
    }

    #region Notice

    public class NoticeListParams
    {
        [JsonProperty("page")] public int? Page { get; set; }
        [JsonProperty("limit")] public int? Limit { get; set; }
        [JsonProperty("keyword")] public string Keyword { get; set; }
        [JsonProperty("view")] public string View { get; set; }
        [JsonIgnore] public bool? Supervisor { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class NoticeDetailParams
    {
        [JsonIgnore] public bool? Supervisor { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class NoticeCreateParams
    {
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("images")] public List<object> Images { get; set; }
        [JsonProperty("is_notice")] public bool? IsNotice { get; set; }
        [JsonProperty("is_display")] public bool? IsDisplay { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class NoticeUpdateParams
    {
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("images")] public List<object> Images { get; set; }
        [JsonProperty("is_notice")] public bool? IsNotice { get; set; }
        [JsonProperty("is_display")] public bool? IsDisplay { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class CommerceNotice
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("images")] public List<object> Images { get; set; }
        [JsonProperty("is_notice")] public bool? IsNotice { get; set; }
        [JsonProperty("is_display")] public bool? IsDisplay { get; set; }
        [JsonProperty("view_count")] public int? ViewCount { get; set; }
        [JsonProperty("created_at")] public string CreatedAt { get; set; }
        [JsonProperty("updated_at")] public string UpdatedAt { get; set; }
    }

    public class NoticeListResponse
    {
        [JsonProperty("list")] public List<CommerceNotice> List { get; set; }
        [JsonProperty("count")] public int? Count { get; set; }
        [JsonProperty("page")] public int? Page { get; set; }
        [JsonProperty("limit")] public int? Limit { get; set; }
    }

    #endregion

    #region FAQ

    public class FaqListParams
    {
        [JsonProperty("page")] public int? Page { get; set; }
        [JsonProperty("limit")] public int? Limit { get; set; }
        [JsonProperty("keyword")] public string Keyword { get; set; }
        [JsonProperty("view")] public string View { get; set; }
        [JsonIgnore] public bool? Supervisor { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class FaqDetailParams
    {
        [JsonIgnore] public bool? Supervisor { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class FaqCreateParams
    {
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("images")] public List<object> Images { get; set; }
        [JsonProperty("is_display")] public bool? IsDisplay { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class FaqUpdateParams
    {
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("images")] public List<object> Images { get; set; }
        [JsonProperty("is_display")] public bool? IsDisplay { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class CommerceFaq
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("images")] public List<object> Images { get; set; }
        [JsonProperty("is_display")] public bool? IsDisplay { get; set; }
        [JsonProperty("view_count")] public int? ViewCount { get; set; }
        [JsonProperty("created_at")] public string CreatedAt { get; set; }
        [JsonProperty("updated_at")] public string UpdatedAt { get; set; }
    }

    public class FaqListResponse
    {
        [JsonProperty("list")] public List<CommerceFaq> List { get; set; }
        [JsonProperty("count")] public int? Count { get; set; }
        [JsonProperty("page")] public int? Page { get; set; }
        [JsonProperty("limit")] public int? Limit { get; set; }
    }

    #endregion

    #region Inquiry

    public class InquiryListParams
    {
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("login_id")] public string LoginId { get; set; }
        [JsonIgnore] public string UserJwt { get; set; }
        [JsonProperty("answered")] public bool? Answered { get; set; }
        [JsonProperty("page")] public int? Page { get; set; }
        [JsonProperty("limit")] public int? Limit { get; set; }
        [JsonIgnore] public bool? Supervisor { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class InquiryDetailParams
    {
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("login_id")] public string LoginId { get; set; }
        [JsonIgnore] public string UserJwt { get; set; }
        [JsonIgnore] public bool? Supervisor { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class InquiryCreateParams
    {
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("login_id")] public string LoginId { get; set; }
        [JsonIgnore] public string UserJwt { get; set; }
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("product_id")] public string ProductId { get; set; }
        [JsonProperty("option")] public string Option { get; set; }
        [JsonProperty("order_id")] public string OrderId { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class InquiryUpdateParams
    {
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("login_id")] public string LoginId { get; set; }
        [JsonIgnore] public string UserJwt { get; set; }
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("content")] public string Content { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class InquiryDeleteParams
    {
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("login_id")] public string LoginId { get; set; }
        [JsonIgnore] public string UserJwt { get; set; }
        [JsonIgnore] public bool? Supervisor { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class CommerceInquiryAnswer
    {
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("answered_at")] public string AnsweredAt { get; set; }
    }

    public class CommerceInquiry
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("login_id")] public string LoginId { get; set; }
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("product_id")] public string ProductId { get; set; }
        [JsonProperty("option")] public string Option { get; set; }
        [JsonProperty("order_id")] public string OrderId { get; set; }
        [JsonProperty("answered")] public bool? Answered { get; set; }
        [JsonProperty("answer")] public CommerceInquiryAnswer Answer { get; set; }
        [JsonProperty("created_at")] public string CreatedAt { get; set; }
        [JsonProperty("updated_at")] public string UpdatedAt { get; set; }
    }

    public class InquiryListResponse
    {
        [JsonProperty("list")] public List<CommerceInquiry> List { get; set; }
        [JsonProperty("count")] public int? Count { get; set; }
        [JsonProperty("page")] public int? Page { get; set; }
        [JsonProperty("limit")] public int? Limit { get; set; }
    }

    #endregion

    #region Product Q&A

    public class ProductQnaListParams
    {
        [JsonProperty("product_id")] public string ProductId { get; set; }
        [JsonProperty("view")] public string View { get; set; }
        [JsonProperty("page")] public int? Page { get; set; }
        [JsonProperty("limit")] public int? Limit { get; set; }
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("login_id")] public string LoginId { get; set; }
        [JsonIgnore] public string UserJwt { get; set; }
        [JsonIgnore] public bool? Supervisor { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class ProductQnaDetailParams
    {
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("login_id")] public string LoginId { get; set; }
        [JsonIgnore] public string UserJwt { get; set; }
        [JsonIgnore] public bool? Supervisor { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class ProductQnaCreateParams
    {
        [JsonProperty("product_id")] public string ProductId { get; set; }
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("product_option_id")] public string ProductOptionId { get; set; }
        [JsonProperty("option_text")] public string OptionText { get; set; }
        [JsonProperty("is_secret")] public bool? IsSecret { get; set; }
        [JsonProperty("guest_name")] public string GuestName { get; set; }
        [JsonProperty("guest_password")] public string GuestPassword { get; set; }
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("login_id")] public string LoginId { get; set; }
        [JsonIgnore] public string UserJwt { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class ProductQnaUpdateParams
    {
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("is_secret")] public bool? IsSecret { get; set; }
        [JsonProperty("guest_password")] public string GuestPassword { get; set; }
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("login_id")] public string LoginId { get; set; }
        [JsonIgnore] public string UserJwt { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class ProductQnaDeleteParams
    {
        [JsonProperty("guest_password")] public string GuestPassword { get; set; }
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("login_id")] public string LoginId { get; set; }
        [JsonIgnore] public string UserJwt { get; set; }
        [JsonIgnore] public bool? Supervisor { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class CommerceProductQnaAnswer
    {
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("answered_at")] public string AnsweredAt { get; set; }
    }

    public class CommerceProductQna
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("product_id")] public string ProductId { get; set; }
        [JsonProperty("product_option_id")] public string ProductOptionId { get; set; }
        [JsonProperty("option_text")] public string OptionText { get; set; }
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("login_id")] public string LoginId { get; set; }
        [JsonProperty("guest_name")] public string GuestName { get; set; }
        [JsonProperty("title")] public string Title { get; set; }
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("is_secret")] public bool? IsSecret { get; set; }
        [JsonProperty("is_viewable")] public bool? IsViewable { get; set; }
        [JsonProperty("is_display")] public bool? IsDisplay { get; set; }
        [JsonProperty("answered")] public bool? Answered { get; set; }
        [JsonProperty("answer")] public CommerceProductQnaAnswer Answer { get; set; }
        [JsonProperty("created_at")] public string CreatedAt { get; set; }
        [JsonProperty("updated_at")] public string UpdatedAt { get; set; }
    }

    public class ProductQnaListResponse
    {
        [JsonProperty("list")] public List<CommerceProductQna> List { get; set; }
        [JsonProperty("count")] public int? Count { get; set; }
        [JsonProperty("page")] public int? Page { get; set; }
        [JsonProperty("limit")] public int? Limit { get; set; }
    }

    #endregion

    #region Product review

    public class ProductReviewListParams
    {
        [JsonProperty("page")] public int? Page { get; set; }
        [JsonProperty("limit")] public int? Limit { get; set; }
        [JsonProperty("product_id")] public string ProductId { get; set; }
        [JsonProperty("view")] public string View { get; set; }
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("login_id")] public string LoginId { get; set; }
        [JsonIgnore] public string UserJwt { get; set; }
        [JsonIgnore] public bool? Supervisor { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class ProductReviewDetailParams
    {
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("login_id")] public string LoginId { get; set; }
        [JsonIgnore] public string UserJwt { get; set; }
        [JsonIgnore] public bool? Supervisor { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class ProductReviewCreateParams
    {
        [JsonProperty("order_id")] public string OrderId { get; set; }
        [JsonProperty("product_id")] public string ProductId { get; set; }
        [JsonProperty("rating")] public int? Rating { get; set; }
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("product_option_id")] public string ProductOptionId { get; set; }
        [JsonProperty("images")] public List<object> Images { get; set; }
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("login_id")] public string LoginId { get; set; }
        [JsonIgnore] public string UserJwt { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class ProductReviewUpdateParams
    {
        [JsonProperty("rating")] public int? Rating { get; set; }
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("images")] public List<object> Images { get; set; }
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("login_id")] public string LoginId { get; set; }
        [JsonIgnore] public string UserJwt { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class ProductReviewDeleteParams
    {
        [JsonProperty("reason")] public string Reason { get; set; }
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("login_id")] public string LoginId { get; set; }
        [JsonIgnore] public string UserJwt { get; set; }
        [JsonIgnore] public bool? Supervisor { get; set; }
        [JsonIgnore] public string IdempotencyKey { get; set; }
    }

    public class CommerceProductReviewReply
    {
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("replied_at")] public string RepliedAt { get; set; }
    }

    public class CommerceProductReview
    {
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("order_id")] public string OrderId { get; set; }
        [JsonProperty("product_id")] public string ProductId { get; set; }
        [JsonProperty("product_option_id")] public string ProductOptionId { get; set; }
        [JsonProperty("user_id")] public string UserId { get; set; }
        [JsonProperty("login_id")] public string LoginId { get; set; }
        [JsonProperty("rating")] public int? Rating { get; set; }
        [JsonProperty("content")] public string Content { get; set; }
        [JsonProperty("images")] public List<object> Images { get; set; }
        [JsonProperty("is_display")] public bool? IsDisplay { get; set; }
        [JsonProperty("reply")] public CommerceProductReviewReply Reply { get; set; }
        [JsonProperty("created_at")] public string CreatedAt { get; set; }
        [JsonProperty("updated_at")] public string UpdatedAt { get; set; }
    }

    public class ProductReviewListResponse
    {
        [JsonProperty("list")] public List<CommerceProductReview> List { get; set; }
        [JsonProperty("count")] public int? Count { get; set; }
        [JsonProperty("page")] public int? Page { get; set; }
        [JsonProperty("limit")] public int? Limit { get; set; }
    }

    #endregion
}
