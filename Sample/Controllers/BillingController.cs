using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bootpay;
using Bootpay.models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sample.Models;

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
            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            var res = await api.GetAccessToken(); 
            var resKey = await api.getBillingKey(subscribe);
            
            var jsonString = System.Text.Json.JsonSerializer.Serialize(resKey);
            return Ok(jsonString);
        }
    }
}
