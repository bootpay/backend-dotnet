using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bootpay.service
{
    public class WalletService
    {
        [Obsolete("다음 메이저 버전에서 제거 예정.")]
        public static async Task<HttpResponseMessage> GetUserWallets(BootpayObject bootpay, string userId, bool sandbox)
        {
            string sandboxStr = sandbox ? "true" : "false";
            return await bootpay.SendAsync("wallet?user_id=" + userId + "&sandbox=" + sandboxStr, HttpMethod.Get);
        }
    }
}
