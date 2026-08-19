using System;
using System.Threading.Tasks;
using Bootpay.Commerce;
using Bootpay.Commerce.Models;
using Xunit;
using Xunit.Abstractions;

namespace Bootpay.Tests
{
    /// <summary>
    /// Commerce API - User (join, token, list, detail, update, check exist, delete)
    /// </summary>
    public class CommerceUserTest
    {
        private readonly ITestOutputHelper _output;

        public CommerceUserTest(ITestOutputHelper output)
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

        [LiveFact]
        public async Task UserJoin_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var user = new CommerceUser
            {
                LoginId = "testuser_" + DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                LoginPw = "password123",
                Email = "test_integration@bootpay.co.kr",
                Phone = "01000000000",
                Name = "integration_test"
            };

            var res = await api.UserJoin(user);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [LiveFact]
        public async Task UserToken_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            // Use a known user ID; if the user does not exist, the API returns an error but the call still completes.
            var res = await api.UserToken("684fa4a6b0eacea5cd97464e");
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [LiveFact]
        public async Task UserCheckExist_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var res = await api.UserCheckExist("email-exist", "test@bootpay.co.kr");
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [LiveFact]
        public async Task UserList_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var listParams = new UserListParams
            {
                Page = 1,
                Limit = 10
            };

            var res = await api.UserList(listParams);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [LiveFact]
        public async Task UserDetail_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var res = await api.UserDetail("684fa4a6b0eacea5cd97464e");
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [LiveFact]
        public async Task UserUpdate_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            var user = new CommerceUser
            {
                UserId = "684fa4a6b0eacea5cd97464e",
                Phone = "01012345678",
                Name = "integration_test_updated"
            };

            var res = await api.UserUpdate(user);
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }

        [LiveFact]
        public async Task UserDelete_ShouldReturnResponse()
        {
            var api = await CreateAuthenticatedApi();

            // WARNING: This deletes a user. Use a disposable test user ID.
            var res = await api.UserDelete("integration_test_delete_placeholder");
            var content = await res.Content.ReadAsStringAsync();

            _output.WriteLine($"Status: {res.StatusCode}");
            _output.WriteLine($"Response: {content}");

            Assert.NotNull(res);
            Assert.NotNull(content);
        }
    }
}
