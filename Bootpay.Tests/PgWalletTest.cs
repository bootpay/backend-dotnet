using System;
using System.Threading.Tasks;
using Bootpay.models;
using Xunit;
using Xunit.Abstractions;

namespace Bootpay.Tests
{
    /// <summary>
    /// PG API - Wallet (user wallets, wallet payment)
    /// </summary>
    public class PgWalletTest
    {
        private readonly ITestOutputHelper _output;

        public PgWalletTest(ITestOutputHelper output)
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
        public async Task GetUserWallets_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var res = await api.GetUserWallets(TestConfig.Data.UserId, true);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task RequestWalletPayment_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var walletRequest = new WalletRequest
            {
                userId = TestConfig.Data.UserId,
                orderName = "wallet payment test",
                price = 1000,
                orderId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(),
                sandbox = true
            };

            var res = await api.RequestWalletPayment(walletRequest);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }
    }
}
