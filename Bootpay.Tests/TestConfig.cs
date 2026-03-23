using System;

namespace Bootpay.Tests
{
    /// <summary>
    /// Integration test configuration.
    /// Environment is controlled by the BOOTPAY_ENV environment variable.
    /// Defaults to "development" when not set.
    /// </summary>
    public static class TestConfig
    {
        public static string Env =>
            Environment.GetEnvironmentVariable("BOOTPAY_ENV") ?? "development";

        public static bool IsProduction => Env == "production";

        #region PG API Keys

        public static class PG
        {
            public const string DevApplicationId = "59bfc738e13f337dbd6ca48a";
            public const string DevPrivateKey = "pDc0NwlkEX3aSaHTp/PPL/i8vn5E/CqRChgyEp/gHD0=";

            public const string ProdApplicationId = "5b8f6a4d396fa665fdc2b5ea";
            public const string ProdPrivateKey = "rm6EYECr6aroQVG2ntW0A6LpWnkTgP4uQ3H18sDDUYw=";

            public static string ApplicationId => IsProduction ? ProdApplicationId : DevApplicationId;
            public static string PrivateKey => IsProduction ? ProdPrivateKey : DevPrivateKey;

            public static int Mode => IsProduction
                ? BootpayObject.MODE_PRODUCTION
                : BootpayObject.MODE_DEVELOPMENT;
        }

        #endregion

        #region Commerce API Keys

        public static class Commerce
        {
            public const string DevClientKey = "hxS-Up--5RvT6oU6QJE0JA";
            public const string DevSecretKey = "r5zxvDcQJiAP2PBQ0aJjSHQtblNmYFt6uFoEMhti_mg=";

            public const string ProdClientKey = "sEN72kYZBiyMNytA8nUGxQ";
            public const string ProdSecretKey = "rnZLJamENRgfwTccwmI_Uu9cxsPpAV9X2W-Htg73yfU=";

            public static string ClientKey => IsProduction ? ProdClientKey : DevClientKey;
            public static string SecretKey => IsProduction ? ProdSecretKey : DevSecretKey;

            public static int Mode => IsProduction
                ? Bootpay.Commerce.BootpayCommerceObject.MODE_PRODUCTION
                : Bootpay.Commerce.BootpayCommerceObject.MODE_DEVELOPMENT;
        }

        #endregion

        #region Test Data (receipt IDs, billing keys, etc.)

        public static class Data
        {
            public const string ReceiptId = "628b2206d01c7e00209b6087";
            public const string ReceiptIdConfirm = "62876963d01c7e001e9b6028";
            public const string ReceiptIdCash = "62e0f11f1fc192036b1b3c92";
            public const string ReceiptIdEscrow = "628ae7ffd01c7e001e9b6066";
            public const string ReceiptIdBilling = "62c7ccebcf9f6d001b3adcd4";
            public const string ReceiptIdTransfer = "66541bc4ca4517e69343e24c";
            public const string BillingKey = "628b2644d01c7e00209b6092";
            public const string BillingKey2 = "66542dfb4d18d5fc7b43e1b6";
            public const string ReserveId = "6490149ca575b40024f0b70d";
            public const string UserId = "1234";
            public const string CertificateReceiptId = "61b009aaec81b4057e7f6ecd";
        }

        #endregion
    }
}
