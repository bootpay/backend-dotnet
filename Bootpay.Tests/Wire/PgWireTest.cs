using Newtonsoft.Json.Linq;
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
            _server.ResponseBody = "{\"access_token\":\"legacy_token\",\"expire_in\":1800}";
            await _api.GetAccessToken();
            _server.ResponseBody = "{\"success\":true,\"data\":{}}";

            await _api.LookupSequentialBillingKey("widget/key 1", "bk_123", "user id@1");
            var req = _server.LastRequest;

            Assert.Equal("GET", req.Method);
            Assert.Equal(
                "/subscribe/sequential_billing_key/bk_123?widget_key=widget%2Fkey%201&user_id=user%20id%401",
                req.PathAndQuery);
        }

        /// <summary>
        /// 별건 현금영수증 발행 (POST request/cash/receipt) — pg 는 선택값이다.
        /// 서버는 pg 가 없으면 가맹점에 설정된 기본 PG사로 발행하므로,
        /// SDK 가 임의의 기본 PG명을 채워 넣거나 필수로 막아서는 안 된다. (ruby SDK c716a1f parity)
        /// </summary>
        [Fact]
        public async Task RequestCashReceipt_OmitsPgWhenUnset_AndForwardsItWhenGiven()
        {
            _server.ResponseBody = "{\"access_token\":\"legacy_token\",\"expire_in\":1800}";
            await _api.GetAccessToken();
            _server.ResponseBody = "{\"success\":true,\"data\":{}}";

            await _api.RequestCashReceipt(new Bootpay.models.CashReceipt
            {
                orderId = "order-1",
                orderName = "테스트 상품",
                identityNo = "0101234",
                cashReceiptType = "소득공제",
                price = 1000
            });

            var body = JObject.Parse(_server.LastRequest.Body);
            Assert.Equal("POST", _server.LastRequest.Method);
            Assert.Equal("/request/cash/receipt", _server.LastRequest.PathAndQuery);
            // pg 를 지정하지 않으면 키 자체가 빠져야 서버가 기본 PG 로 발행한다
            Assert.False(body.ContainsKey("pg"));
            Assert.Equal("order-1", (string?)body["order_id"]);

            await _api.RequestCashReceipt(new Bootpay.models.CashReceipt
            {
                orderId = "order-2",
                orderName = "테스트 상품",
                identityNo = "0101234",
                cashReceiptType = "소득공제",
                price = 1000,
                pg = "kcp"
            });

            // 지정하면 그대로 전달된다 (기존 호출 동작 불변)
            Assert.Equal("kcp", (string?)JObject.Parse(_server.LastRequest.Body)["pg"]);
        }
    }
}
