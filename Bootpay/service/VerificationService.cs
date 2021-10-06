using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.models;

namespace Bootpay.service
{
    public class VerificationService
    {
        /**
         * 
         * 
        ## 2. 결제 검증 
        결제창 및 정기결제에서 승인/취소된 결제건에 대하여 올바른 결제건인지 서버간 통신으로 결제검증을 합니다.
         */

        public static async Task<ResDefault> Verify(BootpayObject bootpay, string receiptId)
        {
            return await bootpay.SendAsync<ResDefault>("receipt/" + receiptId + ".json", HttpMethod.Get);
        }


        /**
         * 
         * 
        ## 8. 본인 인증 결과 조회 
        다날 본인인증 후 결과값을 조회합니다. 
        다날 본인인증에서 통신사, 외국인여부, 전화번호 이 3가지 정보는 다날에 추가로 요청하셔야 받으실 수 있습니다.
         */
        public static async Task<ResDefault> Certificate(BootpayObject bootpay, string receiptId)
        {
            return await bootpay.SendAsync<ResDefault>("certificate/" + receiptId + ".json", HttpMethod.Get);
        }
    }
}
