using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bootpay;
using Bootpay.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sample.Models;


namespace Sample.Controllers
{

    public class EscrowController : Controller
    {
        // 1. 토큰 발급 
        [HttpGet("escrow")]
        public async Task<IActionResult> IndexAsync()
        { 
            Shipping shipping = new Shipping();
            shipping.trackingNumber = "214234";
            shipping.receiptId = "62b12f4b6262500007629fec";

            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.PutShippingStart(shipping);

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
