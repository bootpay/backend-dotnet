using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
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

        // 합성 응답 자체는 오프라인이지만, TestConfig 키 미주입 시 빈 ck/sk 가 legacy 분기로 빠져
        // 실제 request/token 호출이 발생하므로 development 게이트가 필요하다.
        [LiveFact]
        public async Task GetAccessToken_CkSk_ReturnsSyntheticEmptyResponse()
        {
            // ck/sk 모드는 매 요청 Basic Auth 로 인증하므로 GetAccessToken 은 HTTP 호출 없이 합성 응답을 돌려준다.
            var api = BootpayApi.WithClientKey(
                TestConfig.PG.ClientKey,
                TestConfig.PG.SecretKey,
                TestConfig.PG.Mode
            );

            var res = await api.GetAccessToken();
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"ck/sk Status: {res.StatusCode}");
            _output.WriteLine($"ck/sk Response: {content}");

            Assert.True(res.IsSuccessStatusCode);
            var parsed = JObject.Parse(content);
            Assert.Equal("", (string)parsed["access_token"]);
            Assert.Equal(0, (int)parsed["expire_in"]);
        }

        [LiveFact]
        public async Task GetAccessToken_Legacy_IssuesRealAccessToken()
        {
            // legacy application_id/private_key 모드는 request/token 호출 후 실제 토큰을 발급받는다.
            var api = new BootpayApi(
                TestConfig.PG.ApplicationId,
                TestConfig.PG.PrivateKey,
                TestConfig.PG.Mode
            );

            var res = await api.GetAccessToken();
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"legacy Status: {res.StatusCode}");
            _output.WriteLine($"legacy Response: {content}");

            Assert.True(res.IsSuccessStatusCode, $"legacy GetAccessToken failed: {content}");
            var parsed = JObject.Parse(content);
            var accessToken = (string)parsed["access_token"];
            Assert.False(string.IsNullOrEmpty(accessToken), "legacy access_token must not be empty");
            Assert.True((int)parsed["expire_in"] > 0, "legacy expire_in must be positive");
        }
    }
}
