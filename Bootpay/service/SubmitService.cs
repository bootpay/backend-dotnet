using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.models;
using Newtonsoft.Json;

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

            string json = JsonConvert.SerializeObject(submit,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });

            return await bootpay.SendAsync<ResDefault>("submit.json", HttpMethod.Post, json);
        }
    }
}
