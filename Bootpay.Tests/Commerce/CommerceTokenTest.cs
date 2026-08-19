using System.Threading.Tasks;
using Bootpay.Commerce;
using Xunit;
using Xunit.Abstractions;

namespace Bootpay.Tests
{
    /// <summary>
    /// Commerce API - Token (access token issuance)
    /// </summary>
    public class CommerceTokenTest
    {
        private readonly ITestOutputHelper _output;

        public CommerceTokenTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [LiveFact]
        public async Task GetAccessToken_ShouldReturnToken()
        {
            var api = new BootpayCommerceApi(
                TestConfig.Commerce.ClientKey,
                TestConfig.Commerce.SecretKey,
                TestConfig.Commerce.Mode
            );

            var res = await api.GetAccessToken();
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.True(res.IsSuccessStatusCode, $"GetAccessToken failed: {content}");
            Assert.NotNull(content);
            Assert.Contains("access_token", content);
        }
    }
}
