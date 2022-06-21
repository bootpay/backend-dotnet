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
        //2. 결제 검증 
        [HttpGet("verification/verify")]
        public async Task<IActionResult> Verify()
        {
            string receiptId = "62b12ece6262500007629fe6";

            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.GetReceipt(receiptId);

            string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });


            return Ok(json);
        }

        // 8. 본인 인증 결과 조회 
        [HttpGet("verification/certificate")]
        public async Task<IActionResult> Certificate()
        {
            string receiptId = "62b12f4b6262500007629fec";

            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.Certificate(receiptId);

            string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });


            return Ok(json);
        }
    }
}
