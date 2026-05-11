using System;
using System.Net;
using System.Net.Http; 
using System.Threading.Tasks;
using Bootpay.models;
using Bootpay.models.response;
using Bootpay.service;

namespace Bootpay
{
    /// <summary>
    /// Bootpay PG API client.
    /// </summary>
    public class BootpayApi : BootpayObject
    {
        /// <summary>
        /// Creates a Bootpay PG API client using legacy application_id/private_key credentials.
        /// </summary>
        /// <param name="applicationId">Legacy application_id credential.</param>
        /// <param name="privateKey">Legacy private_key credential.</param>
        /// <param name="mode">Bootpay environment mode.</param>
        public BootpayApi(string applicationId, string privateKey, int mode = MODE_PRODUCTION) : base(applicationId: applicationId, privateKey: privateKey, mode: mode) { }

        private BootpayApi(string clientKey, string secretKey, int mode, bool useClientKey) : base(clientKey: clientKey, secretKey: secretKey, mode: mode) { }

        /// <summary>
        /// Creates a Bootpay PG API client using client_key/secret_key credentials.
        /// </summary>
        /// <remarks>
        /// Prefer this factory for new integrations. The legacy constructor remains supported for existing users.
        /// </remarks>
        /// <param name="clientKey">PG client_key credential.</param>
        /// <param name="secretKey">PG secret_key credential.</param>
        /// <param name="mode">Bootpay environment mode.</param>
        /// <returns>A Bootpay PG API client configured with client_key/secret_key credentials.</returns>
        public static BootpayApi WithClientKey(string clientKey, string secretKey, int mode = MODE_PRODUCTION)
        {
            return new BootpayApi(clientKey, secretKey, mode, true);
        }

     


        /* billing */
        public async Task<HttpResponseMessage> GetBillingKey(Subscribe subsribe) {
            return await BillingService.GetBillingKey(this, subsribe);
        }


        public async Task<HttpResponseMessage> LookupBillingKey(String receiptId)
        {
            return await BillingService.LookupBillingKey(this, receiptId);
        }

        public async Task<HttpResponseMessage> LookupBillingKeyByKey(String billingKey)
        {
            return await BillingService.LookupBillingKeyByKey(this, billingKey);
        }

        

        public async Task<HttpResponseMessage> DestroyBillingKey(String billing_key) {
            return await BillingService.DestroyBillingKey(this, billing_key);
        }

        public async Task<HttpResponseMessage> RequestSubscribe(SubscribePayload payload) {
            return await BillingService.RequestSubscribe(this, payload);
        }

        public async Task<HttpResponseMessage> ReserveSubscribe(SubscribePayload payload) {
            return await BillingService.ReserveSubscribe(this, payload);
        }

        public async Task<HttpResponseMessage> ReserveCancelSubscribe(string reserveId) {
            return await BillingService.ReserveCancelSubscribe(this, reserveId);
        }

        /* cancel */
        public async Task<HttpResponseMessage> ReceiptCancel(Cancel cancel) {
            return await CancelService.ReceiptCancel(this, cancel);
        }

        /* easy */
        public async Task<HttpResponseMessage> GetUserToken(UserToken userToken) {
            return await EasyService.GetUserToken(this, userToken);
        }

        /* link */
        public async Task<HttpResponseMessage> RequestPayment(Payload paylod)
        {
            return await LinkService.RequestPayment(this, paylod);
        }

        /* submit */
        public async Task<HttpResponseMessage> Confirm(string receiptId)
        {
            return await ConfirmService.Confirm(this, receiptId);
        }

        /* verification */
        public async Task<HttpResponseMessage> GetReceipt(string receiptId)
        {
            return await VerificationService.GetReceipt(this, receiptId);
        }

        public async Task<HttpResponseMessage> Certificate(string receiptId)
        {
            return await VerificationService.Certificate(this, receiptId);
        }

        public async Task<HttpResponseMessage> PutShippingStart(Shipping shipping)
        {
            return await EscrowService.PutShippingStart(this, shipping);
        }

        public async Task<HttpResponseMessage> RequestCashReceiptByBootpay(CashReceipt cashReceipt)
        {
            return await CashReceiptService.RequestCashReceiptByBootpay(this, cashReceipt);
        }

        public async Task<HttpResponseMessage> RequestCashReceiptCancelByBootpay(Cancel cancel)
        {
            return await CashReceiptService.RequestCashReceiptCancelByBootpay(this, cancel);
        }

        public async Task<HttpResponseMessage> RequestCashReceipt(CashReceipt cashReceipt)
        {
            return await CashReceiptService.RequestCashReceipt(this, cashReceipt);
        }

        public async Task<HttpResponseMessage> RequestCashReceiptCancel(Cancel cancel)
        {
            return await CashReceiptService.RequestCashReceiptCancel(this, cancel);
        }

        public async Task<HttpResponseMessage> RequestAuthentication(Authentication authentication)
        {
            return await AuthService.RequestAuthentication(this, authentication);
        }

        public async Task<HttpResponseMessage> ConfirmAuthentication(AuthenticationParams authParams)
        {
            return await AuthService.ConfirmAuthentication(this, authParams);
        }

        public async Task<HttpResponseMessage> RealarmAuthentication(AuthenticationParams authParams)
        {
            return await AuthService.RealarmAuthentication(this, authParams);
        }

        public async Task<HttpResponseMessage> ReserveSubscribeLookup(string reserveId)
        {
            return await BillingService.ReserveSubscribeLookup(this, reserveId);
        }
        

        public async Task<HttpResponseMessage> GetBillingKeyTransfer(Subscribe subscribe)
        {
            return await BillingService.GetBillingKeyTransfer(this, subscribe);
        }

        public async Task<HttpResponseMessage> PublishBillingKeyTransfer(string receiptId)
        {
            return await BillingService.PublishBillingKeyTransfer(this, receiptId);
        }

        /* wallet */
        /// <summary>
        /// 사용자 지갑 목록 조회.
        /// </summary>
        [Obsolete("다음 메이저 버전에서 제거 예정. wallet 엔드포인트는 폐기 예정이며, 결제는 Request::PaymentController#create 의 wallet_id + user_token 으로 처리됩니다.")]
        public async Task<HttpResponseMessage> GetUserWallets(string userId, bool sandbox)
        {
            return await WalletService.GetUserWallets(this, userId, sandbox);
        }

    }
}
