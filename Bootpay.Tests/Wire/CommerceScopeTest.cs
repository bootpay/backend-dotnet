using Bootpay.Commerce.Models;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Bootpay.Tests.Wire
{
    /// <summary>
    /// commerce-api 가 supervisor / manager scope 를 요구하는 엔드포인트의 BOOTPAY-ROLE 검증.
    /// 헤더를 붙이지 않으면 인스턴스 기본값 user 로 조용히 나가고 서버가 scope_invalid! 로 거절한다.
    /// </summary>
    public class CommerceScopeTest : IDisposable
    {
        private readonly MockServer _server;
        private readonly WireCommerceApi _api;

        public CommerceScopeTest()
        {
            _server = new MockServer();
            _api = new WireCommerceApi(_server.BaseUrl);
        }

        public void Dispose() => _server.Dispose();

        private void AssertScope(string method, string pathAndQuery, string role)
        {
            var req = _server.LastRequest;
            Assert.Equal(method, req.Method);
            Assert.Equal(pathAndQuery, req.PathAndQuery);
            Assert.Equal(role, req.Headers["BOOTPAY-ROLE"]);
            Assert.False(string.IsNullOrEmpty(req.Headers["Idempotency-Key"]));
        }

        [Fact]
        public async Task SupervisorSubscriptionActions_SendSupervisorRole()
        {
            await _api.OrderSubscriptionSupervisorApprove("s1", new SupervisorOrderSubscriptionApproveParams { Reason = "승인" });
            AssertScope("PUT", "/order_subscriptions/s1/approve", "supervisor");

            await _api.OrderSubscriptionSupervisorReject("s1", new SupervisorOrderSubscriptionRejectParams { Reason = "반려" });
            AssertScope("PUT", "/order_subscriptions/s1/reject", "supervisor");

            await _api.OrderSubscriptionSupervisorTerminate("s1", new SupervisorOrderSubscriptionTerminateParams { Reason = "해지" });
            AssertScope("PUT", "/order_subscriptions/s1/terminate", "supervisor");

            await _api.OrderSubscriptionSupervisorPause("s1", new SupervisorOrderSubscriptionPauseParams { PausedAt = "2026-01-01" });
            AssertScope("PUT", "/order_subscriptions/s1/pause", "supervisor");

            await _api.OrderSubscriptionSupervisorResume("s1");
            AssertScope("PUT", "/order_subscriptions/s1/resume", "supervisor");
        }

        [Fact]
        public async Task CategoryWrites_SendSupervisorRole()
        {
            await _api.CategoryCreate(new CategoryCreateParams { Name = "카테고리" });
            AssertScope("POST", "/categories", "supervisor");
            Assert.Equal("카테고리", (string?)JObject.Parse(_server.LastRequest.Body)["name"]);

            await _api.CategoryUpdate(new CategoryUpdateParams { CategoryId = "c1", Name = "변경" });
            AssertScope("PUT", "/categories/c1", "supervisor");

            await _api.CategoryDestroy("c1");
            AssertScope("DELETE", "/categories/c1", "supervisor");
        }

        [Fact]
        public async Task UserGroupMembership_SendsManagerRole()
        {
            await _api.UserGroupUserCreate("g1", "u1");
            AssertScope("POST", "/user-groups/g1/user", "manager");
            Assert.Equal("u1", (string?)JObject.Parse(_server.LastRequest.Body)["user_id"]);

            await _api.UserGroupUserDelete("g1", "u1");
            AssertScope("DELETE", "/user-groups/g1/user/u1", "manager");
        }

        [Fact]
        public async Task ExplicitIdempotencyKey_IsForwarded()
        {
            await _api.CategoryCreate(new CategoryCreateParams { Name = "카테고리" }, "fixed-key");
            Assert.Equal("fixed-key", _server.LastRequest.Headers["Idempotency-Key"]);

            await _api.UserGroupUserCreate("g1", "u1", "member-key");
            Assert.Equal("member-key", _server.LastRequest.Headers["Idempotency-Key"]);

            await _api.OrderSubscriptionSupervisorApprove("s1", null, "approve-key");
            Assert.Equal("approve-key", _server.LastRequest.Headers["Idempotency-Key"]);
        }
    }
}
