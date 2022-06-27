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

    public class ShippingController : Controller
    {
        // 1. 토큰 발급 
        [HttpGet("shipping")]
        public async Task<IActionResult> IndexAsync()
        {
            Shipping shipping = new Shipping();
            shipping.receiptId = "628ae7ffd01c7e001e9b6066";
            shipping.trackingNumber = "123456";
            shipping.deliveryCorp = "CJ대한통운";
            ShippingUser user = new ShippingUser();
            user.username = "홍길동";
            user.phone = "01000000000";
            user.address = "서울특별시 종로구";
            user.zipcode = "08490";
            shipping.user = user;

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
