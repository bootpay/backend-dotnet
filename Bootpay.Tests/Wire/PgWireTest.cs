using Xunit;

namespace Bootpay.Tests.Wire
{
    /// <summary>
    /// PG API wire-format 검증 (NodeJS 2.9.0 parity).
    /// </summary>
    public class PgWireTest : IDisposable
    {
        private readonly MockServer _server;
        private readonly WireBootpayApi _api;

        public PgWireTest()
        {
            _server = new MockServer();
            _api = new WireBootpayApi(_server.BaseUrl);
        }

        public void Dispose() => _server.Dispose();

        [Fact]
        public async Task LookupSequentialBillingKey_SendsWidgetKeyAndUserIdEscaped()
        {
            await _api.LookupSequentialBillingKey("widget/key 1", "bk_123", "user id@1");
            var req = _server.LastRequest;

            Assert.Equal("GET", req.Method);
            Assert.Equal(
                "/subscribe/sequential_billing_key/bk_123?widget_key=widget%2Fkey%201&user_id=user%20id%401",
                req.PathAndQuery);
        }
    }
}
