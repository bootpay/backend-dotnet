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
            //Subscribe subscribe = new Subscribe();
            //subscribe.orderName = "정기결제 테스트 아이템";
            //subscribe.subscriptionId = "" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            //subscribe.pg = "nicepay";

            //subscribe.cardNo = "5570**********1074"; //실제 테스트시에는 *** 마스크처리가 아닌 숫자여야 함
            //subscribe.cardPw = "**"; //실제 테스트시에는 *** 마스크처리가 아닌 숫자여야 함
            //subscribe.cardExpireYear = "**"; //실제 테스트시에는 *** 마스크처리가 아닌 숫자여야 함
            //subscribe.cardExpireMonth = "**"; //실제 테스트시에는 *** 마스크처리가 아닌 숫자여야 함
            //subscribe.cardIdentityNo = ""; //주민등록번호 또는 사업자 등록번호 (- 없이 입력)

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
