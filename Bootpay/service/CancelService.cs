using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.models;
using Newtonsoft.Json;

namespace Bootpay.service
{
    public class CancelService
    {
        public static async Task<ResDefault> ReceiptCancel(BootpayObject bootpay, Cancel cancel)
        {
            string json = JsonConvert.SerializeObject(cancel,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            return await bootpay.SendAsync<ResDefault>("cancel.json", HttpMethod.Post, json);
        }
    }
}
