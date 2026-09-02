using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bootpay.Commerce.Models
{
    #region 발송 (POST /v1/alimtalk/send · /send/bulk · DELETE /send/{receipt_id})

    /// <summary>
    /// 단건 발송 파라미터 (POST /v1/alimtalk/send)
    ///
    /// ⚠️ 실제로 카카오톡이 발송되고 과금된다. 샌드박스가 없다.
    /// - 멱등: 같은 (프로젝트, RefId) 로 재요청하면 기존 receipt 를 그대로 돌려준다.
    /// - 필수 변수: 템플릿 응답의 required_variables 를 모두 채워야 한다. 하나라도 비면 3017 로 거부된다.
    /// - 채널: SenderKey(공개키)로 지정한다. 생략하면 프로젝트 연동 채널로 해석하며,
    ///   연동 채널이 둘 이상일 때만 필수다 (ksp_id 는 내부 문서 id 라 발송 API 에 쓰지 않는다).
    /// </summary>
    public class AlimtalkSendParams
    {
        /// <summary>템플릿 코드 (필수)</summary>
        [JsonProperty("template_code")]
        public string TemplateCode { get; set; }

        /// <summary>수신번호 (필수)</summary>
        [JsonProperty("to")]
        public string To { get; set; }

        /// <summary>{ company_name: "부트페이몰", user_name: "홍길동" } 형태의 치환값</summary>
        [JsonProperty("variables")]
        public Dictionary<string, object> Variables { get; set; }

        /// <summary>가맹점 발송 식별자 — 멱등 키로 쓰인다</summary>
        [JsonProperty("ref_id")]
        public string RefId { get; set; }

        /// <summary>
        /// 알림톡 실패 시 문자(LMS) 대체발송 여부.
        /// ⚠️ 미지정(null)과 false 는 다르다 — 미지정이면 프로젝트 기본값을 따르고, false 는 명시적으로 끈다.
        /// 켜면 발신번호가 등록돼 있어야 하며 없으면 3030 으로 거부된다.
        /// </summary>
        [JsonProperty("fallback")]
        public bool? Fallback { get; set; }

        /// <summary>예약 발송 시각(ISO8601). 생략하면 즉시 발송</summary>
        [JsonProperty("reserved_at")]
        public string ReservedAt { get; set; }

        /// <summary>발신 채널 공개키</summary>
        [JsonProperty("sender_key")]
        public string SenderKey { get; set; }

        /// <summary>가맹점 사용자 식별자</summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }

    /// <summary>벌크 발송 수신자</summary>
    public class AlimtalkSendBulkRecipient
    {
        /// <summary>수신번호 (필수)</summary>
        [JsonProperty("to")]
        public string To { get; set; }

        /// <summary>수신자별 멱등 키</summary>
        [JsonProperty("ref_id")]
        public string RefId { get; set; }

        /// <summary>수신자별 치환값</summary>
        [JsonProperty("variables")]
        public Dictionary<string, object> Variables { get; set; }
    }

    /// <summary>
    /// 벌크 발송 파라미터 (POST /v1/alimtalk/send/bulk) — 1요청 = N수신자
    ///
    /// ⚠️ 수신자 수만큼 실제 발송되고 과금된다.
    /// - 쿼터를 넘으면 요청 시점에 전체 거부된다(3022) — 일부만 나가지 않는다.
    /// - 수신거부 번호는 skipped 이며 과금되지 않고 발송 기록도 만들지 않는다.
    /// - Fallback 은 요청 단위로 한 번만 판정한다 — 발신번호가 없으면 요청 전체가 3030 으로 거부된다.
    /// </summary>
    public class AlimtalkSendBulkParams
    {
        /// <summary>템플릿 코드 (필수)</summary>
        [JsonProperty("template_code")]
        public string TemplateCode { get; set; }

        /// <summary>수신자 목록 (필수)</summary>
        [JsonProperty("recipients")]
        public List<AlimtalkSendBulkRecipient> Recipients { get; set; }

        /// <summary>문자(LMS) 대체발송 여부. 미지정(null)과 false 는 다르다.</summary>
        [JsonProperty("fallback")]
        public bool? Fallback { get; set; }

        /// <summary>예약 발송 시각(ISO8601)</summary>
        [JsonProperty("reserved_at")]
        public string ReservedAt { get; set; }

        /// <summary>발신 채널 공개키</summary>
        [JsonProperty("sender_key")]
        public string SenderKey { get; set; }

        /// <summary>가맹점 사용자 식별자</summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }

    #endregion

    #region 발송내역·집계 (GET /v1/alimtalk/messages 계열)

    /// <summary>
    /// 발송내역 조회 파라미터 (GET /v1/alimtalk/messages)
    /// ⚠️ 기간 기본값은 최근 30일이고 최대 조회 폭은 92일이다 — 초과분은 거부하지 않고 시작일을 당겨 잘라낸다.
    ///    실제 적용된 구간은 응답의 period 로 확인한다.
    /// </summary>
    public class AlimtalkMessageListParams
    {
        /// <summary>템플릿 코드</summary>
        public string TemplateCode { get; set; }

        /// <summary>requested · success · failed · canceled</summary>
        public string Status { get; set; }

        /// <summary>발송 시 넘긴 멱등키</summary>
        public string RefId { get; set; }

        /// <summary>수신번호 (하이픈 무관, 정확 매칭)</summary>
        public string To { get; set; }

        /// <summary>조회 시작일</summary>
        public string SAt { get; set; }

        /// <summary>조회 종료일</summary>
        public string EAt { get; set; }

        /// <summary>페이지</summary>
        public int? Page { get; set; }

        /// <summary>서버 기본 20, 최대 100</summary>
        public int? Limit { get; set; }
    }

    /// <summary>기간 집계 조회 파라미터 (GET /v1/alimtalk/messages/stats)</summary>
    public class AlimtalkMessageStatsParams
    {
        /// <summary>조회 시작일</summary>
        public string SAt { get; set; }

        /// <summary>조회 종료일</summary>
        public string EAt { get; set; }
    }

    #endregion

    #region 공식 템플릿 카탈로그 (GET/POST /v1/alimtalk/official 계열)

    /// <summary>
    /// 공식 템플릿 검색 파라미터 (GET /v1/alimtalk/official)
    /// Keyword 는 본문·이름·분류를 부분일치(대소문자 무시)로 훑는다.
    /// </summary>
    public class AlimtalkOfficialListParams
    {
        /// <summary>서버는 q 를 먼저 보고 없으면 keyword 를 본다 — SDK 는 정본 키인 q 로 보낸다</summary>
        public string Keyword { get; set; }

        /// <summary>분류</summary>
        public string Category { get; set; }

        /// <summary>BA(기본형)·EX(부가정보형) 만 존재한다 — 그룹 템플릿이라 AD/MI 는 쓸 수 없다</summary>
        public string MsgType { get; set; }

        /// <summary>페이지</summary>
        public int? Page { get; set; }

        /// <summary>서버 기본 20, 최대 100 으로 clamp</summary>
        public int? Per { get; set; }

        /// <summary>주면 그 채널의 변수 예문 사전으로 variable_examples 를 채워 준다(표시용)</summary>
        public string KspId { get; set; }
    }

    /// <summary>
    /// 공식 템플릿 추천 파라미터 (POST /v1/alimtalk/official/recommend)
    /// 유사도 score(0~1) 내림차순으로 돌려준다.
    /// </summary>
    public class AlimtalkOfficialRecommendParams
    {
        /// <summary>보내려는 문구 (필수)</summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>분류</summary>
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>서버 기본 5</summary>
        [JsonProperty("limit")]
        public int? Limit { get; set; }

        /// <summary>변수 예문을 채워 볼 채널 ID</summary>
        [JsonProperty("ksp_id")]
        public string KspId { get; set; }
    }

    #endregion

    #region 자체 템플릿 (/v1/alimtalk/templates 계열)

    /// <summary>
    /// 자체 템플릿 목록 조회 파라미터 (GET /v1/alimtalk/templates)
    /// ⚠️ 페이지네이션이 없다 — 필터에 걸린 템플릿을 한 번에 모두 돌려준다.
    /// </summary>
    public class AlimtalkTemplateListParams
    {
        /// <summary>
        /// 검수상태 필터 — 1 REG(등록) / 2 REQ(검수요청) / 3 APR(승인) / 4 KRR(등록거절) / 5 REJ(승인반려).
        /// 숫자·숫자문자열·벤더 문자열("APR" 등)을 모두 받는다. 해석 못 하는 값은 필터 없음으로 떨어진다.
        /// </summary>
        public string Ins { get; set; }

        /// <summary>latest(기본)·oldest·code</summary>
        public string Sort { get; set; }

        /// <summary>코드·이름·본문·분류 부분일치</summary>
        public string Keyword { get; set; }
    }

    /// <summary>
    /// 자체 템플릿 생성·수정이 공유하는 본문 필드.
    /// 여기 명시되지 않은 값은 <see cref="Attrs"/> 에 담으면 서버로 그대로 전달된다
    /// (NodeJS SDK 의 index signature · Ruby SDK 의 **attrs 와 같은 자리).
    /// </summary>
    public class AlimtalkTemplateParams
    {
        /// <summary>템플릿 이름</summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>본문. 변수는 #{변수명} 형식이고 템플릿 전체에서 최대 40개다.</summary>
        [JsonProperty("content")]
        public string Content { get; set; }

        /// <summary>버튼 목록</summary>
        [JsonProperty("buttons")]
        public List<Dictionary<string, object>> Buttons { get; set; }

        /// <summary>BA(기본형)·EX(부가정보형, TemplateExtra 필수)·AD(채널추가형)·MI(복합형)</summary>
        [JsonProperty("msg_type")]
        public string MsgType { get; set; }

        /// <summary>NONE·TEXT(강조표기형)·IMAGE(이미지형)·ITEM_LIST(아이템리스트형)</summary>
        [JsonProperty("emphasize_type")]
        public string EmphasizeType { get; set; }

        /// <summary>TEXT 강조표기형 필수 (50자)</summary>
        [JsonProperty("emphasize_title")]
        public string EmphasizeTitle { get; set; }

        /// <summary>TEXT 강조표기형 필수 (40자)</summary>
        [JsonProperty("emphasize_subtitle")]
        public string EmphasizeSubtitle { get; set; }

        /// <summary>EX(부가정보형) 필수</summary>
        [JsonProperty("template_extra")]
        public string TemplateExtra { get; set; }

        /// <summary>아이템리스트형 헤더</summary>
        [JsonProperty("template_header")]
        public string TemplateHeader { get; set; }

        /// <summary>아이템리스트형 하이라이트</summary>
        [JsonProperty("item_highlight")]
        public Dictionary<string, object> ItemHighlight { get; set; }

        /// <summary>ITEM_LIST 필수 — list 는 2~10개</summary>
        [JsonProperty("template_item")]
        public Dictionary<string, object> TemplateItem { get; set; }

        /// <summary>이미지 URL</summary>
        [JsonProperty("image_url")]
        public string ImageUrl { get; set; }

        /// <summary>AlimtalkTemplateImage() 로 올려 받은 URL</summary>
        [JsonProperty("storage_image_url")]
        public string StorageImageUrl { get; set; }

        /// <summary>보안 템플릿 여부</summary>
        [JsonProperty("security_flag")]
        public bool? SecurityFlag { get; set; }

        /// <summary>분류</summary>
        [JsonProperty("category")]
        public string Category { get; set; }

        /// <summary>태그</summary>
        [JsonProperty("tags")]
        public List<string> Tags { get; set; }

        /// <summary>변수 예문(표시용). 주면 모든 변수에 예문이 있어야 한다(없으면 3017).</summary>
        [JsonProperty("examples")]
        public Dictionary<string, string> Examples { get; set; }

        /// <summary>템플릿 코드</summary>
        [JsonProperty("template_code")]
        public string TemplateCode { get; set; }

        /// <summary>
        /// 여기 명시되지 않은 필드를 그대로 서버로 보낸다.
        /// 위 속성과 같은 키를 담으면 이 값이 이긴다 (Ruby SDK 의 .merge(attrs) 와 같은 순서).
        /// </summary>
        [JsonIgnore]
        public Dictionary<string, object> Attrs { get; set; }
    }

    /// <summary>
    /// 자체 템플릿 생성 파라미터 (POST /v1/alimtalk/templates)
    /// ⚠️ Register 를 false 로 주지 않으면 대행사·카카오에 실제 등록된다(되돌리려면 삭제해야 한다).
    /// </summary>
    public class AlimtalkTemplateCreateParams : AlimtalkTemplateParams
    {
        /// <summary>채널 ID (필수)</summary>
        [JsonProperty("ksp_id")]
        public string KspId { get; set; }

        /// <summary>
        /// false 로 주면 초안만 만든다 — 확인 후 AlimtalkTemplateRegister() 로 올리는 것을 권장한다.
        /// </summary>
        [JsonProperty("register")]
        public bool? Register { get; set; }
    }

    /// <summary>
    /// 자체 템플릿 수정 파라미터 (PUT /v1/alimtalk/templates/{template_id})
    /// ⚠️ 부분 수정이 아니다. 보내지 않은 필드는 null 로 덮어써지므로 항상 전체 필드를 보낸다.
    /// ⚠️ 수정 가능 상태는 초안 / REG(등록) / REJ(승인반려) / KRR(등록거절) 뿐이다 — APR·REQ 는 거부된다.
    /// StorageImageUrl 을 빈 값으로 보내면 이미지 삭제로 처리되어 벤더에도 전달된다.
    /// </summary>
    public class AlimtalkTemplateUpdateParams : AlimtalkTemplateParams
    {
    }

    /// <summary>
    /// 템플릿 목록 내보내기 파라미터 (GET /v1/alimtalk/templates/export)
    /// ⚠️ SDK 기본 Format 은 json 이다 — 서버 기본은 csv 지만 csv 본문은 JSON 이 아니다.
    /// 1회 5,000건을 넘으면 3031 로 거부되므로 채널·상태 필터로 좁힌다.
    /// </summary>
    public class AlimtalkTemplateExportParams
    {
        /// <summary>json(SDK 기본)·csv</summary>
        public string Format { get; set; }

        /// <summary>private(기본, 내 채널 자체 템플릿)·official(공식 카탈로그)·all</summary>
        public string Scope { get; set; }

        /// <summary>채널 ID</summary>
        public string KspId { get; set; }

        /// <summary>검수 상태</summary>
        public string Status { get; set; }

        /// <summary>본문 포함 여부</summary>
        public bool? IncludeContent { get; set; }
    }

    #endregion

    #region 발신프로필 (GET /v1/alimtalk/categories · /senders 계열)

    /// <summary>
    /// OTP 발송 파라미터 (POST /v1/alimtalk/senders/otp)
    /// ⚠️ 채널 관리자 휴대폰으로 실제 문자가 나간다.
    /// </summary>
    public class AlimtalkSenderOtpParams
    {
        /// <summary>카카오채널 검색용 아이디 (@ 포함, 필수)</summary>
        [JsonProperty("yellow_id")]
        public string YellowId { get; set; }

        /// <summary>채널 관리자 휴대폰 번호 (필수)</summary>
        [JsonProperty("phone")]
        public string Phone { get; set; }
    }

    /// <summary>
    /// 발신프로필 등록 파라미터 (POST /v1/alimtalk/senders)
    /// ⚠️ 카카오에 발신프로필이 실제 등록된다. 같은 YellowId 를 다시 등록하면 기존 프로필을 재사용한다(dedup).
    /// 등록 성공 시 그룹키 등록까지 서버가 수행하므로 공식 카탈로그 전체를 바로 발송할 수 있다.
    /// </summary>
    public class AlimtalkSenderCreateParams
    {
        /// <summary>AlimtalkSenderOtp() 로 받은 인증번호 (필수)</summary>
        [JsonProperty("otp")]
        public string Otp { get; set; }

        /// <summary>카카오채널 검색용 아이디 (필수)</summary>
        [JsonProperty("yellow_id")]
        public string YellowId { get; set; }

        /// <summary>채널 관리자 휴대폰 번호 (필수)</summary>
        [JsonProperty("phone")]
        public string Phone { get; set; }

        /// <summary>AlimtalkSenderCategories() 로 조회한 카테고리 코드 (필수)</summary>
        [JsonProperty("category_code")]
        public string CategoryCode { get; set; }
    }

    #endregion

    #region 수신거부 (/v1/alimtalk/optouts 계열)

    /// <summary>
    /// 수신거부 목록 조회 파라미터 (GET /v1/alimtalk/optouts)
    /// Phone 은 숫자만 남겨 부분일치로 찾는다(정확 매칭이 아니다). 50건 단위로 페이징된다.
    /// </summary>
    public class AlimtalkOptoutListParams
    {
        /// <summary>수신번호 (부분일치)</summary>
        public string Phone { get; set; }

        /// <summary>페이지</summary>
        public int? Page { get; set; }
    }

    /// <summary>수신거부 등록 파라미터 (POST /v1/alimtalk/optouts) — 같은 번호를 다시 등록해도 멱등이다.</summary>
    public class AlimtalkOptoutCreateParams
    {
        /// <summary>수신거부할 번호 (필수)</summary>
        [JsonProperty("phone")]
        public string Phone { get; set; }

        /// <summary>사유</summary>
        [JsonProperty("reason")]
        public string Reason { get; set; }
    }

    /// <summary>
    /// 수신거부 사전 확인 파라미터 (POST /v1/alimtalk/optouts/check)
    /// 단건(Phone)·다건(Phones) 모두 받는다.
    /// ⚠️ 1회 최대 1,000건이고 넘으면 -48 이다(중복은 서버가 제거).
    /// </summary>
    public class AlimtalkOptoutCheckParams
    {
        /// <summary>다건 확인</summary>
        [JsonProperty("phones")]
        public List<string> Phones { get; set; }

        /// <summary>단건 확인</summary>
        [JsonProperty("phone")]
        public string Phone { get; set; }
    }

    #endregion

    #region 알림톡 웹훅 (/v1/alimtalk/webhook 계열)

    /// <summary>
    /// 알림톡 웹훅 이벤트 코드.
    /// ⚠️ 주문·구독 통합 웹훅(WebhookSendTest)과 완전히 별개다.
    /// </summary>
    public static class AlimtalkWebhookEvent
    {
        /// <summary>발송 접수 (기본 미구독)</summary>
        public const int Requested = 300;

        /// <summary>전달 성공</summary>
        public const int Delivered = 301;

        /// <summary>전달 실패</summary>
        public const int Failed = 302;

        /// <summary>예약 취소</summary>
        public const int Canceled = 303;

        /// <summary>문자(LMS) 대체발송 전환</summary>
        public const int Fallback = 304;

        /// <summary>검수 승인</summary>
        public const int InspectApproved = 310;

        /// <summary>검수 반려</summary>
        public const int InspectRejected = 311;

        /// <summary>수신거부 등록 (기본 미구독)</summary>
        public const int Optout = 320;
    }

    /// <summary>
    /// 웹훅 설정 저장 파라미터 (PUT /v1/alimtalk/webhook)
    /// Url 은 https 만 허용한다(아니면 3028). 최초 저장 시 서명 시크릿이 자동 발급된다.
    /// </summary>
    public class AlimtalkWebhookUpdateParams
    {
        /// <summary>수신 URL (https 만 허용)</summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// 구독할 이벤트 코드(<see cref="AlimtalkWebhookEvent"/>). 목록에 없는 값은 저장 시 조용히 버려진다(유령 구독 방지).
        /// 비우면 기본 구독셋(301·302·303·304·310·311)이 적용된다.
        /// </summary>
        [JsonProperty("events")]
        public List<int> Events { get; set; }

        /// <summary>활성화 여부</summary>
        [JsonProperty("enabled")]
        public bool? Enabled { get; set; }
    }

    /// <summary>웹훅 전송 이력 조회 파라미터 (GET /v1/alimtalk/webhook/deliveries)</summary>
    public class AlimtalkWebhookDeliveriesParams
    {
        /// <summary>페이지</summary>
        public int? Page { get; set; }

        /// <summary>서버 기본 20, 최대 100</summary>
        public int? Limit { get; set; }
    }

    #endregion
}
