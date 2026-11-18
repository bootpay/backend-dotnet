using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bootpay.Commerce.Models;

namespace Bootpay.Commerce.Service
{
    internal static class BoardServiceSupport
    {
        internal static string Query(string path, NameValueCollection query)
        {
            var value = query.ToString();
            return string.IsNullOrEmpty(value) ? path : $"{path}?{value}";
        }

        internal static NameValueCollection Paging(int? page, int? limit)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            query["page"] = (page ?? 1).ToString();
            query["limit"] = (limit ?? 20).ToString();
            return query;
        }

        internal static System.Collections.Generic.Dictionary<string, string> Headers(bool? supervisor, string userJwt, string idempotencyKey)
        {
            var headers = CommerceRequestHeaders.WithRole(supervisor == true ? "supervisor" : "user", idempotencyKey);
            if (!string.IsNullOrEmpty(userJwt)) headers["Bootpay-User-JWT"] = userJwt;
            return headers;
        }

        internal static object Content(string content) => new { content };
    }

    public class NoticeService
    {
        public static Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, NoticeListParams listParams = null)
        {
            var query = BoardServiceSupport.Paging(listParams?.Page, listParams?.Limit);
            if (listParams?.Keyword != null) query["keyword"] = listParams.Keyword;
            if (listParams?.View != null) query["view"] = listParams.View;
            return bootpay.SendAsync(BoardServiceSupport.Query("notices", query), HttpMethod.Get, null,
                BoardServiceSupport.Headers(listParams?.Supervisor, null, listParams?.IdempotencyKey));
        }

        public static Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string noticeId, NoticeDetailParams detailParams = null) =>
            bootpay.SendAsync($"notices/{noticeId}", HttpMethod.Get, null,
                BoardServiceSupport.Headers(detailParams?.Supervisor, null, detailParams?.IdempotencyKey));

        public static Task<HttpResponseMessage> Create(BootpayCommerceObject bootpay, NoticeCreateParams createParams) =>
            bootpay.SendAsync("notices", HttpMethod.Post, createParams, CommerceRequestHeaders.Supervisor(createParams?.IdempotencyKey));

        public static Task<HttpResponseMessage> Update(BootpayCommerceObject bootpay, string noticeId, NoticeUpdateParams updateParams) =>
            bootpay.SendAsync($"notices/{noticeId}", HttpMethod.Put, updateParams, CommerceRequestHeaders.Supervisor(updateParams?.IdempotencyKey));

        public static Task<HttpResponseMessage> Delete(BootpayCommerceObject bootpay, string noticeId, string idempotencyKey = null) =>
            bootpay.SendAsync($"notices/{noticeId}", HttpMethod.Delete, null, CommerceRequestHeaders.Supervisor(idempotencyKey));
    }

    public class FaqService
    {
        public static Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, FaqListParams listParams = null)
        {
            var query = BoardServiceSupport.Paging(listParams?.Page, listParams?.Limit);
            if (listParams?.Keyword != null) query["keyword"] = listParams.Keyword;
            if (listParams?.View != null) query["view"] = listParams.View;
            return bootpay.SendAsync(BoardServiceSupport.Query("faqs", query), HttpMethod.Get, null,
                BoardServiceSupport.Headers(listParams?.Supervisor, null, listParams?.IdempotencyKey));
        }

        public static Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string faqId, FaqDetailParams detailParams = null) =>
            bootpay.SendAsync($"faqs/{faqId}", HttpMethod.Get, null,
                BoardServiceSupport.Headers(detailParams?.Supervisor, null, detailParams?.IdempotencyKey));

        public static Task<HttpResponseMessage> Create(BootpayCommerceObject bootpay, FaqCreateParams createParams) =>
            bootpay.SendAsync("faqs", HttpMethod.Post, createParams, CommerceRequestHeaders.Supervisor(createParams?.IdempotencyKey));

        public static Task<HttpResponseMessage> Update(BootpayCommerceObject bootpay, string faqId, FaqUpdateParams updateParams) =>
            bootpay.SendAsync($"faqs/{faqId}", HttpMethod.Put, updateParams, CommerceRequestHeaders.Supervisor(updateParams?.IdempotencyKey));

        public static Task<HttpResponseMessage> Delete(BootpayCommerceObject bootpay, string faqId, string idempotencyKey = null) =>
            bootpay.SendAsync($"faqs/{faqId}", HttpMethod.Delete, null, CommerceRequestHeaders.Supervisor(idempotencyKey));
    }

    public class InquiryService
    {
        public static Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, InquiryListParams listParams = null)
        {
            var query = BoardServiceSupport.Paging(listParams?.Page, listParams?.Limit);
            if (listParams?.UserId != null) query["user_id"] = listParams.UserId;
            if (listParams?.LoginId != null) query["login_id"] = listParams.LoginId;
            if (listParams?.Answered != null) query["answered"] = listParams.Answered.Value ? "true" : "false";
            return bootpay.SendAsync(BoardServiceSupport.Query("inquiries", query), HttpMethod.Get, null,
                BoardServiceSupport.Headers(listParams?.Supervisor, listParams?.UserJwt, listParams?.IdempotencyKey));
        }

        public static Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string inquiryId, InquiryDetailParams detailParams = null)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (detailParams?.UserId != null) query["user_id"] = detailParams.UserId;
            if (detailParams?.LoginId != null) query["login_id"] = detailParams.LoginId;
            return bootpay.SendAsync(BoardServiceSupport.Query($"inquiries/{inquiryId}", query), HttpMethod.Get, null,
                BoardServiceSupport.Headers(detailParams?.Supervisor, detailParams?.UserJwt, detailParams?.IdempotencyKey));
        }

        public static Task<HttpResponseMessage> Create(BootpayCommerceObject bootpay, InquiryCreateParams createParams) =>
            bootpay.SendAsync("inquiries", HttpMethod.Post, createParams,
                BoardServiceSupport.Headers(false, createParams?.UserJwt, createParams?.IdempotencyKey));

        public static Task<HttpResponseMessage> Update(BootpayCommerceObject bootpay, string inquiryId, InquiryUpdateParams updateParams) =>
            bootpay.SendAsync($"inquiries/{inquiryId}", HttpMethod.Put, updateParams,
                BoardServiceSupport.Headers(false, updateParams?.UserJwt, updateParams?.IdempotencyKey));

        public static Task<HttpResponseMessage> Delete(BootpayCommerceObject bootpay, string inquiryId, InquiryDeleteParams deleteParams = null)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (deleteParams?.UserId != null) query["user_id"] = deleteParams.UserId;
            if (deleteParams?.LoginId != null) query["login_id"] = deleteParams.LoginId;
            return bootpay.SendAsync(BoardServiceSupport.Query($"inquiries/{inquiryId}", query), HttpMethod.Delete, null,
                BoardServiceSupport.Headers(deleteParams?.Supervisor, deleteParams?.UserJwt, deleteParams?.IdempotencyKey));
        }

        public static Task<HttpResponseMessage> Answer(BootpayCommerceObject bootpay, string inquiryId, string content, string idempotencyKey = null) =>
            bootpay.SendAsync($"inquiries/{inquiryId}/answer", HttpMethod.Put, BoardServiceSupport.Content(content), CommerceRequestHeaders.Supervisor(idempotencyKey));
    }

    public class ProductQnaService
    {
        public static Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, ProductQnaListParams listParams = null)
        {
            var query = BoardServiceSupport.Paging(listParams?.Page, listParams?.Limit);
            if (listParams?.ProductId != null) query["product_id"] = listParams.ProductId;
            if (listParams?.View != null) query["view"] = listParams.View;
            if (listParams?.UserId != null) query["user_id"] = listParams.UserId;
            if (listParams?.LoginId != null) query["login_id"] = listParams.LoginId;
            return bootpay.SendAsync(BoardServiceSupport.Query("product-qnas", query), HttpMethod.Get, null,
                BoardServiceSupport.Headers(listParams?.Supervisor, listParams?.UserJwt, listParams?.IdempotencyKey));
        }

        public static Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string productQnaId, ProductQnaDetailParams detailParams = null)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (detailParams?.UserId != null) query["user_id"] = detailParams.UserId;
            if (detailParams?.LoginId != null) query["login_id"] = detailParams.LoginId;
            return bootpay.SendAsync(BoardServiceSupport.Query($"product-qnas/{productQnaId}", query), HttpMethod.Get, null,
                BoardServiceSupport.Headers(detailParams?.Supervisor, detailParams?.UserJwt, detailParams?.IdempotencyKey));
        }

        public static Task<HttpResponseMessage> Create(BootpayCommerceObject bootpay, ProductQnaCreateParams createParams) =>
            bootpay.SendAsync("product-qnas", HttpMethod.Post, createParams,
                BoardServiceSupport.Headers(false, createParams?.UserJwt, createParams?.IdempotencyKey));

        public static Task<HttpResponseMessage> Update(BootpayCommerceObject bootpay, string productQnaId, ProductQnaUpdateParams updateParams) =>
            bootpay.SendAsync($"product-qnas/{productQnaId}", HttpMethod.Put, updateParams,
                BoardServiceSupport.Headers(false, updateParams?.UserJwt, updateParams?.IdempotencyKey));

        public static Task<HttpResponseMessage> Delete(BootpayCommerceObject bootpay, string productQnaId, ProductQnaDeleteParams deleteParams = null) =>
            bootpay.SendAsync($"product-qnas/{productQnaId}", HttpMethod.Delete, deleteParams ?? new ProductQnaDeleteParams(),
                BoardServiceSupport.Headers(deleteParams?.Supervisor, deleteParams?.UserJwt, deleteParams?.IdempotencyKey));

        public static Task<HttpResponseMessage> Answer(BootpayCommerceObject bootpay, string productQnaId, string content, string idempotencyKey = null) =>
            bootpay.SendAsync($"product-qnas/{productQnaId}/answer", HttpMethod.Put, BoardServiceSupport.Content(content), CommerceRequestHeaders.Supervisor(idempotencyKey));
    }

    public class ProductReviewService
    {
        public static Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, ProductReviewListParams listParams = null)
        {
            var query = BoardServiceSupport.Paging(listParams?.Page, listParams?.Limit);
            if (listParams?.ProductId != null) query["product_id"] = listParams.ProductId;
            if (listParams?.View != null) query["view"] = listParams.View;
            if (listParams?.UserId != null) query["user_id"] = listParams.UserId;
            if (listParams?.LoginId != null) query["login_id"] = listParams.LoginId;
            return bootpay.SendAsync(BoardServiceSupport.Query("reviews", query), HttpMethod.Get, null,
                BoardServiceSupport.Headers(listParams?.Supervisor, listParams?.UserJwt, listParams?.IdempotencyKey));
        }

        public static Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string productReviewId, ProductReviewDetailParams detailParams = null)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (detailParams?.UserId != null) query["user_id"] = detailParams.UserId;
            if (detailParams?.LoginId != null) query["login_id"] = detailParams.LoginId;
            return bootpay.SendAsync(BoardServiceSupport.Query($"reviews/{productReviewId}", query), HttpMethod.Get, null,
                BoardServiceSupport.Headers(detailParams?.Supervisor, detailParams?.UserJwt, detailParams?.IdempotencyKey));
        }

        public static Task<HttpResponseMessage> Create(BootpayCommerceObject bootpay, ProductReviewCreateParams createParams) =>
            bootpay.SendAsync("reviews", HttpMethod.Post, createParams,
                BoardServiceSupport.Headers(false, createParams?.UserJwt, createParams?.IdempotencyKey));

        public static Task<HttpResponseMessage> Update(BootpayCommerceObject bootpay, string productReviewId, ProductReviewUpdateParams updateParams) =>
            bootpay.SendAsync($"reviews/{productReviewId}", HttpMethod.Put, updateParams,
                BoardServiceSupport.Headers(false, updateParams?.UserJwt, updateParams?.IdempotencyKey));

        public static Task<HttpResponseMessage> Delete(BootpayCommerceObject bootpay, string productReviewId, ProductReviewDeleteParams deleteParams = null)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (deleteParams?.Reason != null) query["reason"] = deleteParams.Reason;
            if (deleteParams?.UserId != null) query["user_id"] = deleteParams.UserId;
            if (deleteParams?.LoginId != null) query["login_id"] = deleteParams.LoginId;
            return bootpay.SendAsync(BoardServiceSupport.Query($"reviews/{productReviewId}", query), HttpMethod.Delete, null,
                BoardServiceSupport.Headers(deleteParams?.Supervisor, deleteParams?.UserJwt, deleteParams?.IdempotencyKey));
        }

        public static Task<HttpResponseMessage> Reply(BootpayCommerceObject bootpay, string productReviewId, string content, string idempotencyKey = null) =>
            bootpay.SendAsync($"reviews/{productReviewId}/reply", HttpMethod.Put, BoardServiceSupport.Content(content), CommerceRequestHeaders.Supervisor(idempotencyKey));
    }
}
