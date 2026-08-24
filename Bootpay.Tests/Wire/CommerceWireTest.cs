using Bootpay.Commerce.Models;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Bootpay.Tests.Wire
{
    /// <summary>
    /// Commerce API wire-format 검증 (NodeJS 2.9.0 parity).
    /// 실제 네트워크 없이 mock 서버로 URL·헤더·바디 규약을 단정한다.
    /// </summary>
    public class CommerceWireTest : IDisposable
    {
        private readonly MockServer _server;
        private readonly WireCommerceApi _api;

        public CommerceWireTest()
        {
            _server = new MockServer();
            _api = new WireCommerceApi(_server.BaseUrl);
        }

        public void Dispose() => _server.Dispose();

        [Fact]
        public async Task MallSetting_Get_SendsSupervisorRoleAndIdempotencyKey()
        {
            await _api.GetMallSetting();
            var req = _server.LastRequest;

            Assert.Equal("GET", req.Method);
            Assert.Equal("/mall-setting", req.PathAndQuery);
            Assert.Equal("supervisor", req.Headers["BOOTPAY-ROLE"]);
            Assert.True(Guid.TryParse(req.Headers["Idempotency-Key"], out _), "Idempotency-Key must be auto-generated GUID");
        }

        [Fact]
        public async Task MallSetting_Update_FlattenBodyWithoutNulls_AndExplicitIdempotencyKey()
        {
            await _api.UpdateMallSetting(new MallSettingUpdateParams { Name = "My Mall", UsePoint = true }, "my-key-1");
            var req = _server.LastRequest;

            Assert.Equal("PUT", req.Method);
            Assert.Equal("/mall-setting", req.PathAndQuery);
            Assert.Equal("supervisor", req.Headers["BOOTPAY-ROLE"]);
            Assert.Equal("my-key-1", req.Headers["Idempotency-Key"]);

            var body = JObject.Parse(req.Body);
            Assert.Equal("My Mall", (string?)body["name"]);
            Assert.True((bool?)body["use_point"]);
            Assert.Equal(2, body.Count); // null 값은 전송되지 않는다
        }

        [Fact]
        public async Task Webhook_SendTest_PostsHeaderContentType()
        {
            await _api.WebhookSendTest(new SendTestWebhookParams { HeaderContentType = 1 });
            var req = _server.LastRequest;

            Assert.Equal("POST", req.Method);
            Assert.Equal("/webhook/test", req.PathAndQuery);
            Assert.True(req.Headers.ContainsKey("Idempotency-Key"));
            Assert.Equal("user", req.Headers["BOOTPAY-ROLE"]); // 기본 role 유지

            var body = JObject.Parse(req.Body);
            Assert.Equal(1, (int?)body["header_content_type"]);
        }

        [Fact]
        public async Task SupervisorCharge_SendsChargeKeyInBodyOnly()
        {
            await _api.OrderSubscriptionSupervisorCharge(new SupervisorOrderSubscriptionChargeParams
            {
                ChargeKey = "ck_123",
                Price = 1000
            });
            var req = _server.LastRequest;

            Assert.Equal("POST", req.Method);
            Assert.Equal("/order_subscriptions/charge", req.PathAndQuery); // charge_key 는 URL/query 금지
            Assert.Equal("supervisor", req.Headers["BOOTPAY-ROLE"]);
            Assert.True(req.Headers.ContainsKey("Idempotency-Key"));

            var body = JObject.Parse(req.Body);
            Assert.Equal("ck_123", (string?)body["charge_key"]);
            Assert.Equal(1000, (int?)body["price"]);
        }

        [Fact]
        public async Task SupervisorChargeRevoke_SendsDeleteWithBody()
        {
            await _api.OrderSubscriptionSupervisorChargeRevoke(new SupervisorOrderSubscriptionChargeRevokeParams
            {
                ChargeKey = "ck_123"
            });
            var req = _server.LastRequest;

            Assert.Equal("DELETE", req.Method);
            Assert.Equal("/order_subscriptions/charge", req.PathAndQuery);
            Assert.Equal("supervisor", req.Headers["BOOTPAY-ROLE"]);

            var body = JObject.Parse(req.Body);
            Assert.Equal("ck_123", (string?)body["charge_key"]);
        }

        [Fact]
        public async Task UserLoginMall_PostsPasswordAndDefaultCorporateType()
        {
            await _api.UserLoginMall("hello", "pw123");
            var req = _server.LastRequest;

            Assert.Equal("POST", req.Method);
            Assert.Equal("/users/login", req.PathAndQuery);
            Assert.True(req.Headers.ContainsKey("Idempotency-Key"));

            var body = JObject.Parse(req.Body);
            Assert.Equal("hello", (string?)body["login_id"]);
            Assert.Equal("pw123", (string?)body["password"]); // login_pw 아님
            Assert.Equal(0, (int?)body["corporate_type"]); // 미지정시 0
        }

        [Fact]
        public async Task UserSession_AttachesUserJwtHeaderOnlyWhenPresent()
        {
            await _api.UserSession("jwt-abc");
            var withJwt = _server.LastRequest;
            Assert.Equal("GET", withJwt.Method);
            Assert.Equal("/users/session", withJwt.PathAndQuery);
            Assert.Equal("jwt-abc", withJwt.Headers["Bootpay-User-JWT"]);

            await _api.UserSession();
            var withoutJwt = _server.LastRequest;
            Assert.False(withoutJwt.Headers.ContainsKey("Bootpay-User-JWT"), "JWT 미지정시 헤더를 붙이지 않는다");
        }

        [Fact]
        public async Task UserLogout_SendsDeleteSessionWithJwt()
        {
            await _api.UserLogout("jwt-abc");
            var req = _server.LastRequest;

            Assert.Equal("DELETE", req.Method);
            Assert.Equal("/users/session", req.PathAndQuery);
            Assert.Equal("jwt-abc", req.Headers["Bootpay-User-JWT"]);
        }

        [Fact]
        public async Task UserJoinMall_CompactsNullsAndDefaultsCorporateType()
        {
            await _api.UserJoinMall(new MallUserJoinParams { LoginId = "hello", Password = "pw", Name = "홍길동" });
            var req = _server.LastRequest;

            Assert.Equal("POST", req.Method);
            Assert.Equal("/users/join", req.PathAndQuery);

            var body = JObject.Parse(req.Body);
            Assert.Equal("hello", (string?)body["login_id"]);
            Assert.Equal(0, (int?)body["corporate_type"]);
            Assert.False(body.ContainsKey("email"), "null 값은 전송하지 않는다");
        }

        [Fact]
        public async Task UserJoinCheckMall_And_UidExist_UsePluralUsersPath()
        {
            await _api.UserJoinCheckMall("email-exist", "a@b.com");
            var joinCheck = _server.LastRequest;
            Assert.Equal("GET", joinCheck.Method);
            Assert.Equal("/users/join/email-exist?pk=a%40b.com", joinCheck.PathAndQuery);

            await _api.UidExist("uid 123");
            var uidExist = _server.LastRequest;
            Assert.Equal("/users/join/uid-exist?pk=uid+123", uidExist.PathAndQuery);
            Assert.Equal("user", uidExist.Headers["BOOTPAY-ROLE"]);
        }

        [Fact]
        public async Task Products_Mall_DefaultsPageAndLimit_AndSupportsCategorySortJwt()
        {
            await _api.Products();
            var defaults = _server.LastRequest;
            Assert.Equal("/products?page=1&limit=20", defaults.PathAndQuery);
            Assert.True(defaults.Headers.ContainsKey("Idempotency-Key"));

            await _api.Products(new MallProductListParams { CategoryId = "cat1", Sort = "-created_at", UserJwt = "jwt-abc" });
            var mall = _server.LastRequest;
            Assert.Equal("/products?page=1&limit=20&category_id=cat1&sort=-created_at", mall.PathAndQuery);
            Assert.Equal("jwt-abc", mall.Headers["Bootpay-User-JWT"]);
        }

        [Fact]
        public async Task ProductDetailMall_AttachesJwtHeader()
        {
            await _api.ProductDetailMall("prod1", "jwt-abc");
            var req = _server.LastRequest;

            Assert.Equal("GET", req.Method);
            Assert.Equal("/products/prod1", req.PathAndQuery);
            Assert.Equal("jwt-abc", req.Headers["Bootpay-User-JWT"]);
        }

        [Fact]
        public async Task PerRequestRole_DoesNotOverwriteAndIsNotOverwritten()
        {
            // 기본 role 을 supervisor 로 바꿔도, endpoint 가 지정한 role(manager)이 단일 값으로 이긴다
            _api.AsSupervisor();
            await _api.ProductCreate(new CommerceProduct { Name = "상품" });
            Assert.Equal("manager", _server.LastRequest.Headers["BOOTPAY-ROLE"]);

            // scope 미지정 endpoint 는 기본 role 을 그대로 쓴다
            // (26-08-24: 예전엔 supervisorApprove 를 예시로 썼지만, 그 endpoint 는
            //  서버가 supervisor 를 요구해 이제 요청 단위로 role 을 고정한다.)
            await _api.UserList();
            Assert.Equal("supervisor", _server.LastRequest.Headers["BOOTPAY-ROLE"]);

            _api.ClearRole();
            await _api.UserList();
            Assert.Equal("user", _server.LastRequest.Headers["BOOTPAY-ROLE"]);

            // 반대로 supervisor scope endpoint 는 기본 role 이 user 여도 supervisor 로 나간다
            await _api.OrderSubscriptionSupervisorApprove("sub1");
            Assert.Equal("supervisor", _server.LastRequest.Headers["BOOTPAY-ROLE"]);
        }

        [Fact]
        public async Task ProductWrite_SendsManagerRole()
        {
            await _api.ProductCreate(new CommerceProduct { Name = "상품" });
            Assert.Equal("manager", _server.LastRequest.Headers["BOOTPAY-ROLE"]);
            Assert.Equal("/products", _server.LastRequest.PathAndQuery);
            Assert.StartsWith("application/json", _server.LastRequest.ContentType); // 이미지 없는 create 는 JSON

            await _api.ProductDelete("prod1");
            Assert.Equal("DELETE", _server.LastRequest.Method);
            Assert.Equal("manager", _server.LastRequest.Headers["BOOTPAY-ROLE"]);

            await _api.ProductStatus(new ProductStatusParams { ProductId = "prod1", Status = 1 });
            Assert.Equal("/products/prod1/status", _server.LastRequest.PathAndQuery);
            Assert.Equal("manager", _server.LastRequest.Headers["BOOTPAY-ROLE"]);
            var statusBody = JObject.Parse(_server.LastRequest.Body);
            Assert.False(statusBody.ContainsKey("product_id"), "product_id 는 URL 로만 전송된다");
        }

        [Fact]
        public async Task ProductCreateWithImages_UsesIndexedMultipartFields()
        {
            var image1 = Path.Combine(Path.GetTempPath(), $"bootpay-wire-{Guid.NewGuid():N}.png");
            var image2 = Path.Combine(Path.GetTempPath(), $"bootpay-wire-{Guid.NewGuid():N}.png");
            await File.WriteAllBytesAsync(image1, new byte[] { 0x89, 0x50, 0x4E, 0x47 });
            await File.WriteAllBytesAsync(image2, new byte[] { 0x89, 0x50, 0x4E, 0x47 });

            try
            {
                await _api.ProductCreateWithImages(new CommerceProduct { Name = "상품" }, new List<string> { image1, image2 });
                var req = _server.LastRequest;

                Assert.Equal("POST", req.Method);
                Assert.StartsWith("multipart/form-data", req.ContentType); // boundary 유실 금지
                Assert.Contains("boundary=", req.ContentType);
                Assert.Equal("manager", req.Headers["BOOTPAY-ROLE"]);
                Assert.Contains("images[0]", req.Body);
                Assert.Contains("images[1]", req.Body);
                Assert.DoesNotContain("name=images;", req.Body);
                Assert.DoesNotContain("name=\"images\"", req.Body);

                // 이미지가 없으면 JSON 으로 전송된다
                await _api.ProductCreateWithImages(new CommerceProduct { Name = "상품" }, new List<string>());
                Assert.StartsWith("application/json", _server.LastRequest.ContentType);
            }
            finally
            {
                File.Delete(image1);
                File.Delete(image2);
            }
        }

        [Fact]
        public async Task InvoiceList_DefaultsLimit24_AndSendsExtendedFilters()
        {
            await _api.InvoiceList();
            Assert.Equal("/invoices?page=1&limit=24", _server.LastRequest.PathAndQuery);
            Assert.Equal("user", _server.LastRequest.Headers["BOOTPAY-ROLE"]);

            await _api.InvoiceList(new InvoiceListParams { CsType = "subscription", UserId = "u1", ProductType = 2, CssAt = "2026-01-01", CseAt = "2026-01-31" });
            Assert.Equal(
                "/invoices?page=1&limit=24&cs_type=subscription&user_id=u1&product_type=2&css_at=2026-01-01&cse_at=2026-01-31",
                _server.LastRequest.PathAndQuery);
        }

        [Fact]
        public async Task InvoiceNotify_SendTypesOptional()
        {
            await _api.InvoiceNotify("inv1");
            var withoutTypes = _server.LastRequest;
            Assert.Equal("/invoices/inv1/notify", withoutTypes.PathAndQuery);
            Assert.Equal("{}", withoutTypes.Body);

            await _api.InvoiceNotify("inv1", new List<int> { 1, 2 });
            var body = JObject.Parse(_server.LastRequest.Body);
            Assert.Equal(new[] { 1, 2 }, body["send_types"]!.ToObject<int[]>());
        }

        [Fact]
        public async Task OrderCancelApprove_AcceptsBothIdNames_AndStripsIdsFromBody()
        {
            await _api.OrderCancelApprove(new OrderCancelActionParams
            {
                OrderCancellationRequestId = "cancel1",
                Message = "승인합니다"
            });
            var req = _server.LastRequest;

            Assert.Equal("PUT", req.Method);
            Assert.Equal("/order/cancel/cancel1/approve", req.PathAndQuery);
            Assert.Equal("supervisor", req.Headers["BOOTPAY-ROLE"]);

            var body = JObject.Parse(req.Body);
            Assert.Equal("승인합니다", (string?)body["message"]);
            Assert.False(body.ContainsKey("order_cancellation_request_id"));
            Assert.False(body.ContainsKey("order_cancel_request_history_id"));

            // 구 이름으로도 같은 URL 이 만들어진다
            await _api.OrderCancelReject(new OrderCancelActionParams { OrderCancelRequestHistoryId = "cancel2" });
            Assert.Equal("/order/cancel/cancel2/reject", _server.LastRequest.PathAndQuery);
        }

        [Fact]
        public async Task OrderCancelWithdraw_UsesUserRole()
        {
            await _api.OrderCancelWithdraw("cancel1");
            var req = _server.LastRequest;

            Assert.Equal("PUT", req.Method);
            Assert.Equal("/order/cancel/cancel1/withdraw", req.PathAndQuery);
            Assert.Equal("user", req.Headers["BOOTPAY-ROLE"]);
        }

        [Fact]
        public async Task AdjustmentDelete_SendsTargetIdInBody()
        {
            await _api.OrderSubscriptionAdjustmentDelete("sub1", "adj1");
            var req = _server.LastRequest;

            Assert.Equal("DELETE", req.Method);
            Assert.Equal("/order_subscriptions/sub1/adjustments", req.PathAndQuery); // query 가 아니라 body
            Assert.Equal("supervisor", req.Headers["BOOTPAY-ROLE"]);

            var body = JObject.Parse(req.Body);
            Assert.Equal("adj1", (string?)body["order_subscription_adjustment_id"]);
        }

        [Fact]
        public async Task AdjustmentUpdate_SupportsAdjustmentsArray_WithDefaultDuration()
        {
            await _api.OrderSubscriptionAdjustmentUpdate(new OrderSubscriptionAdjustmentUpdateParams
            {
                OrderSubscriptionId = "sub1",
                Adjustments = new List<CommerceOrderSubscriptionAdjustment>
                {
                    new CommerceOrderSubscriptionAdjustment { Price = -1000, Name = "할인" }
                }
            });
            var req = _server.LastRequest;

            Assert.Equal("PUT", req.Method);
            Assert.Equal("/order_subscriptions/sub1/adjustments", req.PathAndQuery);

            var body = JObject.Parse(req.Body);
            Assert.Equal(1, (int?)body["duration"]); // duration 미지정시 1
            Assert.False(body.ContainsKey("order_subscription_id"));
            Assert.Equal(-1000, (int?)body["adjustments"]![0]!["price"]);
        }

        [Fact]
        public async Task AdjustmentCreate_AppliesDefaults()
        {
            await _api.OrderSubscriptionAdjustmentCreate("sub1", new CommerceOrderSubscriptionAdjustment { Name = "설치비" });
            var body = JObject.Parse(_server.LastRequest.Body);

            Assert.Equal(0, (int?)body["price"]);
            Assert.Equal(1, (int?)body["duration"]);
            Assert.Equal(0, (int?)body["tax_free_price"]);
            Assert.Equal("supervisor", _server.LastRequest.Headers["BOOTPAY-ROLE"]);
        }

        [Fact]
        public async Task OrderSubscriptionUpdate_SupervisorRole_IdOnlyInUrl()
        {
            await _api.OrderSubscriptionUpdate(new OrderSubscriptionUpdateParams { OrderSubscriptionId = "sub1", BillingKey = "bk1" });
            var req = _server.LastRequest;

            Assert.Equal("PUT", req.Method);
            Assert.Equal("/order_subscriptions/sub1", req.PathAndQuery);
            Assert.Equal("supervisor", req.Headers["BOOTPAY-ROLE"]);

            var body = JObject.Parse(req.Body);
            Assert.Equal("bk1", (string?)body["billing_key"]);
            Assert.False(body.ContainsKey("order_subscription_id"));
        }

        [Fact]
        public async Task RequestIng_PurchaseAndTransfer_UseUserRole()
        {
            await _api.OrderSubscriptionPurchase(new OrderSubscriptionPurchaseParams { OrderNumber = "on1", Reason = "인수" });
            var purchase = _server.LastRequest;
            Assert.Equal("POST", purchase.Method);
            Assert.Equal("/order_subscriptions/requests/ing/purchase", purchase.PathAndQuery);
            Assert.Equal("user", purchase.Headers["BOOTPAY-ROLE"]);

            await _api.OrderSubscriptionTransfer(new OrderSubscriptionTransferParams { OrderSubscriptionId = "sub1", NewUserId = "u2" });
            var transfer = _server.LastRequest;
            Assert.Equal("POST", transfer.Method);
            Assert.Equal("/order_subscriptions/requests/ing/transfer", transfer.PathAndQuery);
            Assert.Equal("u2", (string?)JObject.Parse(transfer.Body)["new_user_id"]);
        }

        [Fact]
        public async Task OrderList_SendsSearchDateRange()
        {
            await _api.OrderList(new OrderListParams { SearchDateFrom = "2026-01-01", SearchDateTo = "2026-01-31" });
            Assert.Equal("/orders?search_date_from=2026-01-01&search_date_to=2026-01-31", _server.LastRequest.PathAndQuery);
        }

        [Fact]
        public async Task OrderSubscriptionList_SendsSearchDateRangeAndStatus()
        {
            await _api.OrderSubscriptionList(new OrderSubscriptionListParams
            {
                SearchDateFrom = "2026-01-01",
                SearchDateTo = "2026-01-31",
                Status = 3
            });
            Assert.Equal(
                "/order_subscriptions?search_date_from=2026-01-01&search_date_to=2026-01-31&status=3",
                _server.LastRequest.PathAndQuery);
        }

        [Fact]
        public async Task RequestList_RoleSwitchesByProjectId_WithDefaults()
        {
            await _api.OrderSubscriptionRequestList();
            var asUser = _server.LastRequest;
            Assert.Equal("/order-subscription-requests?page=1&limit=20", asUser.PathAndQuery);
            Assert.Equal("user", asUser.Headers["BOOTPAY-ROLE"]);

            await _api.OrderSubscriptionRequestList(new OrderSubscriptionRequestListParams
            {
                ProjectId = "proj1",
                OrderSubscriptionId = "sub1",
                UserId = "u1",
                UserGroupId = "g1"
            });
            var asSupervisor = _server.LastRequest;
            Assert.Equal(
                "/order-subscription-requests?project_id=proj1&order_subscription_id=sub1&page=1&limit=20&user_id=u1&user_group_id=g1",
                asSupervisor.PathAndQuery);
            Assert.Equal("supervisor", asSupervisor.Headers["BOOTPAY-ROLE"]);
        }

        [Fact]
        public async Task UserGroupLimit_ManagerRole_PurchaseLimitsInBody()
        {
            await _api.UserGroupLimit(new UserGroupLimitParams
            {
                UserGroupId = "g1",
                UseLimit = true,
                LimitMonthPurchase = 100000,
                LimitWeekPurchase = 30000
            });
            var req = _server.LastRequest;

            Assert.Equal("/user-groups/g1/limit", req.PathAndQuery);
            Assert.Equal("manager", req.Headers["BOOTPAY-ROLE"]);

            var body = JObject.Parse(req.Body);
            Assert.Equal(100000, (int?)body["limit_month_purchase"]);
            Assert.Equal(30000, (int?)body["limit_week_purchase"]);
            Assert.False(body.ContainsKey("user_group_id"), "user_group_id 는 URL 로만 전송된다");
        }

        [Fact]
        public async Task Multipart_SerializesBoolLowercaseAndInvariantNumbers()
        {
            var image = Path.Combine(Path.GetTempPath(), $"bootpay-wire-{Guid.NewGuid():N}.png");
            await File.WriteAllBytesAsync(image, new byte[] { 0x89, 0x50, 0x4E, 0x47 });

            try
            {
                await _api.ProductCreateWithImages(
                    new CommerceProduct { Name = "상품", UseStock = true, UseDiscount = false, Stock = 1000 },
                    new List<string> { image });
                var req = _server.LastRequest;

                // Rails 는 "True"/"False" 를 boolean 으로 캐스팅하지 못한다 (false 가 true 로 읽히는 위험)
                Assert.DoesNotContain("True", req.Body);
                Assert.DoesNotContain("False", req.Body);
                Assert.Contains("\r\ntrue\r\n", req.Body);
                Assert.Contains("\r\nfalse\r\n", req.Body);
                Assert.Contains("\r\n1000\r\n", req.Body); // invariant — 자릿수 구분자/로케일 포맷 없음
            }
            finally
            {
                File.Delete(image);
            }
        }

        [Fact]
        public async Task RequestUpdate_SupervisorRole_ApprovalInBody()
        {
            await _api.OrderSubscriptionRequestUpdate(new OrderSubscriptionRequestUpdateParams
            {
                OrderSubscriptionRequestHistoryId = "req1",
                Approval = "approve",
                Reason = "승인"
            });
            var req = _server.LastRequest;

            Assert.Equal("PUT", req.Method);
            Assert.Equal("/order-subscription-requests/req1", req.PathAndQuery);
            Assert.Equal("supervisor", req.Headers["BOOTPAY-ROLE"]);
            Assert.True(req.Headers.ContainsKey("Idempotency-Key"));

            var body = JObject.Parse(req.Body);
            Assert.Equal("approve", (string?)body["approval"]);
            Assert.Equal("승인", (string?)body["reason"]);
            Assert.False(body.ContainsKey("order_subscription_request_history_id"), "대상 ID 는 URL 로만 전송된다");
        }

        [Fact]
        public async Task RequestDetail_RoleSwitchesByProjectId()
        {
            await _api.OrderSubscriptionRequestDetail("req1");
            var asUser = _server.LastRequest;
            Assert.Equal("/order-subscription-requests/req1", asUser.PathAndQuery);
            Assert.Equal("user", asUser.Headers["BOOTPAY-ROLE"]);

            await _api.OrderSubscriptionRequestDetail("req1", "proj1");
            var asSupervisor = _server.LastRequest;
            Assert.Equal("/order-subscription-requests/req1?project_id=proj1", asSupervisor.PathAndQuery);
            Assert.Equal("supervisor", asSupervisor.Headers["BOOTPAY-ROLE"]);
        }

        [Fact]
        public async Task UserGroupAggregateTransaction_ManagerRole_IdOnlyInUrl()
        {
            await _api.UserGroupAggregateTransaction(new UserGroupAggregateTransactionParams
            {
                UserGroupId = "g1",
                UseSubscriptionAggregateTransaction = true,
                SubscriptionMonthDay = 25
            });
            var req = _server.LastRequest;

            Assert.Equal("PUT", req.Method);
            Assert.Equal("/user-groups/g1/aggregate-transaction", req.PathAndQuery);
            Assert.Equal("manager", req.Headers["BOOTPAY-ROLE"]);

            var body = JObject.Parse(req.Body);
            Assert.True((bool?)body["use_subscription_aggregate_transaction"]);
            Assert.Equal(25, (int?)body["subscription_month_day"]);
            Assert.False(body.ContainsKey("user_group_id"), "user_group_id 는 URL 로만 전송된다");
        }

        [Fact]
        public async Task CalculateTerminationFee_SendsBothParamsTogether()
        {
            await _api.OrderSubscriptionCalculateTerminationFee("sub1", "on1");
            var req = _server.LastRequest;

            Assert.Equal(
                "/order_subscriptions/requests/ing/calculate_termination_fee?order_subscription_id=sub1&order_number=on1",
                req.PathAndQuery);
            Assert.Equal("user", req.Headers["BOOTPAY-ROLE"]);
        }

        [Fact]
        public async Task UserLoginMall_PreservesExplicitCorporateType()
        {
            await _api.UserLoginMall(new MallUserLoginParams { LoginId = "corp", Password = "pw", CorporateType = 1 });
            var body = JObject.Parse(_server.LastRequest.Body);

            Assert.Equal(1, (int?)body["corporate_type"]); // 명시값은 0 으로 덮어쓰지 않는다
        }

        [Fact]
        public async Task OrderSubscriptionUpdate_SerializesContractChangeFields()
        {
            await _api.OrderSubscriptionUpdate(new OrderSubscriptionUpdateParams
            {
                OrderSubscriptionId = "sub1",
                ProductId = "prod1",
                ProductOptionId = "opt1",
                OrderName = "구독 변경",
                TotalSubscriptionDuration = 12,
                Quantity = 2,
                AddressId = "addr1",
                Username = "홍길동",
                Phone = "01000000000",
                Email = "a@b.com",
                UseFreeTrial = false,
                FreeTrialDay = 7,
                ServiceStartAt = "2026-09-01"
            });
            var body = JObject.Parse(_server.LastRequest.Body);

            Assert.Equal("prod1", (string?)body["product_id"]);
            Assert.Equal("opt1", (string?)body["product_option_id"]);
            Assert.Equal("구독 변경", (string?)body["order_name"]);
            Assert.Equal(12, (int?)body["total_subscription_duration"]);
            Assert.Equal(2, (int?)body["quantity"]);
            Assert.Equal("addr1", (string?)body["address_id"]);
            Assert.Equal("홍길동", (string?)body["username"]);
            Assert.Equal("01000000000", (string?)body["phone"]);
            Assert.Equal("a@b.com", (string?)body["email"]);
            Assert.False((bool?)body["use_free_trial"]);
            Assert.Equal(7, (int?)body["free_trial_day"]);
            Assert.Equal("2026-09-01", (string?)body["service_start_at"]);
            Assert.False(body.ContainsKey("order_subscription_id"));
        }

        [Fact]
        public async Task ProductStatus_SerializesPeriodFields()
        {
            await _api.ProductStatus(new ProductStatusParams
            {
                ProductId = "prod1",
                Status = 1,
                StatusFrozen = false,
                StatusReview = true,
                UseDisplayPeriod = true,
                DisplayStartAt = "2026-09-01",
                DisplayEndAt = "2026-09-30",
                UseSalePeriod = true,
                SaleStartAt = "2026-09-01",
                SaleEndAt = "2026-09-15"
            });
            var body = JObject.Parse(_server.LastRequest.Body);

            Assert.False((bool?)body["status_frozen"]);
            Assert.True((bool?)body["status_review"]);
            Assert.True((bool?)body["use_display_period"]);
            Assert.Equal("2026-09-01", (string?)body["display_start_at"]);
            Assert.Equal("2026-09-30", (string?)body["display_end_at"]);
            Assert.True((bool?)body["use_sale_period"]);
            Assert.Equal("2026-09-01", (string?)body["sale_start_at"]);
            Assert.Equal("2026-09-15", (string?)body["sale_end_at"]);
        }

        [Fact]
        public async Task OrderSubscriptionBillList_DefaultsPageAndLimit_UserRole()
        {
            await _api.OrderSubscriptionBillList();
            var req = _server.LastRequest;

            Assert.Equal("/order_subscription_bills?page=1&limit=20", req.PathAndQuery);
            Assert.Equal("user", req.Headers["BOOTPAY-ROLE"]);
            Assert.True(req.Headers.ContainsKey("Idempotency-Key"));
        }

        [Fact]
        public async Task Store_AttachesIdempotencyKey()
        {
            await _api.StoreInfo("store-key-1");
            var info = _server.LastRequest;
            Assert.Equal("/store", info.PathAndQuery);
            Assert.Equal("store-key-1", info.Headers["Idempotency-Key"]);

            await _api.StoreDetail();
            var detail = _server.LastRequest;
            Assert.Equal("/store/detail", detail.PathAndQuery);
            Assert.True(Guid.TryParse(detail.Headers["Idempotency-Key"], out _));
        }
    }
}
