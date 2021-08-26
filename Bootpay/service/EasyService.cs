using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.models;
using Bootpay.models.response;
using Newtonsoft.Json;

namespace Bootpay.service
{
    public class EasyService
    {
        public static async Task<ResEasy> GetUserToken(BootpayObject bootpay, UserToken userToken)
        {
            string json = JsonConvert.SerializeObject(userToken,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            return await bootpay.SendAsync<ResEasy>("request/user/token.json", HttpMethod.Post, json);
        }
    }
}
