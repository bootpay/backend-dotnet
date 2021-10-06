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
        public async Task<ResBillingKey> getBillingKey(Subscribe subsribe) {
            return await BillingService.GetBillingKey(this, subsribe);
        }

        public async Task<ResDefault> destroyBillingKey(String billing_key) {
            return await BillingService.DestroyBillingKey(this, billing_key);
        }

        public async Task<ResDefault> requestSubscribe(SubscribePayload payload) {
            return await BillingService.RequestSubscribe(this, payload);
        }

        public async Task<ResDefault> reserveSubscribe(SubscribePayload payload) {
            return await BillingService.ReserveSubscribe(this, payload);
        }

        public async Task<ResDefault> reserveCancelSubscribe(string reserveId) {
            return await BillingService.ReserveCancelSubscribe(this, reserveId);
        }

        /* cancel */
        public async Task<ResDefault> receiptCancel(Cancel cancel) {
            return await CancelService.ReceiptCancel(this, cancel);
        }

        /* easy */
        public async Task<ResEasy> getUserToken(UserToken userToken) {
            return await EasyService.GetUserToken(this, userToken);
        }

        /* link */
        public async Task<ResLink> requestPayment(Payload paylod)
        {
            return await LinkService.RequestPayment(this, paylod);
        }

        /* submit */
        public async Task<ResDefault> Submit(string receiptId)
        {
            return await SubmitService.Submit(this, receiptId);
        }

        /* verification */
        public async Task<ResDefault> Verify(string receiptId)
        {
            return await VerificationService.Verify(this, receiptId);
        }

        public async Task<ResDefault> Certificate(string receiptId)
        {
            return await VerificationService.Certificate(this, receiptId);
        }
    }
}
