using System;
using System.Net;
using System.Net.Http; 
using System.Threading.Tasks;
using Bootpay.models;
using Bootpay.models.response;
using Bootpay.service;

namespace Bootpay
{
    public class BootpayApi : BootpayObject
    {

        public BootpayApi(string applicationId, string privateKey, int mode = MODE_PRODUCTION) : base(applicationId, privateKey, mode) { }

     


        /* billing */
        public async Task<HttpResponseMessage> GetBillingKey(Subscribe subsribe) {
            return await BillingService.GetBillingKey(this, subsribe);
        }


        public async Task<HttpResponseMessage> LookupBillingKey(String receiptId)
        {
            return await BillingService.LookupBillingKey(this, receiptId);
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
    }
}
