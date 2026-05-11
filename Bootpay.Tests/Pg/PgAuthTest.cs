using System;
using System.Threading.Tasks;
using Bootpay.models;
using Xunit;
using Xunit.Abstractions;

namespace Bootpay.Tests
{
    /// <summary>
    /// PG API - Authentication (certificate, request/confirm/realarm authentication)
    /// </summary>
    public class PgAuthTest
    {
        private readonly ITestOutputHelper _output;

        public PgAuthTest(ITestOutputHelper output)
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
        public async Task Certificate_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var res = await api.Certificate(TestConfig.Data.CertificateReceiptId);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task RequestAuthentication_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var authentication = new Authentication
            {
                pg = "다날",
                method = "본인인증",
                username = "테스트사용자",
                identityNo = "0000000",
                carrier = "SKT",
                phone = "01010002000",
                siteUrl = "https://www.bootpay.co.kr",
                orderName = "본인인증 테스트",
                authenticationId = DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()
            };

            var res = await api.RequestAuthentication(authentication);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task ConfirmAuthentication_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var authParams = new AuthenticationParams
            {
                receiptId = "636a0bc4d01c7e00331cd25e",
                otp = "556659"
            };

            var res = await api.ConfirmAuthentication(authParams);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task RealarmAuthentication_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var authParams = new AuthenticationParams
            {
                receiptId = "6369dc33d01c7e00271cccad"
            };

            var res = await api.RealarmAuthentication(authParams);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }
    }
}
