# Bootpay ASP.NET 패키지

Bootpay 패키지는 ASP.NET 언어로 작성된 어플리케이션, 프레임워크 등에서 사용가능합니다.
.net standart 2.0 이상부터 지원합니다. 


* PG 결제창 연동은 클라이언트 라이브러리에서 수행됩니다. (Javascript, Android, iOS, React Native, Flutter 등)
* 결제 검증 및 취소, 빌링키 발급, 본인인증 등의 수행은 서버사이드에서 진행됩니다. (Java, PHP, Python, Ruby, Node.js, Go, ASP.NET 등)



- [기능](#기능)
- [Nuget 이용하여 설치하기](#nuget-이용하여-설치하기)
- [사용하기](#사용하기)
  - [1. 토큰 발급](#1-토큰-발급)
  - [2. 결제 검증](#2-결제-검증)
  - [3. 결제 취소 (전액 취소 / 부분 취소)](#3-결제-취소-전액-취소--부분-취소)
  - [4-1. 카드 빌링키 발급](#4-1-카드-빌링키-발급)
  - [4-2. 계좌 빌링키 발급](#4-2-계좌-빌링키-발급)
  - [4-3. 결제 요청하기](#4-3-결제-요청하기)
  - [4-4. 결제 예약하기](#4-4-결제-예약하기)
  - [4-5. 예약 조회하기](#4-5-예약-조회하기)
  - [4-6. 예약 취소하기](#4-6-예약-취소하기)
  - [4-7. 빌링키 삭제하기](#4-7-빌링키-삭제하기)
  - [4-8. 빌링키 조회하기](#4-8-빌링키-조회하기)
  - [5. 회원 토큰 발급요청](#5-회원-토큰-발급요청)
  - [6. 서버 승인 요청](#6-서버-승인-요청)
  - [7. 본인 인증 결과 조회](#7-본인-인증-결과-조회)
  - [8. 에스크로 이용시 PG사로 배송정보 보내기](#8-에스크로-이용시-pg사로-배송정보-보내기)
  - [9-1. 현금영수증 발행하기](#9-1-현금영수증-발행하기)
  - [9-2. 현금영수증 발행 취소](#9-2-현금영수증-발행-취소)
  - [9-3. 별건 현금영수증 발행](#9-3-별건-현금영수증-발행)
  - [9-4. 별건 현금영수증 발행 취소](#9-4-별건-현금영수증-발행-취소)
- [Example 프로젝트](#example-프로젝트)
- [Documentation](#documentation)
- [기술문의](#기술문의)
- [License](#license)


## Nuget 이용하여 설치하기  
1. 솔루션 탐색기(Solution Explorer) 열기 
2. 만드신 솔루션 프로젝트 우클릭 
3. Manage Nuget Packages 클릭 
4. Bootpay 검색 
5. Bootpay 클릭 후 설치 
 
# 사용하기 
BootpayExample.java
```cs

using System.Threading.Tasks;
using Bootpay;
using Microsoft.AspNetCore.Mvc;

String getToken() {
  BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
  var res = await api.GetAccessToken();

  string json = JsonConvert.SerializeObject(res,
          Newtonsoft.Json.Formatting.None,
          new JsonSerializerSettings
          {
              NullValueHandling = NullValueHandling.Ignore
          });

  return Ok(json);
}
```
함수 단위의 샘플 코드는 [이곳](https://github.com/bootpay/backend-dotnet/tree/main/Sample/Controllers)을 참조하세요.

## 1. 토큰 발급 

부트페이와 서버간 통신을 하기 위해서는 부트페이 서버로부터 토큰을 발급받아야 합니다.  
발급된 토큰은 30분간 유효하며, 최초 발급일로부터 30분이 지날 경우 토큰 발급 함수를 재호출 해주셔야 합니다.
```java 
BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
var res = await api.GetAccessToken();

string json = JsonConvert.SerializeObject(res,
        Newtonsoft.Json.Formatting.None,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

return Ok(json);
```

## 2. 결제 검증 
결제창 및 정기결제에서 승인/취소된 결제건에 대하여 올바른 결제건인지 서버간 통신으로 결제검증을 합니다.
```cs 
string receiptId = "62b12d7fd01c7e001ebc71de";

BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
await api.GetAccessToken();
var res = await api.GetReceipt(receiptId);

string json = JsonConvert.SerializeObject(res,
        Newtonsoft.Json.Formatting.None,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });


return Ok(json);
```

## 3. 결제 취소 (전액 취소 / 부분 취소)
price를 지정하지 않으면 전액취소 됩니다. 
* 휴대폰 결제의 경우 이월될 경우 이통사 정책상 취소되지 않습니다
* 정산받으실 금액보다 취소금액이 클 경우 PG사 정책상 취소되지 않을 수 있습니다. 이때 PG사에 문의하시면 되겠습니다.
* 가상계좌의 경우 CMS 특약이 되어있지 않으면 취소되지 않습니다. 그러므로 결제 테스트시에는 가상계좌로 테스트 하지 않길 추천합니다. 

부분취는 카드로 결제된 건만 가능하며, 일부 PG사만 지원합니다. 요청시 price에 금액을 지정하시면 되겠습니다. 
* (지원가능 PG사: 이니시스, kcp, 다날, 페이레터, 나이스페이, 카카오페이, 페이코)

간혹 개발사에서 실수로 여러번 부분취소를 보내서 여러번 취소되는 경우가 있기때문에, 부트페이에서는 부분취소 중복 요청을 막기 위해 cancel_id 라는 필드를 추가했습니다. cancel_id를 지정하시면, 해당 건에 대해 중복 요청방지가 가능합니다.  
```cs 
BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
await api.GetAccessToken();
var res = await api.receiptCancel(cancel);

string json = JsonConvert.SerializeObject(res,
        Newtonsoft.Json.Formatting.None,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });


return Ok(json);
```

## 4-1. 카드 빌링키 발급 
REST API 방식으로 고객으로부터 카드 정보를 전달하여, PG사에게 빌링키를 발급받을 수 있습니다. 
발급받은 빌링키를 저장하고 있다가, 원하는 시점, 원하는 금액에 결제 승인 요청하여 좀 더 자유로운 결제시나리오에 적용이 가능합니다.
* 비인증 정기결제(REST API) 방식을 지원하는 PG사만 사용 가능합니다. 
```cs 
Subscribe subscribe = new Subscribe();
subscribe.itemName = "정기결제 테스트 아이템";
subscribe.orderId = "" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
subscribe.pg = "nicepay"; 

subscribe.cardNo = "5570**********1074"; //실제 테스트시에는 *** 마스크처리가 아닌 숫자여야 함
subscribe.cardPw = "**"; //실제 테스트시에는 *** 마스크처리가 아닌 숫자여야 함
subscribe.expireYear = "**"; //실제 테스트시에는 *** 마스크처리가 아닌 숫자여야 함
subscribe.expireMonth = "**"; //실제 테스트시에는 *** 마스크처리가 아닌 숫자여야 함
subscribe.identifyNumber = ""; //주민등록번호 또는 사업자 등록번호 (- 없이 입력)

BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
await api.GetAccessToken();
var res = await api.getBillingKey(subscribe);

string json = JsonConvert.SerializeObject(res,
        Newtonsoft.Json.Formatting.None,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

return Ok(json);
```

## 4-2. 계좌 빌링키 발급
REST API 방식으로 고객의 계좌 정보를 전달하여, PG사에게 빌링키 발급을 요청합니다. 요청 후 빌링키가 바로 발급되진 않고, 출금동의 확인 절차까지 진행해야 빌링키가 발급됩니다.
먼저 빌링키를 요청합니다.
```cs 
Subscribe subscribe = new Subscribe();
subscribe.orderName = "정기결제 테스트 아이템";
subscribe.subscriptionId = "" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
subscribe.pg = "nicepay";

subscribe.username = "홍길동";
subscribe.bankName = "국민";
subscribe.identityNo = "901014"; 
subscribe.phone = "01012341234"; 
subscribe.bankAccount = "67560123422472";


BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
await api.GetAccessToken();
var res = await api.GetBillingKeyTransfer(subscribe);

string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
        Newtonsoft.Json.Formatting.None,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });


return Ok(json);
```

이후 빌링키 발급 요청시 응답받은 receipt_id로, 출금 동의 확인을 요청합니다. 
```cs 
BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
await api.GetAccessToken();
var res = await api.PublishBillingKeyTransfer("6655e139d79bea0da31c05e5");

string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
        Newtonsoft.Json.Formatting.None,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });


return Ok(json);
```
출금 동의가 확인되면 응답값으로 빌링키가 발급됩니다. 


 
## 4-3. 결제 요청하기
발급된 빌링키로 원하는 시점에 원하는 금액으로 결제 승인 요청을 할 수 있습니다. 잔액이 부족하거나 도난 카드 등의 특별한 건이 아니면 PG사에서 결제를 바로 승인합니다.

```cs 
SubscribePayload payload = new SubscribePayload();
payload.billingKey = "615d00f0238684001f60241e";
payload.itemName = "아이템01";
payload.price = 1000;
payload.orderId = "" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();


BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
await api.GetAccessToken();
var res = await api.requestSubscribe(payload);

string json = JsonConvert.SerializeObject(res,
        Newtonsoft.Json.Formatting.None,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

return Ok(json);
```

## 4-4. 결제 예약하기 
원하는 시점에 4-1로 결제 승인 요청을 보내도 되지만, 빌링키 발급 이후에 바로 결제 예약 할 수 있습니다. (빌링키당 최대 10건)
```cs 
SubscribePayload payload = new SubscribePayload();
payload.billingKey = "615d00f0238684001f60241e";
payload.itemName = "아이템01";
payload.price = 1000;
payload.orderId = "" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
payload.executeAt = (DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() / 1000) + 10;

BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
await api.GetAccessToken();
var res = await api.reserveSubscribe(payload);

string json = JsonConvert.SerializeObject(res,
        Newtonsoft.Json.Formatting.None,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

return Ok(json);
```

## 4-5. 예약 조회하기
빌링키로 예약된 결제건을 취소합니다.
```cs 
string reserveId = "6490149ca575b40024f0b70d";

BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
await api.GetAccessToken();
var res = await api.ReserveSubscribeLookup(reserveId);

string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
        Newtonsoft.Json.Formatting.None,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

return Ok(json);
```

## 4-6. 예약 취소하기
빌링키로 예약된 결제건을 취소합니다.
```cs 
string reserveId = "615d08a67b5ba4002011cd41";

BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
await api.GetAccessToken();
var res = await api.reserveCancelSubscribe(reserveId);

string json = JsonConvert.SerializeObject(res,
        Newtonsoft.Json.Formatting.None,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

return Ok(json);
```


## 4-7. 빌링키 삭제하기 
발급된 빌링키로 더 이상 사용되지 않도록, 삭제 요청합니다.
```cs 
string billingKey = "62b12d7fd01c7e001ebc71de";

BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
await api.GetAccessToken();
var res = await api.DestroyBillingKey(billingKey);

string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
        Newtonsoft.Json.Formatting.None,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });


return Ok(json);
```


## 4-8. 빌링키 조회하기 
(빌링키 발급 완료시 리턴받았던 receipt_id에 한정) 어떤 빌링키였는지 조회합니다. 
```cs 
string receiptId = "62b12d7fd01c7e001ebc71de";

BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
await api.GetAccessToken();
var res = await api.LookupBillingKey(receiptId);

string json = JsonConvert.SerializeObject(res,
        Newtonsoft.Json.Formatting.None,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });


return Ok(json);
```

5. (ㅇㅇ페이) 회원 토큰 발급요청 
(부트페이 단독) 부트페이에서 제공하는 간편결제창, 생체인증 기반의 결제 사용을 위해서는 개발사에서 회원 고유번호를 관리해야하며, 해당 회원에 대한 사용자 토큰을 발급합니다.
이 토큰값을 기반으로 클라이언트에서 결제요청 하시면 되겠습니다.
```cs 
UserToken userToken = new UserToken();
userToken.userId = "1234";

BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
await api.GetAccessToken();
var res = await api.getUserToken(userToken);

string json = JsonConvert.SerializeObject(res,
        Newtonsoft.Json.Formatting.None,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

return Ok(json);
``` 

## 6. 서버 승인 요청 
결제승인 방식은 클라이언트 승인 방식과, 서버 승인 방식으로 총 2가지가 있습니다.

클라이언트 승인 방식은 javascript나 native 등에서 confirm 함수에서 진행하는 일반적인 방법입니다만, 경우에 따라 서버 승인 방식이 필요할 수 있습니다.

필요한 이유 
1. 100% 안정적인 결제 후 고객 안내를 위해 - 클라이언트에서 PG결제 진행 후 승인 완료될 때 onDone이 수행되지 않아 (인터넷 환경 등), 결제 이후 고객에게 안내하지 못할 수 있습니다  
2. 단일 트랜잭션의 개념이 필요할 경우 - 재고파악이 중요한 커머스를 운영할 경우 트랜잭션 개념이 필요할 수 있겠으며, 이를 위해서는 서버 승인을 사용해야 합니다. 

```cs 
string receiptId = "62b13138d01c7e001bbc71d9";

BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
await api.GetAccessToken();
var res = await api.Confirm(receiptId);

string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
        Newtonsoft.Json.Formatting.None,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });


return Ok(json);
```

## 7. 본인 인증 결과 조회 
다날 본인인증 후 결과값을 조회합니다. 
다날 본인인증에서 통신사, 외국인여부, 전화번호 이 3가지 정보는 다날에 추가로 요청하셔야 받으실 수 있습니다.
```cs 
string receiptId = "";

BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
await api.GetAccessToken();
var res = await api.Certificate(receiptId);

string json = JsonConvert.SerializeObject(res,
        Newtonsoft.Json.Formatting.None,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });


return Ok(json);
```


8. (에스크로 이용시) PG사로 배송정보 보내기
현금 거래에 한해 구매자의 안전거래를 보장하는 방법으로, 판매자와 구매자의 온라인 전자상거래가 원활하게 이루어질 수 있도록 중계해주는 매매보호서비스입니다. 국내법에 따라 전자상거래에서 반드시 적용이 되어 있어야합니다. PG에서도 에스크로 결제를 지원하며, 에스크로 결제 사용을 원하시면 PG사 가맹시에 에스크로결제를 미리 얘기하고나서 진행을 하시는 것이 수월합니다.

PG사로 배송정보( 이니시스, KCP만 지원 )를 보내서 에스크로 상태를 변경하는 API 입니다.
```cs 
Shipping shipping = new Shipping();
shipping.receiptId = "628ae7ffd01c7e001e9b6066";
shipping.trackingNumber = "123456";
shipping.deliveryCorp = "CJ대한통운";
ShippingUser user = new ShippingUser();
user.username = "홍길동";
user.phone = "01000000000";
user.address = "서울특별시 종로구";
user.zipcode = "08490";
shipping.user = user;

BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
await api.GetAccessToken();


var res = await api.PutShippingStart(shipping);

string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
        Newtonsoft.Json.Formatting.None,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });

return Ok(json);
```

## 9-1. 현금영수증 발행하기 
bootpay api를 통해 결제된 건에 대하여 현금영수증을 발행합니다. 
```cs 
CashReceipt cashReceipt = new CashReceipt();
cashReceipt.receiptId = "62e0f11f1fc192036b1b3c92"; 

cashReceipt.username = "테스트";
cashReceipt.email = "test@bootpay.co.kr";
cashReceipt.phone = "01000000000";

cashReceipt.identityNo = "01000000000";
cashReceipt.cashReceiptType = "소득공제";


//BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
BootpayApi api = new BootpayApi(Constants.dev_application_id, Constants.dev_private_key, BootpayObject.MODE_DEVELOPMENT);
await api.GetAccessToken();


var res = await api.RequestCashReceiptByBootpay(cashReceipt);

string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
        Newtonsoft.Json.Formatting.None,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });


return Ok(json);
```

## 9-2. 현금영수증 발행 취소 
9-1을 통해 발행한 현금영수증을 취소합니다. 
```cs 
Cancel cancel = new Cancel();
cancel.receiptId = "62e0f11f1fc192036b1b3c92";
cancel.cancelUsername = "관리자";
cancel.cancelMessage = "테스트 결제";

//cancel.price = 1000.0; //부분취소 요청시
//cancel.cancelId = "12342134"; //부분취소 요청시, 중복 부분취소 요청하는 실수를 방지하고자 할때 지정
//RefundData refund = new RefundData(); // 가상계좌 환불 요청시, 단 CMS 특약이 되어있어야만 환불요청이 가능하다.
//refund.account = "675601012341234"; //환불계좌
//refund.accountholder = "홍길동"; //환불계좌주
//refund.bankcode = BankCode.getCode("국민은행");//은행코드
//cancel.refund = refund;

//BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
BootpayApi api = new BootpayApi(Constants.dev_application_id, Constants.dev_private_key, BootpayObject.MODE_DEVELOPMENT);
var token = await api.GetAccessToken();


var res = await api.RequestCashReceiptCancelByBootpay(cancel);

string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
        Newtonsoft.Json.Formatting.None,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });


return Ok(json);
```

## 9-3. (별건) 현금영수증 발행  
부트페이 결제와 상관없이 금액, 상품명, 현금영수증 발행정보 등을 보내 현금영수증을 발행하는 API 입니다
```cs 
CashReceipt cashReceipt = new CashReceipt();
cashReceipt.pg = "토스";
cashReceipt.price = 1000;
cashReceipt.orderName = "테스트";
cashReceipt.cashReceiptType = "소득공제";
cashReceipt.identityNo = "01000000000";

cashReceipt.orderId = "" + DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
cashReceipt.purchasedAt = DateTime.Now.AddSeconds(100).ToString("yyyy-MM-dd'T'HH:mm:ss zzz");


//BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
BootpayApi api = new BootpayApi(Constants.dev_application_id, Constants.dev_private_key, BootpayObject.MODE_DEVELOPMENT);

await api.GetAccessToken();


var res = await api.RequestCashReceipt(cashReceipt);

string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
        Newtonsoft.Json.Formatting.None,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });


return Ok(json);
```

## 9-4. (별건) 현금영수증 발행 취소 
9-3을 통해 발행한 현금영수증을 취소합니다.
```cs 
Cancel cancel = new Cancel();
cancel.receiptId = "62f5bbb91fc192036f9f4c05";
cancel.cancelUsername = "관리자";
cancel.cancelMessage = "테스트 결제";

//cancel.price = 1000.0; //부분취소 요청시
//cancel.cancelId = "12342134"; //부분취소 요청시, 중복 부분취소 요청하는 실수를 방지하고자 할때 지정
//RefundData refund = new RefundData(); // 가상계좌 환불 요청시, 단 CMS 특약이 되어있어야만 환불요청이 가능하다.
//refund.account = "675601012341234"; //환불계좌
//refund.accountholder = "홍길동"; //환불계좌주
//refund.bankcode = BankCode.getCode("국민은행");//은행코드
//cancel.refund = refund;

//BootpayApi api = new BootpayApi(Constants.application_id, Constants.private_key);
BootpayApi api = new BootpayApi(Constants.dev_application_id, Constants.dev_private_key, BootpayObject.MODE_DEVELOPMENT);
var token = await api.GetAccessToken();


var res = await api.RequestCashReceiptCancel(cancel);

string json = JsonConvert.SerializeObject(await res.Content.ReadAsStringAsync(),
        Newtonsoft.Json.Formatting.None,
        new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });


return Ok(json);
```

## Example 프로젝트

[적용한 샘플 프로젝트](https://github.com/bootpay/backend-dotnet/tree/main/Sample)을 참조해주세요

## Documentation

[부트페이 개발매뉴얼](https://developer.bootpay.co.kr/)을 참조해주세요

## 기술문의

[부트페이 홈페이지](https://www.bootpay.co.kr) 우측 하단 채팅을 통해 기술문의 주세요!

## License

[MIT License](https://opensource.org/licenses/MIT).
