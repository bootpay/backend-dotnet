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

        public void Print() {
            Console.WriteLine("applicationId: " + _baseUrl);
        }

        //billing
        public async Task<ResBillingKey> getBillingKey(Subscribe subsribe) {
            return await BillingService.GetBillingKey(this, subsribe);
        }

        //public async Task<HttpResponseMessage> requestSubscribe(SubscribePayload payload)  {
        //    return BillingService.requestSubscribe(this, payload);
        //}
        //public async Task<HttpResponseMessage> reserveSubscribe(SubscribePayload payload) throws Exception
        //{
        //return BillingService.reserveSubscribe(this, payload);
        //}
        //public HttpResponse reserveCancelSubscribe(String reserve_id) throws Exception
        //{
        //return BillingService.reserveCancelSubscribe(this, reserve_id);
        //}
        //public HttpResponse destroyBillingKey(String billing_key) throws Exception
        //{
        //return BillingService.destroyBillingKey(this, billing_key);
        //}
    }
}
