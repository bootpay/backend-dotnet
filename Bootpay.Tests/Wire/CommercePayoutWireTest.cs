using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bootpay.Commerce.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Bootpay.Tests.Wire
{
    public class CommercePayoutWireTest : IDisposable
    {
        private readonly MockServer _server = new MockServer();
        private readonly WireCommerceApi _api;

        public CommercePayoutWireTest()
        {
            _api = new WireCommerceApi(_server.BaseUrl + "v1/");
            _api.AsManager();
        }

        public void Dispose() => _server.Dispose();

        private async Task Check(Func<Task> call, string method, string path, JObject? body = null, string? key = null)
        {
            await call();
            var request = _server.LastRequest;
            Assert.Equal(method, request.Method);
            Assert.Equal("/v1/" + path, request.PathAndQuery);
            Assert.Equal("supervisor", request.Headers["BOOTPAY-ROLE"]);
            Assert.Equal("manager", _api.GetCurrentRole());
            if (key == null) Assert.True(Guid.TryParse(request.Headers["Idempotency-Key"], out _));
            else Assert.Equal(key, request.Headers["Idempotency-Key"]);
            if (body == null) Assert.True(string.IsNullOrEmpty(request.Body));
            else Assert.True(JToken.DeepEquals(body, JObject.Parse(request.Body)), request.Body);
        }

        [Fact]
        public async Task Payout_ThreeRoutes_ForceSupervisorAndPreserveRole()
        {
            await Check(() => _api.PayoutCreate(new PayoutCreateParams
            {
                SubSellerId = "ss-1",
                Amount = 150000,
                MerchantPayoutId = "mp-1",
                OrderNumber = null,
                PayoutDate = "2026-11-20",
                IdempotencyKey = "create-key"
            }), "POST", "sdk/payouts", JObject.Parse(@"{
                'sub_seller_id':'ss-1','amount':150000,'merchant_payout_id':'mp-1','payout_date':'2026-11-20'
            }"), "create-key");

            await Check(() => _api.PayoutDetail("mp-1", "detail-key"), "GET", "sdk/payouts/mp-1", key: "detail-key");

            await Check(() => _api.PayoutList(new PayoutListParams { IdempotencyKey = "list-key" }),
                "GET", "sdk/payouts", key: "list-key");
        }

        [Fact]
        public async Task Detail_EncodesPayoutIdLikeDeposit()
        {
            await Check(() => _api.PayoutDetail("a/b c?d#e한"), "GET", "sdk/payouts/a%2Fb%20c%3Fd%23e%ED%95%9C");
        }

        [Fact]
        public async Task List_SendsOnlySpecifiedFiltersWithoutDefaults()
        {
            await Check(() => _api.PayoutList(), "GET", "sdk/payouts");
            await Check(() => _api.PayoutList(new PayoutListParams
            {
                Status = PayoutStatus.Ready,
                SubSellerId = "ss-1",
                From = "2026-11-01",
                To = "2026-11-30",
                Page = 2,
                Limit = 50
            }), "GET", "sdk/payouts?status=ready&sub_seller_id=ss-1&from=2026-11-01&to=2026-11-30&page=2&limit=50");
        }

        [Fact]
        public async Task AutoIdempotencyKeys_AreFreshForEveryCall()
        {
            await Check(() => _api.PayoutCreate(new PayoutCreateParams
            {
                SubSellerId = "ss-1", Amount = 1, MerchantPayoutId = "mp-auto"
            }), "POST", "sdk/payouts", JObject.Parse("{'sub_seller_id':'ss-1','amount':1,'merchant_payout_id':'mp-auto'}"));
            await Check(() => _api.PayoutDetail("mp-auto"), "GET", "sdk/payouts/mp-auto");
            await Check(() => _api.PayoutList(), "GET", "sdk/payouts");

            var keys = new HashSet<string>();
            foreach (var request in _server.Requests) Assert.True(keys.Add(request.Headers["Idempotency-Key"]));
        }

        [Fact]
        public async Task ListResponse_DeserializesBareArray()
        {
            _server.ResponseBody = @"[{
                'id':'p-1','merchant_payout_id':'mp-1','sub_seller_id':'ss-1','amount':2147483648,
                'status':'succeeded','payout_date':'2026-11-20','order_number':null,
                'last_code':null,'last_message':null,
                'created_at':'2026-11-12T10:00:00+09:00','updated_at':'2026-11-12T10:05:00+09:00'
            }]";

            using var response = await _api.PayoutList();
            var payouts = JsonConvert.DeserializeObject<List<CommercePayout>>(await response.Content.ReadAsStringAsync());

            Assert.NotNull(payouts);
            Assert.Single(payouts);
            Assert.Equal("p-1", payouts[0].Id);
            Assert.Equal("mp-1", payouts[0].MerchantPayoutId);
            Assert.Equal("ss-1", payouts[0].SubSellerId);
            Assert.Equal(2147483648L, payouts[0].Amount); // int 범위를 넘는 지급액
            Assert.Equal(PayoutStatus.Succeeded, payouts[0].Status);
            Assert.Equal("2026-11-20", payouts[0].PayoutDate);
            Assert.Null(payouts[0].OrderNumber);
            Assert.Null(payouts[0].LastCode);
            Assert.Null(payouts[0].LastMessage);
        }
    }
}
