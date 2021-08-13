using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.models;

namespace Bootpay.service
{
    public class VerificationService
    {

        public static async Task<ResDefault> Verify(BootpayObject bootpay, string receiptId)
        {
            return await bootpay.SendAsync<ResDefault>("receipt/" + receiptId, HttpMethod.Get);
        }

        public static async Task<ResDefault> Certificate(BootpayObject bootpay, string receiptId)
        { 
            return await bootpay.SendAsync<ResDefault>("certificate/" + receiptId, HttpMethod.Get);
        }
    }
}
