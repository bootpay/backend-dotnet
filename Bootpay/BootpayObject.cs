﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Bootpay.models;
using Newtonsoft.Json;

namespace Bootpay
{
    public class BootpayObject
    {
        protected string _applicationId;
        protected string _privateKey;
        protected string _baseUrl;
        private string _token;


        public const int MODE_DEVELOPMENT = 0;
        public const int MODE_TEST = 1;
        public const int MODE_STAGE = 2;
        public const int MODE_PRODUCTION = 3;


        private readonly Dictionary<int, string> _URL = new Dictionary<int, string>()
        {
            { MODE_DEVELOPMENT, "https://dev-api.bootpay.co.kr/" },
            { MODE_TEST, "https://test-api.bootpay.co.kr/" },
            { MODE_STAGE, "https://stage-api.bootpay.co.kr/" },
            { MODE_PRODUCTION, "https://api.bootpay.co.kr/" },
        };

        public BootpayObject(string applicationId, string privateKey, int mode = MODE_PRODUCTION)
        {
            _applicationId = applicationId;
            _privateKey = privateKey;
            _baseUrl = _URL[mode];
        }

        public async Task<ResToken> GetAccessToken()
        { 
            Token token = new Token()
            {
                applicationId = _applicationId,
                privateKey = _privateKey
            }; 

            var res = await SendAsync<ResToken>("request/token", HttpMethod.Post, System.Text.Json.JsonSerializer.Serialize(token));
            _token = res.data.token;
            return res;
        }
         
        public async Task<TRes> SendAsync<TRes>(string url, HttpMethod method, string json = "")
        { 
            using (HttpRequestMessage request = new HttpRequestMessage())
            using (HttpClient client = new HttpClient())
            {                     
                request.Method = method;
                request.RequestUri = new Uri(_baseUrl + url);
                if (json.Length > 0) {
                    request.Content = new StringContent(json, Encoding.UTF8, "application/json");  
                }
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "1234");

                

                if (_token != null && _token.Length > 0) { client.DefaultRequestHeaders.Add("Authorization", _token); }
                
                var res = await client.SendAsync(request);
                string resJson = await res.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TRes>(resJson);
            }
        }
    }
}
