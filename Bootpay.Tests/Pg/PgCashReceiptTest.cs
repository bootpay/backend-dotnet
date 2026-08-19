using System;
using System.Threading.Tasks;
using Bootpay.models;
using Xunit;
using Xunit.Abstractions;

namespace Bootpay.Tests
{
    /// <summary>
    /// PG API - Cash Receipt (request/cancel cash receipts)
    /// </summary>
    public class PgCashReceiptTest
    {
        private readonly ITestOutputHelper _output;

        public PgCashReceiptTest(ITestOutputHelper output)
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

        [LiveFact]
        public async Task RequestCashReceipt_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var cashReceipt = new CashReceipt
            {
                pg = "토스",
                price = 1000,
                orderName = "cash receipt test",
                cashReceiptType = "소득공제",
                identityNo = "01000000000",
                purchasedAt = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:sszzz"),
                orderId = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString()
            };

            var res = await api.RequestCashReceipt(cashReceipt);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [LiveFact]
        public async Task RequestCashReceiptCancel_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var cancel = new Cancel
            {
                receiptId = TestConfig.Data.ReceiptIdCash,
                cancelMessage = "cash receipt cancel test",
                cancelUsername = "test_admin"
            };

            var res = await api.RequestCashReceiptCancel(cancel);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [LiveFact]
        public async Task RequestCashReceiptByBootpay_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var cashReceipt = new CashReceipt
            {
                receiptId = TestConfig.Data.ReceiptIdCash,
                username = "테스트",
                email = "test@bootpay.co.kr",
                phone = "01000000000",
                identityNo = "01000000000",
                cashReceiptType = "소득공제"
            };

            var res = await api.RequestCashReceiptByBootpay(cashReceipt);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [LiveFact]
        public async Task RequestCashReceiptCancelByBootpay_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var cancel = new Cancel
            {
                receiptId = TestConfig.Data.ReceiptIdCash,
                cancelMessage = "cash receipt cancel by bootpay test",
                cancelUsername = "test_admin"
            };

            var res = await api.RequestCashReceiptCancelByBootpay(cancel);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }
    }
}
