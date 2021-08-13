using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.models;

namespace Bootpay.service
{
    public class SubmitService
    {
        public static async Task<ResDefault> Submit(BootpayObject bootpay, string receiptId)
        {
            Submit submit = new Submit()
            {
                receiptId = receiptId
            };
            return await bootpay.SendAsync<ResDefault>("submit", HttpMethod.Post, System.Text.Json.JsonSerializer.Serialize(submit));
        }
    }
}
