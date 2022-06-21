 
using System.Collections.Generic; 
using Newtonsoft.Json;

namespace Bootpay.models
{
    public class Extra
    {

        [JsonProperty("card_quota")]
        public string cardQuota; //카드 결제시 할부 기간 설정 (5만원 이상 구매시)
        [JsonProperty("seller_name")]
        public string sellerName; //노출되는 판매자명 설정
        [JsonProperty("delivery_day")]
        public int deliveryDay = 1; //배송일자
        public string locale = "ko"; //결제창 언어지원
        [JsonProperty("offer_period")]
        public string offerPeriod; //결제창 제공기간에 해당하는 string 값, 지원하는 PG만 적용됨
        [JsonProperty("display_cash_receipt")]
        public bool displayCashReceipt; // 현금영수증 보일지 말지.. 가상계좌 KCP 옵션
        [JsonProperty("deposit_expiration")]
        public string depositExpiration; //가상계좌 입금 만료일자 설정, yyyy-MM-dd

        [JsonProperty("app_scheme")]
        public string appScheme; //모바일 앱에서 결제 완료 후 돌아오는 옵션 ( 아이폰만 적용 )
        [JsonProperty("use_card_point")]
        public bool useCardPoint; //카드 포인트 사용 여부 (토스만 가능)
        [JsonProperty("direct_card")]
        public string directCard; //해당 카드로 바로 결제창 (토스만 가능)

        [JsonProperty("use_order_id")]
        public bool useOrderId; //가맹점 order_id로 PG로 전송

        [JsonProperty("international_card_only")]
        public bool internationalCardOnly; //해외 결제카드 선택 여부 (토스만 가능)

        [JsonProperty("phone_carrier")]
        public string phoneCarrier;  //본인인증 시 고정할 통신사명, SKT,KT,LGT 중 1개만 가능

        [JsonProperty("direct_app_card")]
        public bool directAppCard; //카드사앱으로 direct 호출

        [JsonProperty("direct_samsungpay")]
        public bool directSamsungpay; //삼성페이 바로 띄우기

        [JsonProperty("test_deposit")]
        public bool testDeposit;  //가상계좌 모의 입금

        [JsonProperty("enable_error_webhook")]
        public bool enableErrorWebhook;  //결제 오류시 Feedback URL로 webhook

        [JsonProperty("separately_confirmed")]
        public bool separatelyConfirmed; // confirm 이벤트를 호출할지 말지, false일 경우 자동승인

        [JsonProperty("confirm_only_rest_api")]
        public bool confirmOnlyRestApi; // REST API로만 승인 처리

        [JsonProperty("open_type")]
        public string openType; //페이지 오픈 type [iframe, popup, redirect] 중 택 1

        [JsonProperty("use_bootpay_inapp_sdk")]
        public bool useBootpayInappSdk; //native app에서는 redirect를 완성도있게 지원하기 위한 옵션

        [JsonProperty("redirect_url")]
        public string redirectUrl; //open_type이 redirect일 경우 페이지 이동할 URL ( 오류 및 결제 완료 모두 수신 가능 )

        [JsonProperty("display_success_result")]
        public bool displaySuccessResult; // 결제 완료되면 부트페이가 제공하는 완료창으로 보여주기 ( open_type이 iframe, popup 일때만 가능 )

        [JsonProperty("display_error_result")]
        public bool displayErrorResult; // 결제가 실패하면 부트페이가 제공하는 실패창으로 보여주기 ( open_type이 iframe, popup 일때만 가능 )

        [JsonProperty("disposable_cup_deposit")]
        public int disposableCupDeposit = 0; //배달대행 플랫폼을 위한 컵 보증급 가격

        [JsonProperty("card_easy_option")]
        public BootExtraCardEasyOption cardEasyOption;

        [JsonProperty("browser_open_type")]
        public List<BrowserOpenType> browserOpenType;

        [JsonProperty("use_welcomepayment")]
        public bool useWelcomepayment; //웰컴 재판모듈 진행시 true

                                                  //public bool escrow { get; set; }

        //[JsonProperty("expire_month")]
        //public int expireMonth { get; set; }
        //public List<int> quota { get; set; }

        //[JsonProperty("subscribe_test_payment")]
        //public bool subscribeTestPayment { get; set; }

        //[JsonProperty("disp_cash_result")]
        //public bool dispCashResult { get; set; }

        //[JsonProperty("offer_period")]
        //public bool offerPeriod { get; set; }

        //public string theme { get; set; }

        //[JsonProperty("custom_background")]
        //public string customBackground { get; set; }

        //[JsonProperty("custom_font_color")]
        //public string customFontColor { get; set; }
    }    
}
