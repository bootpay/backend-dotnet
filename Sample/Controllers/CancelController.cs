using System;

using System.Threading.Tasks;
using Bootpay;
using Bootpay.models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sample.Models;

namespace Sample.Controllers
{
    public class CancelController : Controller
    {
        [HttpPost("cancel")]
        public async Task<IActionResult> ReceiptCancel([FromBody]Cancel cancel)
        {
            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.receiptCancel(cancel);

            string json = JsonConvert.SerializeObject(res,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });


            return Ok(json);
        }
    }
}
