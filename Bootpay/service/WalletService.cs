using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.models;
using Newtonsoft.Json;

namespace Bootpay.service
{
    public class WalletService
    {
        public static async Task<HttpResponseMessage> GetUserWallets(BootpayObject bootpay, string userId, bool sandbox)
        {
            string sandboxStr = sandbox ? "true" : "false";
            return await bootpay.SendAsync("wallet?user_id=" + userId + "&sandbox=" + sandboxStr, HttpMethod.Get);
        }

        public static async Task<HttpResponseMessage> RequestWalletPayment(BootpayObject bootpay, WalletRequest walletRequest)
        {
            string json = JsonConvert.SerializeObject(walletRequest,
                            Newtonsoft.Json.Formatting.None,
                            new JsonSerializerSettings
                            {
                                NullValueHandling = NullValueHandling.Ignore
                            });
            return await bootpay.SendAsync("wallet/payment", HttpMethod.Post, json);
        }
    }
}
