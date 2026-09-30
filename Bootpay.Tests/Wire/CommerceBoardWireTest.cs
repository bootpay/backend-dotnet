using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bootpay.Commerce.Models;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Bootpay.Tests.Wire
{
    public class CommerceBoardWireTest : IDisposable
    {
        private readonly MockServer _server = new MockServer();
        private readonly WireCommerceApi _api;

        public CommerceBoardWireTest()
        {
            _api = new WireCommerceApi(_server.BaseUrl);
            _api.AsManager();
        }

        public void Dispose() => _server.Dispose();

        private async Task Check(Func<Task> call, string method, string path, string role, JObject? body = null, string? jwt = null)
        {
            await call();
            var request = _server.LastRequest;
            Assert.Equal(method, request.Method);
            var expectedUri = new Uri("http://localhost" + path);
            var actualUri = new Uri("http://localhost" + request.PathAndQuery);
            Assert.Equal(expectedUri.AbsolutePath, actualUri.AbsolutePath);
            var expectedQuery = System.Web.HttpUtility.ParseQueryString(expectedUri.Query);
            var actualQuery = System.Web.HttpUtility.ParseQueryString(actualUri.Query);
            Assert.Equal(expectedQuery.Count, actualQuery.Count);
            foreach (var key in expectedQuery.AllKeys) Assert.Equal(expectedQuery[key], actualQuery[key]);
            Assert.Equal(role, request.Headers["BOOTPAY-ROLE"]);
            Assert.True(Guid.TryParse(request.Headers["Idempotency-Key"], out _));
            Assert.Equal(jwt != null, request.Headers.ContainsKey("Bootpay-User-JWT"));
            if (jwt != null) Assert.Equal(jwt, request.Headers["Bootpay-User-JWT"]);
            Assert.DoesNotContain("supervisor=", path);
            Assert.DoesNotContain("user_jwt=", path);
            Assert.DoesNotContain("idempotency_key=", path);
            if (body == null) Assert.True(string.IsNullOrEmpty(request.Body));
            else
            {
                var actual = JObject.Parse(request.Body);
                Assert.True(JToken.DeepEquals(body, actual), $"Expected {body}, got {actual}");
                Assert.False(actual.ContainsKey("supervisor"));
                Assert.False(actual.ContainsKey("user_jwt"));
                Assert.False(actual.ContainsKey("idempotency_key"));
            }
        }

        [Fact]
        public async Task Notice_FiveRoutes()
        {
            await Check(() => _api.NoticeList(), "GET", "/notices?page=1&limit=20", "user");
            await Check(() => _api.NoticeList(new NoticeListParams { View = "all", Supervisor = true }), "GET", "/notices?view=all&page=1&limit=20", "supervisor");
            await Check(() => _api.NoticeDetail("n1", new NoticeDetailParams { Supervisor = true }), "GET", "/notices/n1", "supervisor");
            await Check(() => _api.NoticeCreate(new NoticeCreateParams { Title = "공지", Content = "내용", IsDisplay = false }), "POST", "/notices", "supervisor", JObject.Parse("{ 'title':'공지', 'content':'내용', 'is_display':false }"));
            await Check(() => _api.NoticeUpdate("n1", new NoticeUpdateParams { Images = new List<object>(), IsNotice = false }), "PUT", "/notices/n1", "supervisor", JObject.Parse("{ 'images':[], 'is_notice':false }"));
            await Check(() => _api.NoticeDelete("n1"), "DELETE", "/notices/n1", "supervisor");
        }

        [Fact]
        public async Task Faq_FiveRoutes()
        {
            await Check(() => _api.FaqList(), "GET", "/faqs?page=1&limit=20", "user");
            await Check(() => _api.FaqList(new FaqListParams { View = "all", Supervisor = true }), "GET", "/faqs?view=all&page=1&limit=20", "supervisor");
            await Check(() => _api.FaqDetail("f1"), "GET", "/faqs/f1", "user");
            await Check(() => _api.FaqCreate(new FaqCreateParams { Title = "FAQ", Content = "답", IsDisplay = false }), "POST", "/faqs", "supervisor", JObject.Parse("{ 'title':'FAQ', 'content':'답', 'is_display':false }"));
            await Check(() => _api.FaqUpdate("f1", new FaqUpdateParams { Images = new List<object>() }), "PUT", "/faqs/f1", "supervisor", JObject.Parse("{ 'images':[] }"));
            await Check(() => _api.FaqDelete("f1"), "DELETE", "/faqs/f1", "supervisor");
        }

        [Fact]
        public async Task Inquiry_SixRoutes()
        {
            await Check(() => _api.InquiryList(new InquiryListParams { Answered = false, UserJwt = "jwt" }), "GET", "/inquiries?answered=false&page=1&limit=20", "user", jwt: "jwt");
            await Check(() => _api.InquiryDetail("i1", new InquiryDetailParams { UserId = "u1", Supervisor = true, UserJwt = "jwt" }), "GET", "/inquiries/i1?user_id=u1", "supervisor", jwt: "jwt");
            await Check(() => _api.InquiryCreate(new InquiryCreateParams { Content = "문의", UserId = "u1", UserJwt = "jwt" }), "POST", "/inquiries", "user", JObject.Parse("{ 'content':'문의', 'user_id':'u1' }"), "jwt");
            await Check(() => _api.InquiryUpdate("i1", new InquiryUpdateParams { Title = "", UserJwt = "jwt" }), "PUT", "/inquiries/i1", "user", JObject.Parse("{ 'title':'' }"), "jwt");
            await Check(() => _api.InquiryDelete("i1", new InquiryDeleteParams { LoginId = "login", Supervisor = true }), "DELETE", "/inquiries/i1?login_id=login", "supervisor");
            await Check(() => _api.InquiryAnswer("i1", "답변"), "PUT", "/inquiries/i1/answer", "supervisor", JObject.Parse("{ 'content':'답변' }"));
        }

        [Fact]
        public async Task ProductQna_SixRoutes()
        {
            await Check(() => _api.ProductQnaList(new ProductQnaListParams { ProductId = "p1" }), "GET", "/product-qnas?product_id=p1&page=1&limit=20", "user");
            await Check(() => _api.ProductQnaDetail("q1", new ProductQnaDetailParams { LoginId = "login", UserJwt = "jwt" }), "GET", "/product-qnas/q1?login_id=login", "user", jwt: "jwt");
            await Check(() => _api.ProductQnaCreate(new ProductQnaCreateParams { ProductId = "p1", Content = "문의", IsSecret = false }), "POST", "/product-qnas", "user", JObject.Parse("{ 'product_id':'p1', 'content':'문의', 'is_secret':false }"));
            await Check(() => _api.ProductQnaUpdate("q1", new ProductQnaUpdateParams { IsSecret = false }), "PUT", "/product-qnas/q1", "user", JObject.Parse("{ 'is_secret':false }"));
            await Check(() => _api.ProductQnaDelete("q1", new ProductQnaDeleteParams { GuestPassword = "1234", UserId = "u1", Supervisor = true }), "DELETE", "/product-qnas/q1", "supervisor", JObject.Parse("{ 'guest_password':'1234', 'user_id':'u1' }"));
            await Check(() => _api.ProductQnaDelete("q1"), "DELETE", "/product-qnas/q1", "user", JObject.Parse("{}"));
            await Check(() => _api.ProductQnaAnswer("q1", "답변"), "PUT", "/product-qnas/q1/answer", "supervisor", JObject.Parse("{ 'content':'답변' }"));
        }

        [Fact]
        public async Task ProductReview_SixRoutes()
        {
            await Check(() => _api.ProductReviewList(new ProductReviewListParams { ProductId = "p1", UserJwt = "jwt" }), "GET", "/reviews?product_id=p1&page=1&limit=20", "user", jwt: "jwt");
            await Check(() => _api.ProductReviewDetail("r1", new ProductReviewDetailParams { UserId = "u1", Supervisor = true }), "GET", "/reviews/r1?user_id=u1", "supervisor");
            await Check(() => _api.ProductReviewCreate(new ProductReviewCreateParams { OrderId = "o1", ProductId = "p1", Rating = 5, Content = "좋음" }), "POST", "/reviews", "user", JObject.Parse("{ 'order_id':'o1', 'product_id':'p1', 'rating':5, 'content':'좋음' }"));
            await Check(() => _api.ProductReviewUpdate("r1", new ProductReviewUpdateParams { Images = new List<object>() }), "PUT", "/reviews/r1", "user", JObject.Parse("{ 'images':[] }"));
            await Check(() => _api.ProductReviewDelete("r1", new ProductReviewDeleteParams { Reason = "운영", Supervisor = true }), "DELETE", "/reviews/r1?reason=%EC%9A%B4%EC%98%81", "supervisor");
            await Check(() => _api.ProductReviewReply("r1", "감사합니다"), "PUT", "/reviews/r1/reply", "supervisor", JObject.Parse("{ 'content':'감사합니다' }"));
        }

        [Fact]
        public async Task ExplicitAndGeneratedIdempotencyKeys()
        {
            await _api.NoticeList(new NoticeListParams { IdempotencyKey = "caller-key" });
            Assert.Equal("caller-key", _server.LastRequest.Headers["Idempotency-Key"]);
            await _api.NoticeList();
            var first = _server.LastRequest.Headers["Idempotency-Key"];
            await _api.NoticeList();
            Assert.NotEqual(first, _server.LastRequest.Headers["Idempotency-Key"]);
            Assert.Equal("manager", _api.GetCurrentRole());
        }

        [Fact]
        public async Task BoardsUseCommerceHostAndPreserveEmptyQueryValues()
        {
            using var commerce = new MockServer();
            using var message = new MockServer();
            var api = new WireCommerceApi(commerce.BaseUrl, message.BaseUrl);

            await api.NoticeList(new NoticeListParams { Keyword = "" });

            Assert.Single(commerce.Requests);
            Assert.Empty(message.Requests);
            Assert.Contains("keyword=", commerce.LastRequest.PathAndQuery);
        }
    }
}
