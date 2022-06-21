using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.models;
using Newtonsoft.Json;

namespace Bootpay.service
{
    public class ConfirmService
    {
        public static async Task<HttpResponseMessage> Confirm(BootpayObject bootpay, string receiptId)
        {
            Confirm submit = new Confirm()
            {
                receiptId = receiptId
            };

            string json = JsonConvert.SerializeObject(submit,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            return await bootpay.SendAsync("confirm.json", HttpMethod.Post, json);
        }
    }
}
