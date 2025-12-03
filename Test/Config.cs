namespace Test
{
    /// <summary>
    /// SDK 테스트용 설정 클래스
    /// </summary>
    public static class Config
    {
        // 현재 환경: "production" 또는 "development"
        public const string CurrentEnv = "production";

        /// <summary>
        /// PG API 키
        /// </summary>
        public static class PG
        {
            // Production 환경
            public const string ProductionApplicationId = "5b8f6a4d396fa665fdc2b5ea";
            public const string ProductionPrivateKey = "rm6EYECr6aroQVG2ntW0A6LpWnkTgP4uQ3H18sDDUYw=";

            // Development 환경
            public const string DevApplicationId = "59bfc738e13f337dbd6ca48a";
            public const string DevPrivateKey = "pDc0NwlkEX3aSaHTp/PPL/i8vn5E/CqRChgyEp/gHD0=";

            public static string GetApplicationId() =>
                CurrentEnv == "production" ? ProductionApplicationId : DevApplicationId;

            public static string GetPrivateKey() =>
                CurrentEnv == "production" ? ProductionPrivateKey : DevPrivateKey;
        }

        /// <summary>
        /// Commerce API 키
        /// </summary>
        public static class Commerce
        {
            // Production 환경
            public const string ProductionClientKey = "sEN72kYZBiyMNytA8nUGxQ";
            public const string ProductionSecretKey = "rnZLJamENRgfwTccwmI_Uu9cxsPpAV9X2W-Htg73yfU=";

            // Development 환경
            public const string DevClientKey = "hxS-Up--5RvT6oU6QJE0JA";
            public const string DevSecretKey = "r5zxvDcQJiAP2PBQ0aJjSHQtblNmYFt6uFoEMhti_mg=";

            public static string GetClientKey() =>
                CurrentEnv == "production" ? ProductionClientKey : DevClientKey;

            public static string GetSecretKey() =>
                CurrentEnv == "production" ? ProductionSecretKey : DevSecretKey;
        }

        /// <summary>
        /// 테스트 데이터
        /// </summary>
        public static class TestData
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
            public const string CertificateReceiptId = "61b009aaec81b4057e7f6ecd";
        }
    }
}
