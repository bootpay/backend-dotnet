using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Bootpay.Tests
{
    /// <summary>
    /// PG API - Token (access token issuance)
    /// </summary>
    public class PgTokenTest
    {
        private readonly ITestOutputHelper _output;

        public PgTokenTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task GetAccessToken_ShouldReturnToken()
        {
            var api = new BootpayApi(
                TestConfig.PG.ApplicationId,
                TestConfig.PG.PrivateKey,
                TestConfig.PG.Mode
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
