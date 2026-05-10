using System.Threading.Tasks;
using Bootpay.Commerce;
using Bootpay.Commerce.Models;
using Xunit;
using Xunit.Abstractions;

namespace Bootpay.Tests
{
    /// <summary>
    /// Commerce API - Order (list, detail, month, subscription list/detail)
    /// </summary>
    public class CommerceOrderTest
    {
        private readonly ITestOutputHelper _output;

        public CommerceOrderTest(ITestOutputHelper output)
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
        public async Task OrderList_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var listParams = new OrderListParams
            {
                Page = 1,
                Limit = 10
            };

            var res = await api.OrderList(listParams);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task OrderDetail_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var res = await api.OrderDetail("order_id_placeholder");
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task OrderMonth_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var res = await api.OrderMonth("user_group_id_placeholder", "2026-03");
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task OrderCancelList_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var listParams = new OrderCancelListParams();

            var res = await api.OrderCancelList(listParams);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task OrderSubscriptionList_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var listParams = new OrderSubscriptionListParams
            {
                Page = 1,
                Limit = 10
            };

            var res = await api.OrderSubscriptionList(listParams);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task OrderSubscriptionDetail_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var res = await api.OrderSubscriptionDetail("order_subscription_id_placeholder");
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task OrderSubscriptionBillList_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var listParams = new OrderSubscriptionBillListParams
            {
                Page = 1,
                Limit = 10
            };

            var res = await api.OrderSubscriptionBillList(listParams);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }
    }
}
