using System;

using System.Threading.Tasks;
using Bootpay;
using Bootpay.models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sample.Models;

namespace Sample.Controllers
{
    public class AuthController : Controller
    {
        //2. 결제 검증 
        [HttpGet("auth/request")]
        public async Task<IActionResult> RequestAuth()
        { 

            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();

            Authentication authentication = new Authentication();
            authentication.pg = "다날";
            authentication.method = "본인인증";
            authentication.carrier = "SKT"; //통신사
            //authentication.username = "사용자명";
            //authentication.identityNo = "0000000"; //생년월일 + 주민번호 뒷 1자리
            //authentication.phone = "01010002000"; //사용자 전화번호
            authentication.username = "윤태섭";
            authentication.identityNo = "8610141"; //생년월일 + 주민번호 뒷 1자리
            authentication.phone = "01040334678"; //사용자 전화번호
            authentication.siteUrl = "https://www.bootpay.co.kr"; //본인인증 하는 url 또는 App 명
            authentication.orderName = "회원 본인인증";
            authentication.authenticationId = "" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();


            var res = await api.RequestAuthentication(authentication);

            string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });


            return Ok(json);
        }

        // 8. 본인 인증 결과 조회 
        [HttpGet("auth/confirm")]
        public async Task<IActionResult> Confirm()
        {

            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();

            AuthenticationParams authParams = new AuthenticationParams();
            authParams.receiptId = "636a0bc4d01c7e00331cd25e";
            authParams.otp = "556659";

            var res = await api.ConfirmAuthentication(authParams); 

            string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });


            return Ok(json);
        }
         
        [HttpGet("auth/realarm")]
        public async Task<IActionResult> Realarm()
        { 
            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();

            AuthenticationParams authParams = new AuthenticationParams();
            authParams.receiptId = "636a0bc4d01c7e00331cd25e";


            var res = await api.RealarmAuthentication(authParams);

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

 