﻿using System;
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
    }
}
