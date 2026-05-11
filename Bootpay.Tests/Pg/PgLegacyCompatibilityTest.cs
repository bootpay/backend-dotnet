using System.Reflection;
using Xunit;

namespace Bootpay.Tests
{
    public class PgLegacyCompatibilityTest
    {
        private static string GetField(BootpayApi api, string fieldName)
        {
            var field = typeof(BootpayObject).GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            return (string)field!.GetValue(api)!;
        }

        [Fact]
        public void LegacyConstructor_ShouldKeepApplicationCredentials()
        {
            var api = new BootpayApi("legacy_application_id", "legacy_private_key", BootpayObject.MODE_DEVELOPMENT);

            Assert.Equal("legacy_application_id", GetField(api, "_applicationId"));
            Assert.Equal("legacy_private_key", GetField(api, "_privateKey"));
            Assert.Null(GetField(api, "_clientKey"));
            Assert.Null(GetField(api, "_secretKey"));
            Assert.Equal("https://dev-api.bootpay.co.kr/v2/", GetField(api, "_baseUrl"));
        }
    }
}
