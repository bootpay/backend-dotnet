using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bootpay;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sample.Models;

namespace Sample.Controllers
{
    public class ConfirmController : Controller
    {
        // 7. 서버 승인 요청 
        [HttpGet("confirm")]
        public async Task<IActionResult> submit()
        {
            string receiptId = "62b13138d01c7e001bbc71d9";

            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.Confirm(receiptId);

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
