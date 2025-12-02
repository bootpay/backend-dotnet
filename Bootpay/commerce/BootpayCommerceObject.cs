using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Bootpay.Commerce.Models;
using Newtonsoft.Json;

namespace Bootpay.Commerce
{
    /// <summary>
    /// Bootpay Commerce API 기본 클래스
    /// </summary>
    public class BootpayCommerceObject
    {
        protected string _clientKey;
        protected string _secretKey;
        protected string _baseUrl;
        private string _token;
        private string _role;

        public const int MODE_DEVELOPMENT = 0;
        public const int MODE_STAGE = 2;
        public const int MODE_PRODUCTION = 3;

        private const string SDK_VERSION = "1.0.0";
        private const string API_VERSION = "1.0.0";
        private const string SDK_TYPE = "307";

        private readonly Dictionary<int, string> _URL = new Dictionary<int, string>()
        {
            { MODE_DEVELOPMENT, "https://dev-api.bootapi.com/v1/" },
            { MODE_STAGE, "https://stage-api.bootapi.com/v1/" },
            { MODE_PRODUCTION, "https://api.bootapi.com/v1/" },
        };

        public BootpayCommerceObject(string clientKey, string secretKey, int mode = MODE_PRODUCTION)
        {
            _clientKey = clientKey;
            _secretKey = secretKey;
            _baseUrl = _URL[mode];
            _role = "user";
        }

        /// <summary>
        /// 액세스 토큰 발급
        /// </summary>
        public async Task<HttpResponseMessage> GetAccessToken()
        {
            var data = new
            {
                client_key = _clientKey,
                secret_key = _secretKey
            };

            string json = JsonConvert.SerializeObject(data,
                Newtonsoft.Json.Formatting.None,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

            var res = await SendWithBasicAuthAsync("request/token", HttpMethod.Post, json);
            if (res.IsSuccessStatusCode)
            {
                string resJson = await res.Content.ReadAsStringAsync();
                var resToken = JsonConvert.DeserializeObject<CommerceTokenResponse>(resJson);
                _token = resToken.AccessToken;
            }
            return res;
        }

        /// <summary>
        /// Role 설정
        /// </summary>
        public BootpayCommerceObject WithRole(string role)
        {
            _role = role;
            return this;
        }

        /// <summary>
        /// 일반 사용자 role로 설정
        /// </summary>
        public BootpayCommerceObject AsUser()
        {
            return WithRole("user");
        }

        /// <summary>
        /// 매니저 role로 설정
        /// </summary>
        public BootpayCommerceObject AsManager()
        {
            return WithRole("manager");
        }

        /// <summary>
        /// 파트너 role로 설정
        /// </summary>
        public BootpayCommerceObject AsPartner()
        {
            return WithRole("partner");
        }

        /// <summary>
        /// 벤더 role로 설정
        /// </summary>
        public BootpayCommerceObject AsVendor()
        {
            return WithRole("vendor");
        }

        /// <summary>
        /// 슈퍼바이저 role로 설정
        /// </summary>
        public BootpayCommerceObject AsSupervisor()
        {
            return WithRole("supervisor");
        }

        /// <summary>
        /// 현재 role 반환
        /// </summary>
        public string GetCurrentRole()
        {
            return _role;
        }

        /// <summary>
        /// role 초기화
        /// </summary>
        public BootpayCommerceObject ClearRole()
        {
            _role = "user";
            return this;
        }

        /// <summary>
        /// HTTP 요청 전송
        /// </summary>
        public async Task<HttpResponseMessage> SendAsync(string url, HttpMethod method, object data = null)
        {
            string json = "";
            if (data != null)
            {
                json = JsonConvert.SerializeObject(data,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });
            }

            using (HttpRequestMessage request = new HttpRequestMessage())
            using (HttpClient client = new HttpClient())
            {
                request.Method = method;
                request.RequestUri = new Uri(_baseUrl + url);

                if (!string.IsNullOrEmpty(json) && method != HttpMethod.Get && method != HttpMethod.Delete)
                {
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                if (!string.IsNullOrEmpty(_token))
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
                }

                client.DefaultRequestHeaders.Add("BOOTPAY-SDK-VERSION", SDK_VERSION);
                client.DefaultRequestHeaders.Add("BOOTPAY-API-VERSION", API_VERSION);
                client.DefaultRequestHeaders.Add("BOOTPAY-SDK-TYPE", SDK_TYPE);
                client.DefaultRequestHeaders.Add("BOOTPAY-ROLE", _role ?? "user");

                return await client.SendAsync(request);
            }
        }

        /// <summary>
        /// Basic Auth를 사용한 HTTP 요청 전송
        /// </summary>
        private async Task<HttpResponseMessage> SendWithBasicAuthAsync(string url, HttpMethod method, string json = "")
        {
            using (HttpRequestMessage request = new HttpRequestMessage())
            using (HttpClient client = new HttpClient())
            {
                request.Method = method;
                request.RequestUri = new Uri(_baseUrl + url);

                if (!string.IsNullOrEmpty(json))
                {
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                // Basic Auth 설정
                var credentials = $"{_clientKey}:{_secretKey}";
                var encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {encodedCredentials}");

                client.DefaultRequestHeaders.Add("BOOTPAY-SDK-VERSION", SDK_VERSION);
                client.DefaultRequestHeaders.Add("BOOTPAY-API-VERSION", API_VERSION);
                client.DefaultRequestHeaders.Add("BOOTPAY-SDK-TYPE", SDK_TYPE);
                client.DefaultRequestHeaders.Add("BOOTPAY-ROLE", _role ?? "user");

                return await client.SendAsync(request);
            }
        }

        /// <summary>
        /// Multipart/form-data HTTP 요청 전송 (파일 업로드용)
        /// </summary>
        public async Task<HttpResponseMessage> SendMultipartAsync(string url, object data, List<string> imagePaths = null)
        {
            using (HttpClient client = new HttpClient())
            using (var content = new MultipartFormDataContent())
            {
                // 객체 필드를 form 데이터로 변환
                if (data != null)
                {
                    var properties = data.GetType().GetProperties();
                    foreach (var prop in properties)
                    {
                        var value = prop.GetValue(data);
                        if (value != null)
                        {
                            var jsonProp = prop.GetCustomAttributes(typeof(JsonPropertyAttribute), false);
                            string fieldName = jsonProp.Length > 0
                                ? ((JsonPropertyAttribute)jsonProp[0]).PropertyName
                                : prop.Name;

                            if (value is System.Collections.IEnumerable && !(value is string))
                            {
                                content.Add(new StringContent(JsonConvert.SerializeObject(value)), fieldName);
                            }
                            else if (value.GetType().IsClass && value.GetType() != typeof(string))
                            {
                                content.Add(new StringContent(JsonConvert.SerializeObject(value)), fieldName);
                            }
                            else
                            {
                                content.Add(new StringContent(value.ToString()), fieldName);
                            }
                        }
                    }
                }

                // 이미지 파일 추가
                if (imagePaths != null && imagePaths.Count > 0)
                {
                    foreach (var imagePath in imagePaths)
                    {
                        if (File.Exists(imagePath))
                        {
                            var fileName = Path.GetFileName(imagePath);
                            var fileContent = new ByteArrayContent(File.ReadAllBytes(imagePath));
                            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(GetMimeType(imagePath));
                            content.Add(fileContent, "images", fileName);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(_token))
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_token}");
                }

                client.DefaultRequestHeaders.Add("BOOTPAY-SDK-VERSION", SDK_VERSION);
                client.DefaultRequestHeaders.Add("BOOTPAY-API-VERSION", API_VERSION);
                client.DefaultRequestHeaders.Add("BOOTPAY-SDK-TYPE", SDK_TYPE);
                client.DefaultRequestHeaders.Add("BOOTPAY-ROLE", _role ?? "user");

                return await client.PostAsync(_baseUrl + url, content);
            }
        }

        /// <summary>
        /// 파일 확장자로 MIME 타입 추론
        /// </summary>
        private string GetMimeType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".webp":
                    return "image/webp";
                case ".bmp":
                    return "image/bmp";
                default:
                    return "application/octet-stream";
            }
        }
    }
}
