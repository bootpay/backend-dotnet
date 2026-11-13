using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bootpay.Commerce.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Bootpay.Tests.Wire
{
    public class CommerceDepositWireTest : IDisposable
    {
        private readonly MockServer _server = new MockServer();
        private readonly WireCommerceApi _api;

        public CommerceDepositWireTest()
        {
            _api = new WireCommerceApi(_server.BaseUrl + "v1/");
            _api.AsManager();
        }

        public void Dispose() => _server.Dispose();

        private async Task Check(Func<Task> call, string method, string path, JObject? body = null, string? key = null)
        {
            await call();
            var request = _server.LastRequest;
            Assert.Equal(method, request.Method);
            Assert.Equal("/v1/" + path, request.PathAndQuery);
            Assert.Equal("supervisor", request.Headers["BOOTPAY-ROLE"]);
            Assert.Equal("manager", _api.GetCurrentRole());
            // 릴리스마다 고치지 않도록 SDK 의 버전 상수와 비교한다
            var sdkVersion = (string)typeof(Bootpay.Commerce.BootpayCommerceObject)
                .GetField("SDK_VERSION", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                .GetValue(null);
            Assert.Equal(sdkVersion, request.Headers["BOOTPAY-SDK-VERSION"]);
            if (key == null) Assert.True(Guid.TryParse(request.Headers["Idempotency-Key"], out _));
            else Assert.Equal(key, request.Headers["Idempotency-Key"]);
            if (body == null) Assert.True(string.IsNullOrEmpty(request.Body));
            else Assert.True(JToken.DeepEquals(body, JObject.Parse(request.Body)), request.Body);
        }

        [Fact]
        public async Task Deposit_FiveRoutes_ForceSupervisorAndPreserveRole()
        {
            await Check(() => _api.DepositAccounts(), "GET", "sdk/deposits/accounts");
            await Check(() => _api.DepositWebhookSecret(), "POST", "sdk/deposits/webhook_secret", new JObject());
            await Check(() => _api.DepositCreate(new DepositCreateParams
            {
                MerchantOrderId = "m-1", Price = 15000, Depositor = "홍길동", AccountId = "acc1",
                BuyerEmail = null,
                Notify = new DepositNotifyParams { DepositGuide = true, DepositPaid = false, Link = "https://example.com/o/m-1" },
                IdempotencyKey = "idem-deposit"
            }), "POST", "sdk/deposits", JObject.Parse(@"{
                'merchant_order_id':'m-1', 'price':15000.0, 'depositor':'홍길동', 'account_id':'acc1',
                'notify':{'deposit_guide':true,'deposit_paid':false,'link':'https://example.com/o/m-1'}
            }"), "idem-deposit");
            await Check(() => _api.DepositDetail("m-1"), "GET", "sdk/deposits/m-1");
            await Check(() => _api.DepositCancel("m-1"), "DELETE", "sdk/deposits/m-1");
            var keys = new HashSet<string>();
            foreach (var request in _server.Requests) Assert.True(keys.Add(request.Headers["Idempotency-Key"]));
        }

        [Theory]
        [InlineData("a/b", "a%2Fb")]
        [InlineData("a?b#c %한", "a%3Fb%23c%20%25%ED%95%9C")]
        public async Task DetailAndCancel_EncodeOrderId(string id, string encoded)
        {
            await Check(() => _api.DepositDetail(id), "GET", "sdk/deposits/" + encoded);
            await Check(() => _api.DepositCancel(id, "cancel-key"), "DELETE", "sdk/deposits/" + encoded, key: "cancel-key");
        }

        [Fact]
        public async Task ExplicitKeysAndFreshGeneratedKeys()
        {
            await Check(() => _api.DepositWebhookSecret("secret-key"), "POST", "sdk/deposits/webhook_secret", new JObject(), "secret-key");
            await Check(() => _api.DepositAccounts(), "GET", "sdk/deposits/accounts");
            var first = _server.LastRequest.Headers["Idempotency-Key"];
            await Check(() => _api.DepositAccounts(), "GET", "sdk/deposits/accounts");
            Assert.NotEqual(first, _server.LastRequest.Headers["Idempotency-Key"]);
        }

        [Fact]
        public async Task Create_CompactsNullsAndKeepsFalseAndEmptyValues()
        {
            await Check(() => _api.DepositCreate(new DepositCreateParams
            {
                MerchantOrderId = "m-2", Price = 1.5, Depositor = "입금자", OrderName = "주문", Memo = "",
                BuyerName = "구매자", BuyerPhone = "01012345678", BuyerEmail = "buyer@example.com",
                AccountId = "acc1", WebhookUrl = "https://example.com/deposit",
                Notify = new DepositNotifyParams { DepositGuide = false }
            }), "POST", "sdk/deposits", JObject.Parse(@"{
                'merchant_order_id':'m-2','price':1.5,'depositor':'입금자','order_name':'주문','memo':'',
                'buyer_name':'구매자','buyer_phone':'01012345678','buyer_email':'buyer@example.com',
                'account_id':'acc1','webhook_url':'https://example.com/deposit','notify':{'deposit_guide':false}
            }"));
            await Check(() => _api.DepositCreate(new DepositCreateParams
            {
                MerchantOrderId = "m-3", Price = 0, Depositor = "입금자"
            }), "POST", "sdk/deposits", JObject.Parse("{'merchant_order_id':'m-3','price':0.0,'depositor':'입금자'}"));
        }

        [Fact]
        public async Task DepositsUseCommerceHost()
        {
            using var message = new MockServer();
            var api = new WireCommerceApi(_server.BaseUrl + "v1/", message.BaseUrl);
            await api.DepositAccounts();
            await api.DepositWebhookSecret();
            await api.DepositCreate(new DepositCreateParams { MerchantOrderId = "m", Price = 1, Depositor = "홍길동" });
            await api.DepositDetail("m");
            await api.DepositCancel("m");
            Assert.Equal(5, _server.Requests.Count);
            Assert.Empty(message.Requests);
        }

        [Theory]
        [InlineData("awaiting")]
        [InlineData("paid")]
        [InlineData("expired")]
        [InlineData("cancelled")]
        public async Task DepositResponse_DeserializesNestedWirePayload(string status)
        {
            _server.ResponseBody = @"{
                'merchant_order_id':'m-1','order_number':'bp-1','order_name':'주문','amount':15000,
                'buyer_name':'구매자','depositor':'입금자',
                'account':{'id':'acc1','bank_name':'은행','account_no':'123','holder':'판매자'},
                'deadline':'2026-10-24','deadline_label':'내일','paid_at':null,
                'notify':{'deposit_guide':true,'deposit_paid':false},
                'deposit':null
            }";
            var json = JObject.Parse(_server.ResponseBody);
            json["status"] = status;
            _server.ResponseBody = json.ToString();
            using var response = await _api.DepositDetail("m-1");
            var deposit = JsonConvert.DeserializeObject<CommerceDeposit>(await response.Content.ReadAsStringAsync())!;
            Assert.Equal("m-1", deposit.MerchantOrderId);
            Assert.Equal("bp-1", deposit.OrderNumber);
            Assert.Equal("주문", deposit.OrderName);
            Assert.Equal(15000, deposit.Amount);
            Assert.Equal(status, deposit.Status);
            Assert.Equal("구매자", deposit.BuyerName);
            Assert.Equal("입금자", deposit.Depositor);
            Assert.Equal("acc1", deposit.Account.Id);
            Assert.Equal("은행", deposit.Account.BankName);
            Assert.Equal("123", deposit.Account.AccountNo);
            Assert.Equal("판매자", deposit.Account.Holder);
            // 주문에 붙은 계좌는 등록 때의 스냅샷이라 primary 가 없다 (계좌 목록에만 실린다)
            Assert.Null(deposit.Account.Primary);
            Assert.Equal("2026-10-24", deposit.Deadline);
            Assert.Equal("내일", deposit.DeadlineLabel);
            Assert.Null(deposit.PaidAt);
            Assert.True(deposit.Notify.DepositGuide);
            Assert.False(deposit.Notify.DepositPaid);
            // 입금 알림(deposit)은 웹훅 본문에만 실린다 — 등록·조회·취소 응답에서는 null 이다
            Assert.Null(deposit.Deposit);
        }

        [Fact]
        public void DepositWebhookPayload_DeserializesDepositNotice()
        {
            var deposit = JsonConvert.DeserializeObject<CommerceDeposit>(@"{
                'merchant_order_id':'m-1','order_number':'bp-1','status':'paid',
                'deposit':{'key':'key1','bank_name':'은행','account_tail':'123','depositor':'입금자','amount':15000,'received_at':'2026-10-23'}
            }")!;
            Assert.Equal("key1", deposit.Deposit.Key);
            Assert.Equal("은행", deposit.Deposit.BankName);
            Assert.Equal("123", deposit.Deposit.AccountTail);
            Assert.Equal("입금자", deposit.Deposit.Depositor);
            Assert.Equal(15000, deposit.Deposit.Amount);
            Assert.Equal("2026-10-23", deposit.Deposit.ReceivedAt);
        }

        [Fact]
        public async Task AccountAndSecretResponses_Deserialize()
        {
            _server.ResponseBody = "{'accounts':[{'id':'acc1','primary':false}]}";
            using var accountsResponse = await _api.DepositAccounts();
            var accounts = JsonConvert.DeserializeObject<DepositAccountsResponse>(await accountsResponse.Content.ReadAsStringAsync())!;
            Assert.Single(accounts.Accounts);
            Assert.Equal("acc1", accounts.Accounts[0].Id);
            Assert.False(accounts.Accounts[0].Primary);
            _server.ResponseBody = "{'secret':'new-secret'}";
            using var secretResponse = await _api.DepositWebhookSecret();
            Assert.Equal("new-secret", JsonConvert.DeserializeObject<DepositWebhookSecretResponse>(await secretResponse.Content.ReadAsStringAsync())!.Secret);
            var sparse = JsonConvert.DeserializeObject<CommerceDeposit>("{'account':null,'notify':null,'deposit':null,'deadline':null,'deadline_label':null,'paid_at':null}")!;
            Assert.Null(sparse.Account);
            Assert.Null(sparse.Notify);
            Assert.Null(sparse.Deposit);
        }

        [Fact]
        public void BankDepositEvents_MatchWireCodes()
        {
            Assert.Equal(new[] { 400, 401, 402, 403, 404, 405, 406, 407 }, new[] {
                BankDepositWebhookEvent.Received, BankDepositWebhookEvent.Matched, BankDepositWebhookEvent.Ambiguous,
                BankDepositWebhookEvent.Unmatched, BankDepositWebhookEvent.ConfirmedManually,
                BankDepositWebhookEvent.Expired, BankDepositWebhookEvent.Cancelled, BankDepositWebhookEvent.Reverted
            });
        }
    }
}
