using System.Threading.Tasks;
using Bootpay.models;
using Xunit;
using Xunit.Abstractions;

namespace Bootpay.Tests
{
    /// <summary>
    /// PG API - Payment (receipt lookup, confirm, cancel)
    /// </summary>
    public class PgPaymentTest
    {
        private readonly ITestOutputHelper _output;

        public PgPaymentTest(ITestOutputHelper output)
        {
            _output = output;
        }

        private async Task<BootpayApi> CreateAuthenticatedApi()
        {
            var api = TestConfig.PG.CreateBootpay();
            // BOOTPAY_AUTH_MODE=legacy 일 때만 토큰 발급이 실제로 일어남 (ck/sk 모드는 합성 응답 반환).
            await api.GetAccessToken();
            return api;
        }

        [Fact]
        public async Task GetReceipt_ShouldReturnReceiptData()
        {
            var api = await CreateAuthenticatedApi();

            var res = await api.GetReceipt(TestConfig.Data.ReceiptId);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task Confirm_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var res = await api.Confirm(TestConfig.Data.ReceiptIdConfirm);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task ReceiptCancel_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var cancel = new Cancel
            {
                receiptId = TestConfig.Data.ReceiptId,
                cancelUsername = "test_admin",
                cancelMessage = "integration test cancel"
            };

            var res = await api.ReceiptCancel(cancel);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task GetUserToken_ShouldReturnUserToken()
        {
            var api = await CreateAuthenticatedApi();

            var userToken = new UserToken
            {
                userId = TestConfig.Data.UserId
            };

            var res = await api.GetUserToken(userToken);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task PutShippingStart_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var shipping = new Shipping
            {
                receiptId = TestConfig.Data.ReceiptIdEscrow,
                trackingNumber = "123456",
                deliveryCorp = "CJ대한통운"
            };
            shipping.user = new ShippingUser
            {
                username = "홍길동",
                phone = "01000000000",
                address = "서울특별시 종로구",
                zipcode = "08490"
            };

            var res = await api.PutShippingStart(shipping);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }
    }
}
