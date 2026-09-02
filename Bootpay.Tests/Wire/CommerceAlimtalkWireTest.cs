using Bootpay.Commerce.Models;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Bootpay.Tests.Wire
{
    /// <summary>
    /// 알림톡 v1 API wire-format 검증 (NodeJS 2.13.0 parity).
    ///
    /// 알림톡은 다른 Commerce 엔드포인트와 두 가지가 다르다 — 여기서 그 둘을 고정한다.
    ///   1) Idempotency-Key 를 <b>싣지 않는다</b> (멱등은 발송의 ref_id 로만 성립한다)
    ///   2) BOOTPAY-ROLE 은 인스턴스 role 과 무관하게 <b>항상 user</b> (스코프 키가 전부 user:alimtalk_*)
    /// </summary>
    public class CommerceAlimtalkWireTest : IDisposable
    {
        private readonly MockServer _server;
        private readonly WireCommerceApi _api;

        public CommerceAlimtalkWireTest()
        {
            _server = new MockServer();
            _api = new WireCommerceApi(_server.BaseUrl);
        }

        public void Dispose() => _server.Dispose();

        /// <summary>알림톡 공통 규약 — 경로/메서드 + user role + Idempotency-Key 부재.</summary>
        private void AssertAlimtalk(string method, string pathAndQuery)
        {
            var req = _server.LastRequest;
            Assert.Equal(method, req.Method);
            Assert.Equal(pathAndQuery, req.PathAndQuery);
            Assert.Equal("user", req.Headers["BOOTPAY-ROLE"]);
            Assert.False(req.Headers.ContainsKey("Idempotency-Key"),
                "알림톡 API 는 Idempotency-Key 를 읽지 않는다 — 붙이면 서버가 주지 않는 보장을 주는 것처럼 보인다");
        }

        private JObject Body() => JObject.Parse(_server.LastRequest.Body);

        #region 발송

        [Fact]
        public async Task Send_PostsCompactBody_AndKeepsExplicitFalseFallback()
        {
            await _api.AlimtalkSend(new AlimtalkSendParams
            {
                TemplateCode = "TPL_001",
                To = "01012345678",
                Variables = new Dictionary<string, object> { { "user_name", "홍길동" } },
                RefId = "order-1",
                Fallback = false
            });
            AssertAlimtalk("POST", "/alimtalk/send");

            var body = Body();
            Assert.Equal("TPL_001", (string?)body["template_code"]);
            Assert.Equal("01012345678", (string?)body["to"]);
            Assert.Equal("홍길동", (string?)body["variables"]?["user_name"]);
            Assert.Equal("order-1", (string?)body["ref_id"]);
            // ⚠️ 미지정(null)과 false 는 다르다 — false 는 반드시 전달되어야 한다
            Assert.True(body.ContainsKey("fallback"));
            Assert.False((bool?)body["fallback"]);
            // 지정하지 않은 값은 전송되지 않는다
            Assert.False(body.ContainsKey("reserved_at"));
            Assert.False(body.ContainsKey("sender_key"));
            Assert.False(body.ContainsKey("user_id"));
        }

        [Fact]
        public async Task Send_OmitsFallbackWhenUnset()
        {
            await _api.AlimtalkSend(new AlimtalkSendParams { TemplateCode = "TPL_001", To = "01012345678" });
            AssertAlimtalk("POST", "/alimtalk/send");

            // 미지정이면 키 자체가 없어야 프로젝트 기본값이 적용된다
            Assert.False(Body().ContainsKey("fallback"));
        }

        [Fact]
        public async Task SendBulk_PostsRecipients()
        {
            await _api.AlimtalkSendBulk(new AlimtalkSendBulkParams
            {
                TemplateCode = "TPL_001",
                Recipients = new List<AlimtalkSendBulkRecipient>
                {
                    new AlimtalkSendBulkRecipient { To = "01011112222", RefId = "b-1" },
                    new AlimtalkSendBulkRecipient { To = "01033334444" }
                },
                SenderKey = "sk_public"
            });
            AssertAlimtalk("POST", "/alimtalk/send/bulk");

            var body = Body();
            Assert.Equal(2, ((JArray?)body["recipients"])?.Count);
            Assert.Equal("01011112222", (string?)body["recipients"]?[0]?["to"]);
            Assert.Equal("sk_public", (string?)body["sender_key"]);
        }

        [Fact]
        public async Task SendCancel_Deletes()
        {
            await _api.AlimtalkSendCancel("rcpt_1");
            AssertAlimtalk("DELETE", "/alimtalk/send/rcpt_1");
        }

        #endregion

        #region 발송내역·집계

        [Fact]
        public async Task MessageList_SendsOnlyGivenFilters()
        {
            await _api.AlimtalkMessageList(new AlimtalkMessageListParams { Status = "success", Limit = 50 });
            AssertAlimtalk("GET", "/alimtalk/messages?status=success&limit=50");
        }

        [Fact]
        public async Task MessageList_WithoutParams_HasNoQueryString()
        {
            await _api.AlimtalkMessageList();
            AssertAlimtalk("GET", "/alimtalk/messages");
        }

        [Fact]
        public async Task MessageStats_SendsPeriod()
        {
            await _api.AlimtalkMessageStats(new AlimtalkMessageStatsParams { SAt = "2026-08-01", EAt = "2026-08-31" });
            AssertAlimtalk("GET", "/alimtalk/messages/stats?s_at=2026-08-01&e_at=2026-08-31");
        }

        [Fact]
        public async Task MessageDetail_Gets()
        {
            await _api.AlimtalkMessageDetail("rcpt_1");
            AssertAlimtalk("GET", "/alimtalk/messages/rcpt_1");
        }

        #endregion

        #region 공식 카탈로그

        [Fact]
        public async Task OfficialList_SendsKeywordAsCanonicalQ()
        {
            await _api.AlimtalkOfficialList(new AlimtalkOfficialListParams { Keyword = "주문", MsgType = "BA" });
            // 서버는 q 를 먼저 보고 없으면 keyword 를 본다 — SDK 는 정본 키인 q 로 보낸다
            AssertAlimtalk("GET", "/alimtalk/official?q=%EC%A3%BC%EB%AC%B8&msg_type=BA");
        }

        [Fact]
        public async Task OfficialRecommend_PostsText()
        {
            await _api.AlimtalkOfficialRecommend(new AlimtalkOfficialRecommendParams { Text = "주문이 접수되었습니다", Limit = 3 });
            AssertAlimtalk("POST", "/alimtalk/official/recommend");

            var body = Body();
            Assert.Equal("주문이 접수되었습니다", (string?)body["text"]);
            Assert.Equal(3, (int?)body["limit"]);
            Assert.False(body.ContainsKey("category"));
        }

        [Fact]
        public async Task OfficialDetail_AppendsKspIdOnlyWhenGiven()
        {
            await _api.AlimtalkOfficialDetail("OFF_001");
            AssertAlimtalk("GET", "/alimtalk/official/OFF_001");

            await _api.AlimtalkOfficialDetail("OFF_001", "ksp_1");
            AssertAlimtalk("GET", "/alimtalk/official/OFF_001?ksp_id=ksp_1");
        }

        #endregion

        #region 자체 템플릿

        [Fact]
        public async Task TemplateList_SendsFilters()
        {
            await _api.AlimtalkTemplateList(new AlimtalkTemplateListParams { Ins = "APR", Sort = "latest" });
            AssertAlimtalk("GET", "/alimtalk/templates?ins=APR&sort=latest");
        }

        [Fact]
        public async Task TemplateCreate_SendsDraftFlagAndExtraAttrs()
        {
            await _api.AlimtalkTemplateCreate(new AlimtalkTemplateCreateParams
            {
                KspId = "ksp_1",
                Name = "주문 접수",
                Content = "#{user_name}님 주문이 접수되었습니다.",
                Register = false,
                Attrs = new Dictionary<string, object> { { "custom_field", "v1" } }
            });
            AssertAlimtalk("POST", "/alimtalk/templates");

            var body = Body();
            Assert.Equal("ksp_1", (string?)body["ksp_id"]);
            Assert.Equal("주문 접수", (string?)body["name"]);
            // ⚠️ register:false 가 빠지면 대행사·카카오에 즉시 실제 등록된다
            Assert.True(body.ContainsKey("register"));
            Assert.False((bool?)body["register"]);
            // 명시되지 않은 필드는 Attrs 로 그대로 전달된다
            Assert.Equal("v1", (string?)body["custom_field"]);
            Assert.False(body.ContainsKey("attrs"));
        }

        [Fact]
        public async Task TemplateCreate_AttrsOverrideNamedProperty()
        {
            await _api.AlimtalkTemplateCreate(new AlimtalkTemplateCreateParams
            {
                KspId = "ksp_1",
                Name = "원본",
                Attrs = new Dictionary<string, object> { { "name", "덮어쓴 값" } }
            });

            var body = Body();
            Assert.Equal("덮어쓴 값", (string?)body["name"]);
            Assert.Single(body.Properties(), p => p.Name == "name");
        }

        [Fact]
        public async Task TemplateDetail_SendsSyncOnlyWhenGiven()
        {
            await _api.AlimtalkTemplateDetail("tpl_1");
            AssertAlimtalk("GET", "/alimtalk/templates/tpl_1");

            // 서버 기본이 true 라 초안 조회는 false 를 명시해야 벤더 동기화를 피한다
            await _api.AlimtalkTemplateDetail("tpl_1", false);
            AssertAlimtalk("GET", "/alimtalk/templates/tpl_1?sync=false");
        }

        [Fact]
        public async Task TemplateUpdate_Puts()
        {
            await _api.AlimtalkTemplateUpdate("tpl_1", new AlimtalkTemplateUpdateParams { Name = "수정", Content = "본문" });
            AssertAlimtalk("PUT", "/alimtalk/templates/tpl_1");

            var body = Body();
            Assert.Equal("수정", (string?)body["name"]);
            Assert.Equal("본문", (string?)body["content"]);
        }

        [Fact]
        public async Task TemplateDelete_Deletes()
        {
            await _api.AlimtalkTemplateDelete("tpl_1");
            AssertAlimtalk("DELETE", "/alimtalk/templates/tpl_1");
        }

        [Fact]
        public async Task TemplateRegisterAndInspect_PostEmptyBody()
        {
            await _api.AlimtalkTemplateRegister("tpl_1");
            AssertAlimtalk("POST", "/alimtalk/templates/tpl_1/register");
            Assert.Equal("{}", _server.LastRequest.Body);

            await _api.AlimtalkTemplateInspect("tpl_1");
            AssertAlimtalk("POST", "/alimtalk/templates/tpl_1/inspect");
            Assert.Equal("{}", _server.LastRequest.Body);
        }

        [Fact]
        public async Task TemplateExport_DefaultsToJsonFormat()
        {
            // ⚠️ 서버 기본은 csv 지만 csv 본문은 JSON 이 아니다 — SDK 기본을 json 으로 둔다
            await _api.AlimtalkTemplateExport();
            AssertAlimtalk("GET", "/alimtalk/templates/export?format=json");
        }

        [Fact]
        public async Task TemplateExport_CsvSendsWildcardAccept()
        {
            await _api.AlimtalkTemplateExport(new AlimtalkTemplateExportParams { Format = "csv", Scope = "all" });
            AssertAlimtalk("GET", "/alimtalk/templates/export?format=csv&scope=all");
            Assert.Equal("*/*", _server.LastRequest.Headers["Accept"]);
        }

        #endregion

        #region 발신프로필

        [Fact]
        public async Task SenderCategories_Gets()
        {
            await _api.AlimtalkSenderCategories();
            AssertAlimtalk("GET", "/alimtalk/categories");
        }

        [Fact]
        public async Task SenderOtp_PostsYellowIdAndPhone()
        {
            await _api.AlimtalkSenderOtp(new AlimtalkSenderOtpParams { YellowId = "@bootpay", Phone = "01012345678" });
            AssertAlimtalk("POST", "/alimtalk/senders/otp");

            var body = Body();
            Assert.Equal("@bootpay", (string?)body["yellow_id"]);
            Assert.Equal("01012345678", (string?)body["phone"]);
        }

        [Fact]
        public async Task SenderCreate_PostsAllRequiredFields()
        {
            await _api.AlimtalkSenderCreate(new AlimtalkSenderCreateParams
            {
                Otp = "123456",
                YellowId = "@bootpay",
                Phone = "01012345678",
                CategoryCode = "001001"
            });
            AssertAlimtalk("POST", "/alimtalk/senders");

            var body = Body();
            Assert.Equal("123456", (string?)body["otp"]);
            Assert.Equal("001001", (string?)body["category_code"]);
        }

        [Fact]
        public async Task SenderListDetailRelease()
        {
            await _api.AlimtalkSenderList();
            AssertAlimtalk("GET", "/alimtalk/senders");

            await _api.AlimtalkSenderDetail("ksp_1", true);
            AssertAlimtalk("GET", "/alimtalk/senders/ksp_1?sync=true");

            await _api.AlimtalkSenderRelease("ksp_1");
            AssertAlimtalk("DELETE", "/alimtalk/senders/ksp_1");
        }

        [Fact]
        public async Task SenderVariableExamples_WrapsInExamplesKey()
        {
            await _api.AlimtalkSenderVariableExamples("ksp_1", new Dictionary<string, string> { { "user_name", "홍길동" } });
            AssertAlimtalk("PUT", "/alimtalk/senders/ksp_1/variable_examples");

            Assert.Equal("홍길동", (string?)Body()["examples"]?["user_name"]);
        }

        #endregion

        #region 수신거부

        [Fact]
        public async Task OptoutList_SendsPhoneAndPage()
        {
            await _api.AlimtalkOptoutList(new AlimtalkOptoutListParams { Phone = "0101234", Page = 2 });
            AssertAlimtalk("GET", "/alimtalk/optouts?phone=0101234&page=2");
        }

        [Fact]
        public async Task OptoutCreate_PostsPhone()
        {
            await _api.AlimtalkOptoutCreate(new AlimtalkOptoutCreateParams { Phone = "01012345678", Reason = "고객 요청" });
            AssertAlimtalk("POST", "/alimtalk/optouts");

            var body = Body();
            Assert.Equal("01012345678", (string?)body["phone"]);
            Assert.Equal("고객 요청", (string?)body["reason"]);
        }

        [Fact]
        public async Task OptoutCheck_PostsPhonesArray()
        {
            await _api.AlimtalkOptoutCheck(new AlimtalkOptoutCheckParams
            {
                Phones = new List<string> { "01011112222", "01033334444" }
            });
            AssertAlimtalk("POST", "/alimtalk/optouts/check");

            var body = Body();
            Assert.Equal(2, ((JArray?)body["phones"])?.Count);
            Assert.False(body.ContainsKey("phone"));
        }

        [Fact]
        public async Task OptoutRelease_Deletes()
        {
            await _api.AlimtalkOptoutRelease("01012345678");
            AssertAlimtalk("DELETE", "/alimtalk/optouts/01012345678");
        }

        #endregion

        #region 알림톡 웹훅

        [Fact]
        public async Task WebhookDetail_Gets()
        {
            await _api.AlimtalkWebhookDetail();
            AssertAlimtalk("GET", "/alimtalk/webhook");
        }

        [Fact]
        public async Task WebhookUpdate_PutsUrlAndEvents()
        {
            await _api.AlimtalkWebhookUpdate(new AlimtalkWebhookUpdateParams
            {
                Url = "https://example.com/hook",
                Events = new List<int> { AlimtalkWebhookEvent.Delivered, AlimtalkWebhookEvent.Failed },
                Enabled = true
            });
            AssertAlimtalk("PUT", "/alimtalk/webhook");

            var body = Body();
            Assert.Equal("https://example.com/hook", (string?)body["url"]);
            Assert.Equal(new[] { 301, 302 }, ((JArray?)body["events"])?.Select(t => (int)t!).ToArray());
            Assert.True((bool?)body["enabled"]);
        }

        [Fact]
        public async Task WebhookUpdate_WithoutParams_PutsEmptyBody()
        {
            await _api.AlimtalkWebhookUpdate();
            AssertAlimtalk("PUT", "/alimtalk/webhook");
            Assert.Equal("{}", _server.LastRequest.Body);
        }

        [Fact]
        public async Task WebhookTestAndRotateSecret_AreSeparateFromOrderWebhook()
        {
            await _api.AlimtalkWebhookTest();
            // ⚠️ 주문 웹훅(/webhook/test) 과 완전히 별개 경로다
            AssertAlimtalk("POST", "/alimtalk/webhook/test");

            await _api.AlimtalkWebhookRotateSecret();
            AssertAlimtalk("POST", "/alimtalk/webhook/secret");
        }

        [Fact]
        public async Task WebhookDeliveries_SendsPaging()
        {
            await _api.AlimtalkWebhookDeliveries(new AlimtalkWebhookDeliveriesParams { Page = 2, Limit = 100 });
            AssertAlimtalk("GET", "/alimtalk/webhook/deliveries?page=2&limit=100");
        }

        #endregion

        #region role 고정

        [Fact]
        public async Task AlimtalkRequests_ForceUserRole_EvenWhenInstanceIsSupervisor()
        {
            _api.AsSupervisor();

            await _api.AlimtalkSenderList();
            AssertAlimtalk("GET", "/alimtalk/senders");

            await _api.AlimtalkMessageList();
            AssertAlimtalk("GET", "/alimtalk/messages");

            // 알림톡 밖의 엔드포인트는 인스턴스 role 을 그대로 쓴다 (알림톡 헤더가 전역을 바꾸지 않는다)
            await _api.GetMallSetting();
            Assert.Equal("supervisor", _server.LastRequest.Headers["BOOTPAY-ROLE"]);
        }

        #endregion
    }
}
