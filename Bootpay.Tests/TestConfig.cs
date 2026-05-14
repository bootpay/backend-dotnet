using System;

namespace Bootpay.Tests
{
    /// <summary>
    /// Integration test configuration.
    /// Environment is controlled by the BOOTPAY_ENV environment variable.
    /// Defaults to "production" when not set.
    /// </summary>
    public static class TestConfig
    {
        private static readonly System.Collections.Generic.Dictionary<string, string> DotEnv = LoadDotEnv();

        private static System.Collections.Generic.Dictionary<string, string> LoadDotEnv()
        {
            var values = new System.Collections.Generic.Dictionary<string, string>();
            foreach (var file in new[] { ".env", "../.env" })
            {
                if (!System.IO.File.Exists(file)) continue;
                foreach (var raw in System.IO.File.ReadAllLines(file))
                {
                    var line = raw.Trim();
                    if (line.Length == 0 || line.StartsWith("#") || !line.Contains("=")) continue;
                    var parts = line.Split(new[] { '=' }, 2);
                    var value = parts[1].Trim().Trim('"', '\'');
                    if (!values.ContainsKey(parts[0].Trim())) values[parts[0].Trim()] = value;
                }
            }
            return values;
        }

        private static string EnvValue(string key, string fallback)
        {
            var value = Environment.GetEnvironmentVariable(key);
            if (!string.IsNullOrEmpty(value)) return value;
            return DotEnv.TryGetValue(key, out value) && !string.IsNullOrEmpty(value) ? value : fallback;
        }

        public static string Env =>
            EnvValue("BOOTPAY_ENV", "production");

        public static bool IsProduction => Env == "production";

        // PG 인증 방식: "new" (client_key/secret_key) 또는 "legacy" (application_id/private_key).
        // 매 실행 시 BOOTPAY_AUTH_MODE 환경변수로 토글한다.
        public static string AuthMode =>
            EnvValue("BOOTPAY_AUTH_MODE", "new").ToLowerInvariant();

        #region PG API Keys

        public static class PG
        {
            // ck/sk 와 legacy application_id/private_key 모두 .env / 환경변수 로 주입 (.env.example 참고)
            public static string ClientKey => IsProduction ? EnvValue("BOOTPAY_PG_CLIENT_KEY_PROD", "") : EnvValue("BOOTPAY_PG_CLIENT_KEY_DEV", "");
            public static string SecretKey => IsProduction ? EnvValue("BOOTPAY_PG_SECRET_KEY_PROD", "") : EnvValue("BOOTPAY_PG_SECRET_KEY_DEV", "");
            public static string ApplicationId => IsProduction ? EnvValue("BOOTPAY_PG_APPLICATION_ID_PROD", "") : EnvValue("BOOTPAY_PG_APPLICATION_ID_DEV", "");
            public static string PrivateKey => IsProduction ? EnvValue("BOOTPAY_PG_PRIVATE_KEY_PROD", "") : EnvValue("BOOTPAY_PG_PRIVATE_KEY_DEV", "");

            public static int Mode => IsProduction
                ? BootpayObject.MODE_PRODUCTION
                : BootpayObject.MODE_DEVELOPMENT;

            /// <summary>
            /// BOOTPAY_AUTH_MODE 에 따라 ck/sk(default) 또는 legacy application_id/private_key 로 BootpayApi 인스턴스 생성.
            /// </summary>
            public static BootpayApi CreateBootpay()
            {
                if (AuthMode == "legacy")
                {
                    Console.WriteLine($"[BOOTPAY_AUTH_MODE=legacy] PG: application_id/private_key (Bearer) | env={Env}");
                    return new BootpayApi(ApplicationId, PrivateKey, Mode);
                }
                Console.WriteLine($"[BOOTPAY_AUTH_MODE=new] PG: client_key/secret_key (Basic Auth) | env={Env}");
                return BootpayApi.WithClientKey(ClientKey, SecretKey, Mode);
            }
        }

        #endregion

        #region Commerce API Keys

        public static class Commerce
        {
            // ck/sk 는 .env / 환경변수 로 주입 (.env.example 참고)
            public static string ClientKey => IsProduction ? EnvValue("BOOTPAY_COMMERCE_CLIENT_KEY_PROD", "") : EnvValue("BOOTPAY_COMMERCE_CLIENT_KEY_DEV", "");
            public static string SecretKey => IsProduction ? EnvValue("BOOTPAY_COMMERCE_SECRET_KEY_PROD", "") : EnvValue("BOOTPAY_COMMERCE_SECRET_KEY_DEV", "");

            public static int Mode => IsProduction
                ? Bootpay.Commerce.BootpayCommerceObject.MODE_PRODUCTION
                : Bootpay.Commerce.BootpayCommerceObject.MODE_DEVELOPMENT;
        }

        #endregion

        #region Test Data (receipt IDs, billing keys, etc.)

        public static class Data
        {
            public const string ReceiptId = "628b2206d01c7e00209b6087";
            public const string ReceiptIdConfirm = "62876963d01c7e00209b6028";
            public const string ReceiptIdCash = "62e0f11f1fc192036b1b3c92";
            public const string ReceiptIdEscrow = "628ae7ffd01c7e001e9b6066";
            public const string ReceiptIdBilling = "62c7ccebcf9f6d001b3adcd4";
            public const string ReceiptIdTransfer = "66541bc4ca4517e69343e24c";
            public const string BillingKey = "628b2644d01c7e00209b6092";
            public const string BillingKey2 = "66542dfb4d18d5fc7b43e1b6";
            public const string ReserveId = "6490149ca575b40024f0b70d";
            public const string ReserveId2 = "628b316cd01c7e00219b6081";
            public const string UserId = "1234";
            public const string CertificateReceiptId = "69fd7187564d1f550535538c";
        }

        #endregion
    }
}
