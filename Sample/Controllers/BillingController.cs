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

        // 4. 빌링키 발급 
        [HttpGet("billing/get_billing_key")]
        public async Task<IActionResult> GetBillingKey()
        {
            Subscribe subscribe = new Subscribe();
            subscribe.itemName = "정기결제 테스트 아이템";
            subscribe.orderId = "" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            subscribe.pg = "nicepay"; 

            subscribe.cardNo = "5570**********1074"; //실제 테스트시에는 *** 마스크처리가 아닌 숫자여야 함
            subscribe.cardPw = "**"; //실제 테스트시에는 *** 마스크처리가 아닌 숫자여야 함
            subscribe.expireYear = "**"; //실제 테스트시에는 *** 마스크처리가 아닌 숫자여야 함
            subscribe.expireMonth = "**"; //실제 테스트시에는 *** 마스크처리가 아닌 숫자여야 함
            subscribe.identifyNumber = ""; //주민등록번호 또는 사업자 등록번호 (- 없이 입력)

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


        // 4-1. 발급된 빌링키로 결제 승인 요청 
        [HttpGet("billing/request_subscribe")]
        public async Task<IActionResult> RequestSubscribe()
        {
            SubscribePayload payload = new SubscribePayload();
            payload.billingKey = "615d00f0238684001f60241e";
            payload.itemName = "아이템01";
            payload.price = 1000;
            payload.orderId = "" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();


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

        // 4-2. 발급된 빌링키로 결제 예약 요청
        [HttpGet("billing/reserve_subscribe")]
        public async Task<IActionResult> ReserveSubscribe()
        {
            SubscribePayload payload = new SubscribePayload();
            payload.billingKey = "615d00f0238684001f60241e";
            payload.itemName = "아이템01";
            payload.price = 1000;
            payload.orderId = "" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            payload.executeAt = (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000) + 10;

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


        // 4-2-1. 발급된 빌링키로 결제 예약 - 취소 요청 
        [HttpGet("billing/reserve_cancel_subscribe")]
        public async Task<IActionResult> ReserveCancelSubscribe()
        {
            string reserveId = "615d08a67b5ba4002011cd41";

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


        // 4-3. 빌링키 삭제         
        [HttpGet("billing/destroy_billing_key")]
        public async Task<IActionResult> DestroyBillingKey()
        {
            string billingKey = "615d00f0238684001f60241e";

            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.destroyBillingKey(billingKey);

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
