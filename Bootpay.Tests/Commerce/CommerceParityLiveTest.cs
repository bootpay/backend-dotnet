using System.Threading.Tasks;
using Bootpay.Commerce;
using Bootpay.Commerce.Models;
using Xunit;
using Xunit.Abstractions;

namespace Bootpay.Tests
{
    /// <summary>
    /// NodeJS 2.9.0 parity 신규 기능 라이브 테스트 — development 환경 전용.
    /// LiveFact 게이트로 BOOTPAY_ENV=development 가 아니면 skip 된다 (production 호출 금지).
    /// wire-format 규약 검증은 Bootpay.Tests/Wire 가 담당한다.
    /// </summary>
    public class CommerceParityLiveTest
    {
        private readonly ITestOutputHelper _output;

        public CommerceParityLiveTest(ITestOutputHelper output)
        {
            _output = output;
        }

        private BootpayCommerceApi CreateApi()
        {
            return new BootpayCommerceApi(
                TestConfig.Commerce.ClientKey,
                TestConfig.Commerce.SecretKey,
                TestConfig.Commerce.Mode
            );
        }

        [LiveFact]
        public async Task MallSetting_Get_ShouldReturnResponse()
        {
            var api = CreateApi();
            var res = await api.GetMallSetting();
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [LiveFact]
        public async Task Webhook_SendTest_ShouldReturnResponse()
        {
            var api = CreateApi();
            var res = await api.WebhookSendTest(new SendTestWebhookParams { HeaderContentType = 0 });
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [LiveFact]
        public async Task Products_Mall_ShouldReturnResponse()
        {
            var api = CreateApi();
            var res = await api.Products(new MallProductListParams { Page = 1, Limit = 5 });
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }
    }
}
