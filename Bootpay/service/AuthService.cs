using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.models;
using Bootpay.models.response;
using Newtonsoft.Json;

namespace Bootpay.service
{
    public class AuthService
    {
        public static async Task<HttpResponseMessage> RequestAuthentication(BootpayObject bootpay, Authentication authentication)
        {
            string json = JsonConvert.SerializeObject(authentication,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            return await bootpay.SendAsync("request/authentication.json", HttpMethod.Post, json);
        }

        public static async Task<HttpResponseMessage> ConfirmAuthentication(BootpayObject bootpay, AuthenticationParams authParams)
        {
            string json = JsonConvert.SerializeObject(authParams,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            return await bootpay.SendAsync("authenticate/confirm.json", HttpMethod.Post, json);
        }

        public static async Task<HttpResponseMessage> RealarmAuthentication(BootpayObject bootpay, AuthenticationParams authParams)
        {
            string json = JsonConvert.SerializeObject(authParams,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            return await bootpay.SendAsync("authenticate/realarm.json", HttpMethod.Post, json);
        }
    }
}
