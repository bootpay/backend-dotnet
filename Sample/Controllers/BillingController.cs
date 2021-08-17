using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bootpay;
using Bootpay.models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sample.Controllers
{
    public class BillingController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return Ok("1234");
        }

        [HttpPost("billing/get_billing_key")]
        public async Task<IActionResult> GetBillingKey(Subscribe subscribe)
        {
            BootpayApi api = new BootpayApi("5b8f6a4d396fa665fdc2b5ea", "rm6EYECr6aroQVG2ntW0A6LpWnkTgP4uQ3H18sDDUYw=");
            var res = await api.GetAccessToken();


            subscribe.orderId = "1234";

            var resKey = await api.getBillingKey(subscribe);
            var jsonString = System.Text.Json.JsonSerializer.Serialize(subscribe);
                //JsonConvert.SerializeObject(resKey);

            //subscribe.card

            return Ok(jsonString);
        }
    }
}
