using System;

using System.Threading.Tasks;
using Bootpay;
using Bootpay.models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sample.Models;

namespace Sample.Controllers
{ 
    public class CancelController : Controller
    {
        //3. 결제 취소 (전액 취소 / 부분 취소)
        [HttpGet("cancel")]
        public async Task<IActionResult> ReceiptCancel()
        {
            Cancel cancel = new Cancel();
            cancel.receiptId = "6100e77a019943003650f4d5";
            cancel.name = "관리자";
            cancel.reason = "테스트 결제";

            //cancel.price = 1000.0; //부분취소 요청시
            //cancel.cancelId = "12342134"; //부분취소 요청시, 중복 부분취소 요청하는 실수를 방지하고자 할때 지정
            //RefundData refund = new RefundData(); // 가상계좌 환불 요청시, 단 CMS 특약이 되어있어야만 환불요청이 가능하다.
            //refund.account = "675601012341234"; //환불계좌
            //refund.accountholder = "홍길동"; //환불계좌주
            //refund.bankcode = BankCode.getCode("국민은행");//은행코드
            //cancel.refund = refund;

            BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            await api.GetAccessToken();
            var res = await api.receiptCancel(cancel);

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
