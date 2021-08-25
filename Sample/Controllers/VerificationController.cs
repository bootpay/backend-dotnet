using System;

using System.Threading.Tasks;
using Bootpay;
using Bootpay.models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sample.Models;

namespace Sample.Controllers
{
    public class VerificationController : Controller
    {
        [HttpGet("verification/verify")]
        public async Task<IActionResult> Verify(string receiptId)
        {
            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.Verify(receiptId);

            string json = JsonConvert.SerializeObject(res,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });


            return Ok(json);
        }

        [HttpGet("verification/certificate")]
        public async Task<IActionResult> Certificate(string receiptId)
        {
            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.Certificate(receiptId);

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
