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
            // @date: 26-11-16 서버 스코프가 supervisor:webhook_test 뿐이라 supervisor 로 고정한다(예전 user 는 401 을 냈다).
            Assert.Equal("supervisor", req.Headers["BOOTPAY-ROLE"]);

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

        // ── 누락 파라미터 보강 (Ruby SDK d4c8989 parity) ──

        [Fact]
        public async Task UserList_SendsMembershipTypeNotMemberType()
        {
            await _api.UserList(new UserListParams { MembershipType = 1 });
            var req = _server.LastRequest;

            Assert.Equal("GET", req.Method);
            Assert.Contains("membership_type=1", req.PathAndQuery);
            // 서버(v1/users_controller#index)가 읽지 않는 키는 보내지 않는다
            Assert.DoesNotContain("member_type=1", req.PathAndQuery);
        }

        [Fact]
        public async Task UserList_LegacyMemberType_IsMappedToMembershipType()
        {
#pragma warning disable CS0618
            await _api.UserList(new UserListParams { MemberType = 2 });
#pragma warning restore CS0618
            Assert.Contains("membership_type=2", _server.LastRequest.PathAndQuery);
        }

        [Fact]
        public async Task UserList_MembershipTypeWins_WhenBothGiven()
        {
#pragma warning disable CS0618
            await _api.UserList(new UserListParams { MembershipType = 1, MemberType = 2 });
#pragma warning restore CS0618
            Assert.Contains("membership_type=1", _server.LastRequest.PathAndQuery);
        }

        [Fact]
        public async Task OrderSubscriptionList_SendsOrderNumber()
        {
            await _api.OrderSubscriptionList(new OrderSubscriptionListParams { OrderNumber = "ORD-20260826-001" });
            Assert.Contains("order_number=ORD-20260826-001", _server.LastRequest.PathAndQuery);
        }

        [Fact]
        public async Task OrderSubscriptionUpdate_SendsMemoAsChangeReason()
        {
            await _api.OrderSubscriptionUpdate(new OrderSubscriptionUpdateParams
            {
                OrderSubscriptionId = "sub1",
                Price = 12000,
                Memo = "고객 요청 금액 변경"
            });
            var body = JObject.Parse(_server.LastRequest.Body);

            Assert.Equal("고객 요청 금액 변경", (string?)body["memo"]);
            Assert.Equal(12000, (int?)body["price"]);
        }

        [Fact]
        public async Task ProductList_SendsServerReadFilters()
        {
            // 서버(v1/products_controller#index)가 읽는 category_id / ex_uid / sort 를 실어야 한다
            await _api.ProductList(new ProductListParams
            {
                Page = 1,
                Limit = 10,
                Keyword = "coffee",
                CategoryId = "cat-1",
                ExUid = "EX-1",
                Sort = "-price"
            });
            var query = _server.LastRequest.PathAndQuery;

            Assert.Contains("category_id=cat-1", query);
            Assert.Contains("ex_uid=EX-1", query);
            Assert.Contains("sort=-price", query);
            Assert.Contains("keyword=coffee", query);
            Assert.Contains("page=1", query);
            Assert.Contains("limit=10", query);
        }

        [Fact]
        public async Task Products_SendsExUid()
        {
            await _api.Products(new MallProductListParams { ExUid = "EXT-1" });
            Assert.Contains("ex_uid=EXT-1", _server.LastRequest.PathAndQuery);
        }

        [Fact]
        public async Task ProductDetail_AttachesJwtHeaderWhenGiven()
        {
            await _api.ProductDetail("prod1", "jwt-abc");
            var req = _server.LastRequest;

            Assert.Equal("GET", req.Method);
            Assert.Equal("/products/prod1", req.PathAndQuery);
            Assert.Equal("jwt-abc", req.Headers["Bootpay-User-JWT"]); // ProductDetailMall 과 동작이 같다
        }

        [Fact]
        public async Task PerRequestRole_DoesNotOverwriteAndIsNotOverwritten()
        {
            // 기본 role 을 supervisor 로 바꿔도, endpoint 가 지정한 role(user)가 단일 값으로 이긴다
            _api.AsSupervisor();
            await _api.ProductCreate(new CommerceProduct { Name = "상품" });
            Assert.Equal("user", _server.LastRequest.Headers["BOOTPAY-ROLE"]);
            Assert.Equal("supervisor", _api.GetCurrentRole());

            // @date: 26-11-16 이제 모든 커머스 endpoint 가 서버 스코프에 맞는 role 을 요청 단위로 고정한다.
            //   user 전용 endpoint(UserList)는 기본 role 이 supervisor 여도 user 로 나가고, 기본 role 은 그대로 남는다.
            await _api.UserList();
            Assert.Equal("user", _server.LastRequest.Headers["BOOTPAY-ROLE"]);
            Assert.Equal("supervisor", _api.GetCurrentRole());

            _api.ClearRole();
            await _api.UserList();
            Assert.Equal("user", _server.LastRequest.Headers["BOOTPAY-ROLE"]);

            // 반대로 supervisor scope endpoint 는 기본 role 이 user 여도 supervisor 로 나간다
            await _api.OrderSubscriptionSupervisorApprove("sub1");
            Assert.Equal("supervisor", _server.LastRequest.Headers["BOOTPAY-ROLE"]);
        }

        [Fact]
        public async Task ProductWrite_SendsUserRoleAndPreservesInstanceRole()
        {
            _api.AsManager();
            await _api.ProductCreate(new CommerceProduct { Name = "상품" });
            Assert.Equal("user", _server.LastRequest.Headers["BOOTPAY-ROLE"]);
            Assert.Equal("/products", _server.LastRequest.PathAndQuery);
            Assert.StartsWith("application/json", _server.LastRequest.ContentType); // 이미지 없는 create 는 JSON

            await _api.ProductUpdate(new CommerceProduct { ProductId = "prod1", Name = "수정 상품" });
            Assert.Equal("PUT", _server.LastRequest.Method);
            Assert.Equal("user", _server.LastRequest.Headers["BOOTPAY-ROLE"]);

            await _api.ProductDelete("prod1");
            Assert.Equal("DELETE", _server.LastRequest.Method);
            Assert.Equal("user", _server.LastRequest.Headers["BOOTPAY-ROLE"]);

            await _api.ProductStatus(new ProductStatusParams { ProductId = "prod1", Status = 1 });
            Assert.Equal("/products/prod1/status", _server.LastRequest.PathAndQuery);
            Assert.Equal("user", _server.LastRequest.Headers["BOOTPAY-ROLE"]);
            var statusBody = JObject.Parse(_server.LastRequest.Body);
            Assert.False(statusBody.ContainsKey("product_id"), "product_id 는 URL 로만 전송된다");
            Assert.Equal("manager", _api.GetCurrentRole());
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
                Assert.Equal("user", req.Headers["BOOTPAY-ROLE"]);
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
        public async Task InvoiceCreate_SendsUserRoleAndPreservesInstanceRole()
        {
            // 서버 스코프는 user:invoice_create 뿐 — 인스턴스 role 이 manager 여도 user 로 보낸다
            _api.AsManager();
            await _api.InvoiceCreate(new CommerceInvoice { Name = "청구서" });
            Assert.Equal("POST", _server.LastRequest.Method);
            Assert.Equal("/invoices", _server.LastRequest.PathAndQuery);
            Assert.Equal("user", _server.LastRequest.Headers["BOOTPAY-ROLE"]);
            Assert.False(string.IsNullOrEmpty(_server.LastRequest.Headers["Idempotency-Key"]));

            await _api.InvoiceCreate(new CommerceInvoice { Name = "청구서" }, "idem-invoice");
            Assert.Equal("idem-invoice", _server.LastRequest.Headers["Idempotency-Key"]);
            Assert.Equal("manager", _api.GetCurrentRole());
        }

        [Fact]
        public async Task OrderPurchases_SendsUserRoleEvenAsManager()
        {
            _api.AsManager();
            await _api.OrderPurchases("ORD-1");
            Assert.Equal("GET", _server.LastRequest.Method);
            Assert.Equal("/orders/ORD-1/purchases", _server.LastRequest.PathAndQuery);
            Assert.Equal("user", _server.LastRequest.Headers["BOOTPAY-ROLE"]);
            Assert.False(string.IsNullOrEmpty(_server.LastRequest.Headers["Idempotency-Key"]));

            await _api.OrderPurchases("ORD-1", "idem-1");
            Assert.Equal("idem-1", _server.LastRequest.Headers["Idempotency-Key"]);
            Assert.Equal("manager", _api.GetCurrentRole());
        }

        [Fact]
        public async Task OrderUpdatePurchases_SendsPurchasesBody()
        {
            _api.AsManager();
            await _api.OrderUpdatePurchases(
                "ORD-1",
                new List<OrderPurchaseUpdateItem> { new OrderPurchaseUpdateItem { OrderPurchaseNumber = "P1", Status = 4 } },
                "idem-2");
            var req = _server.LastRequest;
            Assert.Equal("PUT", req.Method);
            Assert.Equal("/orders/ORD-1/purchases", req.PathAndQuery);
            Assert.Equal("user", req.Headers["BOOTPAY-ROLE"]);
            Assert.Equal("idem-2", req.Headers["Idempotency-Key"]);
            Assert.Contains("\"purchases\"", req.Body);
            Assert.Contains("\"order_purchase_number\":\"P1\"", req.Body);
            Assert.Contains("\"status\":4", req.Body);
            Assert.DoesNotContain("order_number\":\"ORD-1", req.Body);
            Assert.DoesNotContain("idempotency", req.Body);
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

        // ── 구독 기준금액 변경 · 범위 회차조정 (Ruby SDK 9832af9 parity) ──

        [Fact]
        public async Task OrderSubscriptionUpdate_SendsPriceAsBaseAmount()
        {
            await _api.OrderSubscriptionUpdate(new OrderSubscriptionUpdateParams
            {
                OrderSubscriptionId = "sub1",
                Price = 12000
            });
            var req = _server.LastRequest;

            Assert.Equal("PUT", req.Method);
            Assert.Equal("/order_subscriptions/sub1", req.PathAndQuery);
            Assert.Equal("supervisor", req.Headers["BOOTPAY-ROLE"]);

            var body = JObject.Parse(req.Body);
            Assert.Equal(12000, (int?)body["price"]);
            Assert.False(body.ContainsKey("order_subscription_id"));
        }

        [Fact]
        public async Task OrderSubscriptionUpdate_OmitsUnsetPrice()
        {
            await _api.OrderSubscriptionUpdate(new OrderSubscriptionUpdateParams
            {
                OrderSubscriptionId = "sub1",
                OrderName = "변경구독"
            });
            var body = JObject.Parse(_server.LastRequest.Body);

            Assert.False(body.ContainsKey("price")); // 지정하지 않은 price 는 전송되지 않는다
        }

        [Fact]
        public async Task AdjustmentCreate_SingleDuration_OmitsRangeFields()
        {
            await _api.OrderSubscriptionAdjustmentCreate("sub1", new CommerceOrderSubscriptionAdjustment
            {
                Name = "5회차 할인",
                Price = -1000,
                Duration = 5
            });
            var body = JObject.Parse(_server.LastRequest.Body);

            Assert.Equal(5, (int?)body["duration"]);
            Assert.False(body.ContainsKey("duration_from"));
            Assert.False(body.ContainsKey("duration_to"));
            Assert.False(body.ContainsKey("is_unlimited"));
        }

        [Fact]
        public async Task AdjustmentCreate_DurationRange_SendsFromAndTo()
        {
            await _api.OrderSubscriptionAdjustmentCreate("sub1", new CommerceOrderSubscriptionAdjustment
            {
                Name = "3~7회차 할인",
                Price = -1000,
                DurationFrom = 3,
                DurationTo = 7
            });
            var req = _server.LastRequest;

            Assert.Equal("POST", req.Method);
            Assert.Equal("/order_subscriptions/sub1/adjustments", req.PathAndQuery);
            Assert.Equal("supervisor", req.Headers["BOOTPAY-ROLE"]);

            var body = JObject.Parse(req.Body);
            Assert.Equal(3, (int?)body["duration_from"]);
            Assert.Equal(7, (int?)body["duration_to"]);
            Assert.Equal(1, (int?)body["duration"]); // 기준 SDK 와 동일하게 기본값 1 이 항상 실린다
            Assert.False(body.ContainsKey("is_unlimited"));
        }

        [Fact]
        public async Task AdjustmentCreate_Unlimited_SendsFromAndFlagOnly()
        {
            await _api.OrderSubscriptionAdjustmentCreate("sub1", new CommerceOrderSubscriptionAdjustment
            {
                Name = "3회차부터 추가금",
                Price = 500,
                DurationFrom = 3,
                IsUnlimited = true
            });
            var body = JObject.Parse(_server.LastRequest.Body);

            Assert.Equal(3, (int?)body["duration_from"]);
            Assert.True((bool?)body["is_unlimited"]);
            Assert.False(body.ContainsKey("duration_to")); // 지정하지 않으면 전송하지 않는다
        }

        [Fact]
        public async Task AdjustmentCreate_ExplicitFalseIsUnlimited_IsSent()
        {
            await _api.OrderSubscriptionAdjustmentCreate("sub1", new CommerceOrderSubscriptionAdjustment
            {
                Name = "할인",
                Price = -1000,
                DurationFrom = 3,
                DurationTo = 7,
                IsUnlimited = false
            });
            var body = JObject.Parse(_server.LastRequest.Body);

            Assert.False((bool?)body["is_unlimited"]); // bool? 이므로 명시적 false 는 구분되어 전송된다
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

        // 엔드포인트별 고정 BOOTPAY-ROLE 표 (서버 검증 규칙) — user_or_supervisor 는 인스턴스 role 이 supervisor 일 때만 supervisor
        private static List<(string Name, string Rule, Func<WireCommerceApi, Task<HttpResponseMessage>> Call)> RoleTable() => new()
        {
            ("CartOrderPreview", "user", a => a.CartOrderPreview()),
            ("CategoryList", "user", a => a.CategoryList()),
            ("CategoryDetail", "user", a => a.CategoryDetail("c1")),
            ("CategoryCreate", "supervisor", a => a.CategoryCreate(new CategoryCreateParams())),
            ("CouponList", "user_or_supervisor", a => a.CouponList()),
            ("CouponAvailable", "user_or_supervisor", a => a.CouponAvailable()),
            ("CouponDownload", "user", a => a.CouponDownload(new CouponDownloadParams())),
            ("DepositAccounts", "supervisor", a => a.DepositAccounts()),
            ("DepositDetail", "supervisor", a => a.DepositDetail("d1")),
            ("NoticeDetail", "user_or_supervisor", a => a.NoticeDetail("n1")),
            ("FaqDetail", "user_or_supervisor", a => a.FaqDetail("f1")),
            ("ProductQnaDelete", "user_or_supervisor", a => a.ProductQnaDelete("q1")),
            ("InvoiceList", "user", a => a.InvoiceList()),
            ("OrderCancelRequest", "user_or_supervisor", a => a.OrderCancelRequest(new OrderCancelParams())),
            ("OrderCancelList", "user", a => a.OrderCancelList()),
            ("OrderSubscriptionBillList", "user_or_supervisor", a => a.OrderSubscriptionBillList()),
            ("OrderSubscriptionBillDetail", "user_or_supervisor", a => a.OrderSubscriptionBillDetail("b1")),
            ("OrderSubscriptionBillUpdate", "supervisor", a => a.OrderSubscriptionBillUpdate(new CommerceOrderSubscriptionBill { OrderSubscriptionBillId = "b1" })),
            ("OrderSubscriptionRequestList", "user_or_supervisor", a => a.OrderSubscriptionRequestList()),
            ("OrderSubscriptionRequestDetail", "user_or_supervisor", a => a.OrderSubscriptionRequestDetail("r1")),
            ("OrderSubscriptionList", "user", a => a.OrderSubscriptionList()),
            ("OrderSubscriptionDetail", "user", a => a.OrderSubscriptionDetail("s1")),
            ("OrderSubscriptionUpdate", "supervisor", a => a.OrderSubscriptionUpdate(new OrderSubscriptionUpdateParams { OrderSubscriptionId = "s1" })),
            ("OrderList", "user", a => a.OrderList()),
            ("OrderDetail", "user", a => a.OrderDetail("o1")),
            ("OrderMonth", "user", a => a.OrderMonth("g1", "2026-11")),
            ("OrderPurchases", "user", a => a.OrderPurchases("ORD-1")),
            ("PointBalance", "user", a => a.PointBalance()),
            ("PointTransactions", "user", a => a.PointTransactions()),
            ("ProductList", "user", a => a.ProductList()),
            ("ProductDetail", "user", a => a.ProductDetail("p1")),
            ("StoreDetail", "user", a => a.StoreDetail()),
            ("UserGroupCreate", "user", a => a.UserGroupCreate(new CommerceUserGroup())),
            ("UserGroupList", "user", a => a.UserGroupList()),
            ("UserGroupDetail", "user", a => a.UserGroupDetail("g1")),
            ("UserGroupUpdate", "user", a => a.UserGroupUpdate(new CommerceUserGroup { UserGroupId = "g1" })),
            ("UserGroupUserCreate", "manager", a => a.UserGroupUserCreate("g1", "u1")),
            ("UserList", "user", a => a.UserList()),
            ("UserDetail", "user", a => a.UserDetail("u1")),
            ("UserSession", "user", a => a.UserSession()),
            ("UserUpdate", "user", a => a.UserUpdate(new CommerceUser { UserId = "u1" })),
            ("UserDelete", "user", a => a.UserDelete("u1")),
            ("UserJoin", "user", a => a.UserJoin(new CommerceUser())),
            ("UserLogin", "user", a => a.UserLogin("id", "pw")),
            ("UserToken", "user", a => a.UserToken("u1")),
            ("UserCheckExist", "user", a => a.UserCheckExist("email-exist", "a@b.c")),
            ("UserAuthenticationData", "user", a => a.UserAuthenticationData("st1")),
            ("UserJoinMall", "user", a => a.UserJoinMall(new MallUserJoinParams { LoginId = "h", Password = "pw", Name = "n" })),
            ("UserLoginMall", "user", a => a.UserLoginMall("id", "pw")),
            ("UserJoinCheckMall", "user", a => a.UserJoinCheckMall("id-exist", "a")),
            ("WebhookSendTest", "supervisor", a => a.WebhookSendTest()),
        };

        [Fact]
        public async Task FixedRolePerEndpoint_ManagerInstance()
        {
            _api.AsManager();
            foreach (var (name, rule, call) in RoleTable())
            {
                await call(_api);
                var expected = rule == "user_or_supervisor" ? "user" : rule;
                Assert.True(expected == _server.LastRequest.Headers["BOOTPAY-ROLE"], $"{name}: expected {expected}");
            }
            Assert.Equal("manager", _api.GetCurrentRole());
        }

        [Fact]
        public async Task FixedRolePerEndpoint_SupervisorInstance()
        {
            _api.AsSupervisor();
            foreach (var (name, rule, call) in RoleTable())
            {
                await call(_api);
                var expected = rule == "user_or_supervisor" ? "supervisor" : rule;
                Assert.True(expected == _server.LastRequest.Headers["BOOTPAY-ROLE"], $"{name}: expected {expected}");
            }
            Assert.Equal("supervisor", _api.GetCurrentRole());
        }

        [Fact]
        public async Task Deposit_AccountsAndDetail_RespectExplicitIdempotencyKey()
        {
            await _api.DepositAccounts("acc-key");
            Assert.Equal("/sdk/deposits/accounts", _server.LastRequest.PathAndQuery);
            Assert.Equal("supervisor", _server.LastRequest.Headers["BOOTPAY-ROLE"]);
            Assert.Equal("acc-key", _server.LastRequest.Headers["Idempotency-Key"]);

            await _api.DepositDetail("d1", "detail-key");
            Assert.Equal("/sdk/deposits/d1", _server.LastRequest.PathAndQuery);
            Assert.Equal("detail-key", _server.LastRequest.Headers["Idempotency-Key"]);
        }

        [Fact]
        public async Task EmptyPathId_ThrowsWithoutSendingRequest()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _api.OrderDetail(""));
            await Assert.ThrowsAsync<ArgumentException>(() => _api.OrderSubscriptionDetail(""));
            await Assert.ThrowsAsync<ArgumentException>(() => _api.DepositDetail(""));
            await Assert.ThrowsAsync<ArgumentException>(() => _api.UserGroupUserCreate("", "u1"));
            Assert.Empty(_server.Requests);
        }

        [Theory]
        [InlineData("orders/", true)]
        [InlineData("/orders/", true)]
        [InlineData("orders//purchases", true)]
        [InlineData("orders/?page=1", true)]
        [InlineData("", true)]
        [InlineData("orders", false)]
        [InlineData("orders/o1/purchases", false)]
        [InlineData("/orders/o1?a=//", false)]
        public void BlankPathSegment_DetectsEmptyIds(string url, bool blank)
        {
            Assert.Equal(blank, Bootpay.Commerce.BootpayCommerceObject.BlankPathSegment(url));
        }
    }
}
