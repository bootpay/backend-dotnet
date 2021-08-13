using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.models;

namespace Bootpay.service
{
    public class CancelService
    {
        public static async Task<ResDefault> ReceiptCancel(BootpayObject bootpay, Cancel cancel)
        {
            return await bootpay.SendAsync<ResDefault>("cancel", HttpMethod.Post, System.Text.Json.JsonSerializer.Serialize(cancel));
        }
    }
}
