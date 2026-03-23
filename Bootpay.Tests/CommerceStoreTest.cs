using System.Threading.Tasks;
using Bootpay.Commerce;
using Xunit;
using Xunit.Abstractions;

namespace Bootpay.Tests
{
    /// <summary>
    /// Commerce API - Store (info, detail)
    /// </summary>
    public class CommerceStoreTest
    {
        private readonly ITestOutputHelper _output;

        public CommerceStoreTest(ITestOutputHelper output)
        {
            _output = output;
        }

        private async Task<BootpayCommerceApi> CreateAuthenticatedApi()
        {
            var api = new BootpayCommerceApi(
                TestConfig.Commerce.ClientKey,
                TestConfig.Commerce.SecretKey,
                TestConfig.Commerce.Mode
            );
            var tokenRes = await api.GetAccessToken();
            Assert.True(tokenRes.IsSuccessStatusCode, "Commerce token issuance failed");
            return api;
        }

        [Fact]
        public async Task StoreInfo_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var res = await api.StoreInfo();
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task StoreDetail_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var res = await api.StoreDetail();
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }
    }
}
