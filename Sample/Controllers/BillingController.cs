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
        public async Task<IActionResult> IndexAsync()
        {
            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            var res = await api.GetAccessToken();
            string json = JsonConvert.SerializeObject(res,
                 Newtonsoft.Json.Formatting.None,
                 new JsonSerializerSettings
                 {
                     NullValueHandling = NullValueHandling.Ignore
                 });

            return Ok(json);
        }

        [HttpGet("billing/lookup")]
        public async Task<IActionResult> LookupBillingKey()
        {
            string receiptId = "62b3cbbecf9f6d001bd20ce8";

            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.LookupBillingKey(receiptId);

            string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });


            return Ok(json);
        }

        [HttpGet("billing/lookup_by_key")]
        public async Task<IActionResult> LookupBillingKeyByKey()
        {
            string billingKey = "66542dfb4d18d5fc7b43e1b6";

            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.LookupBillingKeyByKey(billingKey);

            string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });


            return Ok(json);
        }

        // 4. 빌링키 발급 
        [HttpGet("billing/get_billing_key")]
        public async Task<IActionResult> GetBillingKey()
        {
            Subscribe subscribe = new Subscribe();
            subscribe.orderName = "정기결제 테스트 아이템";
            subscribe.subscriptionId = "" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            subscribe.pg = "nicepay";

            subscribe.cardNo = "5570**********1074"; //실제 테스트시에는 *** 마스크처리가 아닌 숫자여야 함
            subscribe.cardPw = "**"; //실제 테스트시에는 *** 마스크처리가 아닌 숫자여야 함
            subscribe.cardExpireYear = "**"; //실제 테스트시에는 *** 마스크처리가 아닌 숫자여야 함
            subscribe.cardExpireMonth = "**"; //실제 테스트시에는 *** 마스크처리가 아닌 숫자여야 함
            subscribe.cardIdentityNo = ""; //주민등록번호 또는 사업자 등록번호 (- 없이 입력) 

            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.GetBillingKey(subscribe);
             

            string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
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
            payload.billingKey = "62b12d7fd01c7e001ebc71de";
            payload.orderName = "아이템01";
            payload.price = 1000;
            payload.orderId = "" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            payload.user = new User();
            payload.user.phone = "01012345678";


            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.RequestSubscribe(payload);

            string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
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
            payload.billingKey = "62b12d7fd01c7e001ebc71de";
            payload.orderName = "아이템01";
            payload.price = 1000;
            payload.orderId = "" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            payload.reserveExecuteAt = DateTime.Now.AddSeconds(100).ToString("yyyy-MM-dd'T'HH:mm:ss zzz");

            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.ReserveSubscribe(payload);

            string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

            return Ok(json);
        }

         // 예약 결제 조회
        [HttpGet("billing/reserve_lookup")]
        public async Task<IActionResult> ReserveLookup()
        {
            string reserveId = "6490149ca575b40024f0b70d";

            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.ReserveSubscribeLookup(reserveId);

            string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

            return Ok(json);
        }


// ReserveSubscribeLookup

        // 4-2-1. 발급된 빌링키로 결제 예약 - 취소 요청 
        [HttpGet("billing/reserve_cancel_subscribe")]
        public async Task<IActionResult> ReserveCancelSubscribe()
        {
            string reserveId = "62b12ed4d01c7e001dbc71e5";

            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.ReserveCancelSubscribe(reserveId);

            string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
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
            string billingKey = "62b12d7fd01c7e001ebc71de";

            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.DestroyBillingKey(billingKey);

            string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });


            return Ok(json);
        }

        //계좌 빌링키 발급 요청 
        [HttpGet("billing/get_billing_key_transfer")]
        public async Task<IActionResult> GetBillingKeyTransfer()
        {
            Subscribe subscribe = new Subscribe();
            subscribe.orderName = "정기결제 테스트 아이템";
            subscribe.subscriptionId = "" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            subscribe.pg = "nicepay";

            subscribe.username = "홍길동";
            subscribe.bankName = "국민";
            subscribe.identityNo = "901014"; 
            subscribe.phone = "01012341234"; 
            subscribe.bankAccount = "67560123422472";
            

            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.GetBillingKeyTransfer(subscribe);

            string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });


            return Ok(json);
        }

        //계좌 출금동의 요청 확인 
        [HttpGet("billing/publish_transfer_billing_key")]
        public async Task<IActionResult> publishBillingKeyTransfer()
        { 
            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.PublishBillingKeyTransfer("6655e139d79bea0da31c05e5");

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
