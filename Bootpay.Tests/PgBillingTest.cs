using System;
using System.Threading.Tasks;
using Bootpay.models;
using Xunit;
using Xunit.Abstractions;

namespace Bootpay.Tests
{
    /// <summary>
    /// PG API - Billing (subscribe billing key, payment, reserve, destroy)
    /// </summary>
    public class PgBillingTest
    {
        private readonly ITestOutputHelper _output;

        public PgBillingTest(ITestOutputHelper output)
        {
            _output = output;
        }

        private async Task<BootpayApi> CreateAuthenticatedApi()
        {
            var api = new BootpayApi(
                TestConfig.PG.ApplicationId,
                TestConfig.PG.PrivateKey,
                TestConfig.PG.Mode
            );
            var tokenRes = await api.GetAccessToken();
            Assert.True(tokenRes.IsSuccessStatusCode, "Token issuance failed");
            return api;
        }

        [Fact]
        public async Task GetBillingKey_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var subscribe = new Subscribe
            {
                orderName = "billing key test",
                subscriptionId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                pg = "나이스페이",
                cardNo = "5570**********1074",
                cardPw = "**",
                cardExpireYear = "**",
                cardExpireMonth = "**",
                cardIdentityNo = ""
            };

            var res = await api.GetBillingKey(subscribe);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task GetBillingKeyTransfer_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var subscribe = new Subscribe
            {
                orderName = "transfer billing key test",
                pg = "나이스페이",
                bankName = "국민",
                bankAccount = "67512341234472",
                username = "홍길동",
                identityNo = "901014",
                phone = "01012341234",
                subscriptionId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
            };

            var res = await api.GetBillingKeyTransfer(subscribe);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task PublishBillingKeyTransfer_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var res = await api.PublishBillingKeyTransfer(TestConfig.Data.ReceiptIdTransfer);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task LookupBillingKey_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var res = await api.LookupBillingKey(TestConfig.Data.ReceiptIdBilling);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task LookupBillingKeyByKey_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var res = await api.LookupBillingKeyByKey(TestConfig.Data.BillingKey2);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task RequestSubscribe_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var payload = new SubscribePayload
            {
                billingKey = TestConfig.Data.BillingKey,
                orderName = "subscribe payment test",
                price = 1000,
                orderId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
            };

            var res = await api.RequestSubscribe(payload);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task ReserveSubscribe_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var reserveExecuteAt = DateTime.UtcNow.AddSeconds(10).ToString("yyyy-MM-ddTHH:mm:sszzz");

            var payload = new SubscribePayload
            {
                billingKey = TestConfig.Data.BillingKey,
                orderName = "reserve subscribe test",
                price = 1000,
                orderId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                reserveExecuteAt = reserveExecuteAt
            };

            var res = await api.ReserveSubscribe(payload);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task ReserveSubscribeLookup_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var res = await api.ReserveSubscribeLookup(TestConfig.Data.ReserveId);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task ReserveCancelSubscribe_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var res = await api.ReserveCancelSubscribe(TestConfig.Data.ReserveId);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task DestroyBillingKey_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var res = await api.DestroyBillingKey(TestConfig.Data.BillingKey);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }
    }
}
