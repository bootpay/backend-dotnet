using System;

using System.Threading.Tasks;
using Bootpay;
using Bootpay.models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sample.Models;

namespace Sample.Controllers
{ 
    public class CashController : Controller
    { 
        [HttpGet("cash/receipt_request")]
        public async Task<IActionResult> RequestCashReceipt()
        {

            CashReceipt cashReceipt = new CashReceipt();
            cashReceipt.pg = "토스";
            cashReceipt.price = 1000;
            cashReceipt.orderName = "테스트";
            cashReceipt.cashReceiptType = "소득공제";
            cashReceipt.identityNo = "01000000000";

            cashReceipt.orderId = "" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            cashReceipt.purchasedAt = DateTime.Now.AddSeconds(100).ToString("yyyy-MM-dd'T'HH:mm:ss zzz");


            //BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            BootpayApi api = new BootpayApi(Constants.dev_application_id, Constants.dev_private_key, BootpayObject.MODE_DEVELOPMENT);

            await api.GetAccessToken();


            var res = await api.RequestCashReceipt(cashReceipt);

            string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });


            return Ok(json);
        }

        [HttpGet("cash/receipt_request_cancel")]
        public async Task<IActionResult> RequestCashReceiptCancel()
        {
            Cancel cancel = new Cancel();
            cancel.receiptId = "62f5bbb91fc192036f9f4c05";
            cancel.cancelUsername = "관리자";
            cancel.cancelMessage = "테스트 결제";

            //cancel.price = 1000.0; //부분취소 요청시
            //cancel.cancelId = "12342134"; //부분취소 요청시, 중복 부분취소 요청하는 실수를 방지하고자 할때 지정
            //RefundData refund = new RefundData(); // 가상계좌 환불 요청시, 단 CMS 특약이 되어있어야만 환불요청이 가능하다.
            //refund.account = "675601012341234"; //환불계좌
            //refund.accountholder = "홍길동"; //환불계좌주
            //refund.bankcode = BankCode.getCode("국민은행");//은행코드
            //cancel.refund = refund;

            //BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            BootpayApi api = new BootpayApi(Constants.dev_application_id, Constants.dev_private_key, BootpayObject.MODE_DEVELOPMENT);
            var token = await api.GetAccessToken();


            var res = await api.RequestCashReceiptCancel(cancel);

            string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });


            return Ok(json);
        }

        [HttpGet("cash/receipt_request_bootpay")]
        public async Task<IActionResult> RequestCashReceiptByBootpay()
        {
            CashReceipt cashReceipt = new CashReceipt();
            cashReceipt.receiptId = "62e0f11f1fc192036b1b3c92";
            //cashReceipt.userna

            cashReceipt.username = "테스트";
            cashReceipt.email = "test@bootpay.co.kr";
            cashReceipt.phone = "01000000000";

            cashReceipt.identityNo = "01000000000";
            cashReceipt.cashReceiptType = "소득공제";


            //BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            BootpayApi api = new BootpayApi(Constants.dev_application_id, Constants.dev_private_key, BootpayObject.MODE_DEVELOPMENT);
            await api.GetAccessToken();


            var res = await api.RequestCashReceiptByBootpay(cashReceipt);

            string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
                    Newtonsoft.Json.Formatting.None,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });


            return Ok(json);
        }

        [HttpGet("cash/receipt_request_cancel_bootpay")]
        public async Task<IActionResult> RequestCashReceiptCancelByBootpay()
        {
            Cancel cancel = new Cancel();
            cancel.receiptId = "62e0f11f1fc192036b1b3c92";
            cancel.cancelUsername = "관리자";
            cancel.cancelMessage = "테스트 결제";

            //cancel.price = 1000.0; //부분취소 요청시
            //cancel.cancelId = "12342134"; //부분취소 요청시, 중복 부분취소 요청하는 실수를 방지하고자 할때 지정
            //RefundData refund = new RefundData(); // 가상계좌 환불 요청시, 단 CMS 특약이 되어있어야만 환불요청이 가능하다.
            //refund.account = "675601012341234"; //환불계좌
            //refund.accountholder = "홍길동"; //환불계좌주
            //refund.bankcode = BankCode.getCode("국민은행");//은행코드
            //cancel.refund = refund;

            //BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
            BootpayApi api = new BootpayApi(Constants.dev_application_id, Constants.dev_private_key, BootpayObject.MODE_DEVELOPMENT);
            var token = await api.GetAccessToken();


            var res = await api.RequestCashReceiptCancelByBootpay(cancel);

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
