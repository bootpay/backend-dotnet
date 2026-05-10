using System.Threading.Tasks;
using Bootpay.Commerce;
using Bootpay.Commerce.Models;
using Xunit;
using Xunit.Abstractions;

namespace Bootpay.Tests
{
    /// <summary>
    /// Commerce API - Product (create, list, detail, update, status, delete)
    /// </summary>
    public class CommerceProductTest
    {
        private readonly ITestOutputHelper _output;

        public CommerceProductTest(ITestOutputHelper output)
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
        public async Task ProductCreate_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var product = new CommerceProduct
            {
                Name = "integration test product",
                DisplayPrice = 10000,
                Type = 1
            };

            var res = await api.ProductCreate(product);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task ProductList_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var listParams = new ProductListParams
            {
                Page = 1,
                Limit = 10
            };

            var res = await api.ProductList(listParams);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task ProductDetail_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            // Use a known product ID or a placeholder
            var res = await api.ProductDetail("product_id_placeholder");
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task ProductUpdate_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var product = new CommerceProduct
            {
                ProductId = "product_id_placeholder",
                Name = "integration test product updated",
                DisplayPrice = 20000
            };

            var res = await api.ProductUpdate(product);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task ProductStatus_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var statusParams = new ProductStatusParams
            {
                ProductId = "product_id_placeholder",
                Status = 1
            };

            var res = await api.ProductStatus(statusParams);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task ProductDelete_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var res = await api.ProductDelete("product_id_placeholder");
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }
    }
}
