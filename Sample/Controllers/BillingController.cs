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
            await api.GetAccessToken();
            var res = await api.getBillingKey(subscribe);

            string json = JsonConvert.SerializeObject(res,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

            return Ok(json);
        }

        [HttpDelete("billing/destroy_billing_key")]
        public async Task<IActionResult> DestroyBillingKey(string billingKeyID)
        {
            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.destroyBillingKey(billingKeyID);

            string json = JsonConvert.SerializeObject(res,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });


            return Ok(json);
        }

        [HttpPost("billing/request_subscribe")]
        public async Task<IActionResult> RequestSubscribe(SubscribePayload payload)
        {
            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.requestSubscribe(payload);

            string json = JsonConvert.SerializeObject(res,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

            return Ok(json);
        }

        [HttpPost("billing/reserve_subscribe")]
        public async Task<IActionResult> ReserveSubscribe(SubscribePayload payload)
        {
            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.reserveSubscribe(payload);

            string json = JsonConvert.SerializeObject(res,
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

            return Ok(json);
        }

        [HttpDelete("billing/reserve_cancel_subscribe")]
        public async Task<IActionResult> ReserveCancelSubscribe(string reserveId)
        {
            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.reserveCancelSubscribe(reserveId);

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
