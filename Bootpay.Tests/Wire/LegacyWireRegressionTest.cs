using Bootpay.Commerce;
using Bootpay.Commerce.Models;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Bootpay.Tests.Wire
{
    /// <summary>
    /// 기존(2.3.0) API 표면의 하위호환 회귀 검증 — 2.4.0 변경 후에도
    /// 기존 메서드들이 같은 method / URL / 바디를 만드는지 mock 서버로 단정한다.
    /// (라이브 스위트의 mutating 호출을 production 에 보내지 않고 요청 수준에서 검증)
    /// </summary>
    public class CommerceLegacyWireRegressionTest : IDisposable
    {
        private readonly MockServer _server;
        private readonly WireCommerceApi _api;

        public CommerceLegacyWireRegressionTest()
        {
            _server = new MockServer();
            _api = new WireCommerceApi(_server.BaseUrl);
        }

        public void Dispose() => _server.Dispose();

        [Fact]
        public async Task GetAccessToken_PostsClientKeySecretKeyWithBasicAuth()
        {
            await _api.GetAccessToken();
            var req = _server.LastRequest;

            Assert.Equal("POST", req.Method);
            Assert.Equal("/request/token", req.PathAndQuery);
            Assert.StartsWith("Basic ", req.Headers["Authorization"]);

            var body = JObject.Parse(req.Body);
            Assert.Equal("test_client_key", (string?)body["client_key"]);
            Assert.Equal("test_secret_key", (string?)body["secret_key"]);
        }

        [Fact]
        public async Task StoredCommerceToken_DoesNotReplaceBasicAuthorization()
        {
            _server.ResponseBody = "{\"access_token\":\"stored_token\",\"expired_at\":\"2099-01-01T00:00:00Z\"}";
            await _api.GetAccessToken();

            await _api.UserList();

            var authorization = _server.LastRequest.Headers["Authorization"];
            Assert.Equal("Basic dGVzdF9jbGllbnRfa2V5OnRlc3Rfc2VjcmV0X2tleQ==", authorization);
        }

        [Theory]
        [InlineData(null, "secret_key")]
        [InlineData("client_key", null)]
        [InlineData("", "secret_key")]
        [InlineData("client_key", "")]
        public void Constructor_RejectsPartialClientCredentials(string? clientKey, string? secretKey)
        {
            Assert.Throws<ArgumentException>(() => new BootpayCommerceApi(clientKey!, secretKey!));
            Assert.Empty(_server.Requests);
        }

        [Fact]
        public async Task UserLegacyEndpoints_KeepPathsAndBodies()
        {
            await _api.UserToken("u1");
            Assert.Equal("POST", _server.LastRequest.Method);
            Assert.Equal("/users/login/token", _server.LastRequest.PathAndQuery);
            Assert.Equal("u1", (string?)JObject.Parse(_server.LastRequest.Body)["user_id"]);

            await _api.UserJoin(new CommerceUser { Email = "test@bootpay.co.kr" });
            Assert.Equal("/users/join", _server.LastRequest.PathAndQuery);

            await _api.UserCheckExist("email-exist", "test@bootpay.co.kr");
            Assert.Equal("/users/join/email-exist?pk=test%40bootpay.co.kr", _server.LastRequest.PathAndQuery);

            await _api.UserList(new UserListParams { Page = 1, Limit = 10 });
            Assert.Equal("/users?page=1&limit=10", _server.LastRequest.PathAndQuery);

            await _api.UserDetail("u1");
            Assert.Equal("/users/u1", _server.LastRequest.PathAndQuery);

            await _api.UserDelete("u1");
            Assert.Equal("DELETE", _server.LastRequest.Method);
            Assert.Equal("/users/u1", _server.LastRequest.PathAndQuery);
        }

        [Fact]
        public async Task UserGroupMembership_KeepsUserRoutes()
        {
            await _api.UserGroupUserCreate("g1", "u1");
            Assert.Equal("POST", _server.LastRequest.Method);
            Assert.Equal("/user-groups/g1/user", _server.LastRequest.PathAndQuery);
            Assert.Equal("u1", (string?)JObject.Parse(_server.LastRequest.Body)["user_id"]);

            await _api.UserGroupUserDelete("g1", "u1");
            Assert.Equal("DELETE", _server.LastRequest.Method);
            Assert.Equal("/user-groups/g1/user/u1", _server.LastRequest.PathAndQuery);
        }

        [Fact]
        public async Task ProductList_Legacy_DoesNotForceDefaults()
        {
            await _api.ProductList();
            Assert.Equal("/products", _server.LastRequest.PathAndQuery); // 기존 list 는 기본값 강제 없음

            await _api.ProductList(new ProductListParams { Page = 1, Limit = 10, Type = 2 });
            Assert.Equal("/products?page=1&limit=10&type=2", _server.LastRequest.PathAndQuery);
        }

        [Fact]
        public async Task OrderEndpoints_KeepPathsAndQueries()
        {
            await _api.OrderList(new OrderListParams { Page = 1, Limit = 10, Status = new List<int> { 1, 2 } });
            Assert.Equal("/orders?page=1&limit=10&status=1%2c2", _server.LastRequest.PathAndQuery);

            await _api.OrderDetail("o1");
            Assert.Equal("/orders/o1", _server.LastRequest.PathAndQuery);

            await _api.OrderMonth("g1", "2026-03");
            Assert.Equal("/orders/month?user_group_id=g1&search_date=2026-03", _server.LastRequest.PathAndQuery);
        }

        [Fact]
        public async Task OrderCancelRequest_KeepsBodyPassthrough()
        {
            await _api.OrderCancelRequest(new OrderCancelParams
            {
                OrderNumber = "on1",
                RequestCancelParameters = new RequestCancelParameter { CancelReason = "사유" }
            });
            var req = _server.LastRequest;

            Assert.Equal("POST", req.Method);
            Assert.Equal("/order/cancel", req.PathAndQuery);
            var body = JObject.Parse(req.Body);
            Assert.Equal("on1", (string?)body["order_number"]);
            Assert.Equal("사유", (string?)body["request_cancel_parameters"]!["cancel_reason"]);
        }

        [Fact]
        public async Task OrderSubscription_LegacyReadsAndSupervisorActions_KeepPaths()
        {
            await _api.OrderSubscriptionDetail("sub1");
            Assert.Equal("/order_subscriptions/sub1", _server.LastRequest.PathAndQuery);

            await _api.OrderSubscriptionCalculateTerminationFeeByOrderNumber("on1");
            Assert.Equal(
                "/order_subscriptions/requests/ing/calculate_termination_fee?order_number=on1",
                _server.LastRequest.PathAndQuery);

            await _api.OrderSubscriptionSupervisorPause("sub1", new SupervisorOrderSubscriptionPauseParams { Reason = "정지" });
            Assert.Equal("PUT", _server.LastRequest.Method);
            Assert.Equal("/order_subscriptions/sub1/pause", _server.LastRequest.PathAndQuery);

            await _api.OrderSubscriptionPause(new OrderSubscriptionPauseParams { OrderNumber = "on1" });
            Assert.Equal("POST", _server.LastRequest.Method);
            Assert.Equal("/order_subscriptions/requests/ing/pause", _server.LastRequest.PathAndQuery);

            await _api.OrderSubscriptionResume(new OrderSubscriptionResumeParams { OrderNumber = "on1" });
            Assert.Equal("PUT", _server.LastRequest.Method); // requests/ing 중 유일한 PUT
            Assert.Equal("/order_subscriptions/requests/ing/resume", _server.LastRequest.PathAndQuery);
        }

        [Fact]
        public async Task V1Modules_KeepPaths()
        {
            await _api.CategoryList();
            Assert.Equal("/categories", _server.LastRequest.PathAndQuery);

            await _api.CouponList();
            Assert.Equal("/coupon", _server.LastRequest.PathAndQuery);

            await _api.PointBalance();
            Assert.Equal("/point/balance", _server.LastRequest.PathAndQuery);

            await _api.CartOrderPreview();
            Assert.Equal("POST", _server.LastRequest.Method);
            Assert.Equal("/cart/order-preview", _server.LastRequest.PathAndQuery);
        }

        [Fact]
        public async Task InvoiceCreate_KeepsPlainPost()
        {
            await _api.InvoiceCreate(new CommerceInvoice());
            Assert.Equal("POST", _server.LastRequest.Method);
            Assert.Equal("/invoices", _server.LastRequest.PathAndQuery);
        }
    }

    /// <summary>
    /// PG API 기존 표면 회귀 검증 — legacy application_id/private_key 흐름.
    /// </summary>
    public class PgLegacyWireRegressionTest : IDisposable
    {
        private readonly MockServer _server;
        private readonly WireBootpayApi _api;

        public PgLegacyWireRegressionTest()
        {
            _server = new MockServer();
            _api = new WireBootpayApi(_server.BaseUrl);
        }

        public void Dispose() => _server.Dispose();

        [Fact]
        public async Task GetAccessToken_Legacy_PostsApplicationIdAndPrivateKey()
        {
            _server.ResponseBody = "{\"access_token\":\"legacy_token\",\"expire_in\":1800}";
            await _api.GetAccessToken();
            var req = _server.LastRequest;

            Assert.Equal("POST", req.Method);
            Assert.Equal("/request/token", req.PathAndQuery);

            var body = JObject.Parse(req.Body);
            Assert.Equal("test_application_id", (string?)body["application_id"]);
            Assert.Equal("test_private_key", (string?)body["private_key"]);

            await _api.GetReceipt("r1");
            Assert.Equal("Bearer legacy_token", _server.LastRequest.Headers["Authorization"]);
        }

        [Fact]
        public async Task ClientCredentials_UseBasicAuthorizationWithoutTokenRequest()
        {
            var api = new WireClientKeyBootpayObject(_server.BaseUrl);

            var tokenResponse = await api.GetAccessToken();
            await api.SendAsync("receipt/r1", HttpMethod.Get);

            Assert.True(tokenResponse.IsSuccessStatusCode);
            Assert.Single(_server.Requests);
            Assert.Equal("/receipt/r1", _server.LastRequest.PathAndQuery);
            Assert.Equal("Basic dGVzdF9jbGllbnRfa2V5OnRlc3Rfc2VjcmV0X2tleQ==", _server.LastRequest.Headers["Authorization"]);
        }

        [Theory]
        [InlineData(null, "private_key")]
        [InlineData("application_id", null)]
        [InlineData("", "private_key")]
        [InlineData("application_id", "")]
        public void LegacyConstructor_RejectsPartialCredentials(string? applicationId, string? privateKey)
        {
            Assert.Throws<ArgumentException>(() => new BootpayApi(applicationId!, privateKey!));
            Assert.Empty(_server.Requests);
        }

        [Theory]
        [InlineData(null, "secret_key")]
        [InlineData("client_key", null)]
        [InlineData("", "secret_key")]
        [InlineData("client_key", "")]
        public void ClientKeyFactory_RejectsPartialCredentials(string? clientKey, string? secretKey)
        {
            Assert.Throws<ArgumentException>(() => BootpayApi.WithClientKey(clientKey!, secretKey!));
            Assert.Empty(_server.Requests);
        }

        [Fact]
        public async Task PaymentAndBillingEndpoints_KeepPaths()
        {
            _server.ResponseBody = "{\"access_token\":\"legacy_token\",\"expire_in\":1800}";
            await _api.GetAccessToken();
            _server.ResponseBody = "{\"success\":true,\"data\":{}}";

            await _api.GetReceipt("r1");
            Assert.Equal("GET", _server.LastRequest.Method);
            Assert.Equal("/receipt/r1", _server.LastRequest.PathAndQuery);

            await _api.Confirm("r1");
            Assert.Equal("POST", _server.LastRequest.Method);
            Assert.Equal("/confirm", _server.LastRequest.PathAndQuery);

            await _api.LookupBillingKey("r1");
            Assert.Equal("/subscribe/billing_key/r1", _server.LastRequest.PathAndQuery);

            await _api.LookupBillingKeyByKey("bk1");
            Assert.Equal("/billing_key/bk1", _server.LastRequest.PathAndQuery);

            await _api.DestroyBillingKey("bk1");
            Assert.Equal("DELETE", _server.LastRequest.Method);
            Assert.Equal("/subscribe/billing_key/bk1", _server.LastRequest.PathAndQuery);
        }
    }
}
