using System;
namespace Sample.Models
{
    static class Constants
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

        // PG ck/sk 와 legacy application_id/private_key 모두 .env / 환경변수 로 주입 (.env.example 참고)
        public static readonly string client_key = EnvValue("BOOTPAY_PG_CLIENT_KEY_PROD", "");
        public static readonly string secret_key = EnvValue("BOOTPAY_PG_SECRET_KEY_PROD", "");
        public static readonly string application_id = EnvValue("BOOTPAY_PG_APPLICATION_ID_PROD", "");
        public static readonly string private_key = EnvValue("BOOTPAY_PG_PRIVATE_KEY_PROD", "");

        public static readonly string dev_client_key = EnvValue("BOOTPAY_PG_CLIENT_KEY_DEV", "");
        public static readonly string dev_secret_key = EnvValue("BOOTPAY_PG_SECRET_KEY_DEV", "");
        public static readonly string dev_application_id = EnvValue("BOOTPAY_PG_APPLICATION_ID_DEV", "");
        public static readonly string dev_private_key = EnvValue("BOOTPAY_PG_PRIVATE_KEY_DEV", "");
    }
}
