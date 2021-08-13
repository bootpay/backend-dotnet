using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.models;

namespace Bootpay.service
{
    public class EasyService
    {
        public static async Task<ResDefault> GetUserToken(BootpayObject bootpay, UserToken userToken)
        {
            return await bootpay.SendAsync<ResDefault>("request/user/token", HttpMethod.Post, System.Text.Json.JsonSerializer.Serialize(userToken));
        }
    }
}
