using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.models;

namespace Bootpay.service
{
    public class LinkService
    {
        public static async Task<ResDefault> GetUserToken(BootpayObject bootpay, Payload payload)
        {
            return await bootpay.SendAsync<ResDefault>("request/payment", HttpMethod.Post, System.Text.Json.JsonSerializer.Serialize(payload));
        }
    }
}
