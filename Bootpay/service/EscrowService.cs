using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.models;
using Bootpay.models.response;
using Newtonsoft.Json;

namespace Bootpay.service
{
    public class EscrowService
    {
        public static async Task<HttpResponseMessage> PutShippingStart(BootpayObject bootpay, Shipping shipping)
        {
             
            string json = JsonConvert.SerializeObject(shipping,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            }); 
            return await bootpay.SendAsync("escrow/shipping/start/" + shipping.receiptId, HttpMethod.Put, json);
        } 
    }
}
