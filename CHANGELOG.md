### 2.8.0

#### 알림톡 v1 API 35종 추가 (NodeJS 2.13.0 parity)

카카오 알림톡 API(`/v1/alimtalk/…`)를 SDK 에 추가했다. 봇 동기화가 dotnet 에는 들어오지 않아 다른 6개 SDK
(nodejs·go·java·python·php·ruby)만 갖고 있던 것을 기준 SDK(NodeJS)를 보고 포팅했다. 발송·발송내역·공식 카탈로그·
자체 템플릿·발신프로필·수신거부·알림톡 웹훅 7개 영역이며 `commerce.Alimtalk*` 로 접근한다.

- `AlimtalkSend` / `AlimtalkSendBulk` / `AlimtalkSendCancel`
  - ⚠️ `Fallback` 은 **미지정(null)과 false 가 다르다**. 미지정이면 프로젝트 기본값을 따르고 `false` 는
    문자(LMS) 대체발송을 명시적으로 끈다. `NullValueHandling.Ignore` 가 null 만 걷어내므로 `false` 는 그대로 전달된다.
  - 멱등은 `RefId` 로만 성립한다 — 같은 (프로젝트, `ref_id`) 로 재요청하면 기존 receipt 를 돌려준다.
- `AlimtalkMessageList` / `AlimtalkMessageStats` / `AlimtalkMessageDetail`
- `AlimtalkOfficialList` / `AlimtalkOfficialRecommend` / `AlimtalkOfficialDetail`
  - `Keyword` 는 서버 정본 키인 **`q`** 로 전송한다 (서버는 `q` 를 먼저 보고 없으면 `keyword` 를 본다).
- `AlimtalkTemplate…` — `List` / `Create` / `Detail` / `Update` / `Delete` / `Register` / `Inspect` /
  `Export` / `Image` / `HighlightImage`
  - `Export` 의 **기본 `Format` 을 `json` 으로 둔다.** 서버 기본은 `csv` 지만 csv 본문은 JSON 이 아니라
    JSON 을 기대하는 호출부에서 파싱이 깨진다. `Format = "csv"` 를 주면 `Accept: */*` 로 원문을 받는다.
  - 본문 이미지(2:1, 가로 500px↑)와 하이라이트 썸네일(1:1, 가로 108px↑)은 **규격이 다른 별개 endpoint** 다.
  - `AlimtalkTemplateParams.Attrs` 에 담은 키는 명시 속성 위로 그대로 전송된다 (ruby SDK 의 `**attrs` 자리).
- `AlimtalkSenderCategories` / `Otp` / `Create` / `List` / `Detail` / `Release` / `VariableExamples`
- `AlimtalkOptoutList` / `Create` / `Check` / `Release` — 전역 차단은 해제되지 않고 `global_blocked: true` 로 알려 준다.
- `AlimtalkWebhookDetail` / `Update` / `Test` / `RotateSecret` / `Deliveries`
  - ⚠️ 주문·구독 통합 웹훅(`WebhookSendTest`, `POST /v1/webhook/test`)과 **완전히 별개 경로**다.

공통 규약 (다른 Commerce 메서드와 다른 점):

- **`Idempotency-Key` 를 싣지 않는다.** 알림톡 API 는 이 헤더를 읽지 않는다 — 붙이면 서버가 주지 않는
  보장을 주는 것처럼 보인다. 그래서 알림톡 메서드에는 `idempotencyKey` 인자가 없다.
- **`BOOTPAY-ROLE` 은 항상 `user`.** 알림톡 스코프 키가 전부 `user:alimtalk_*` 라, 인스턴스가
  `AsSupervisor()` 로 설정돼 있어도 알림톡 요청만 user 로 나간다 (인스턴스 role 은 바뀌지 않는다).
- ⚠️ **샌드박스가 없다.** 발송·OTP·발신프로필 등록·템플릿 등록/검수는 모두 실제로 나가고 과금된다.

내부 추가: `BootpayCommerceObject.SendMultipartFileAsync()` — 상품 등록의 `images[0]` 인덱싱과 달리
서버가 단일 필드명(`image`)을 읽는 템플릿 이미지 업로드용이다.

#### 테스트

- `Bootpay.Tests/Wire/CommerceAlimtalkWireTest.cs` 추가 (35건) — mock 서버로 35개 엔드포인트의 경로·쿼리·
  바디를 고정하고, 위 두 공통 규약(Idempotency-Key 부재 · role 고정)을 단정한다.
- `PgWireTest.RequestCashReceipt_OmitsPgWhenUnset_AndForwardsItWhenGiven` 추가 — 별건 현금영수증의 `pg` 가
  미지정이면 바디에서 빠지고(서버 기본 PG 로 발행), 주면 그대로 전달되는 것을 고정한다.
  dotnet 은 이미 그렇게 동작하고 있었으나 다른 SDK 가 2.x.1 에서 건 회귀 방지를 여기에도 맞췄다.

**기존 PG/Commerce 메서드·타입·응답 구조 변경 없음.**

### 2.7.0

#### `product.list` 의 조회 필터를 서버 실제 계약에 맞춤

서버(`v1/products_controller#index`)가 읽는 것은 **page · limit · keyword · category_id · ex_uid · sort** 뿐인데,
``ProductList()`` 은 정작 그중 `category_id` · `ex_uid` · `sort` 를 **보내지 않고**, 서버가 읽지 않는
`type` · `period_type` · `s_at` · `e_at` · `category_code` 만 보내고 있었다.
필터가 걸린 줄 알았는데 전체 목록이 돌아오는, `member_type` → `membership_type` 과 같은 조용한 실패였다.

- ``ProductListParams`` 에 **`CategoryId` / `ExUid` / `Sort`** 추가 — 서버가 읽는 값이라 이제 실제로 필터가 걸린다.
- 서버가 읽지 않는 `type` / `period_type` / `s_at` / `e_at` / `category_code` 는 **전송은 그대로 유지**하되(기존 호출 보호) 무시된다는 경고를 문서에 달았다.
  `type` 은 서버의 상품 타입 필터가 문자열(`subscription`/`discount`/`normal`)이라 이 숫자 필드와 값 체계 자체가 다르다.
- ⚠️ `keyword` 는 **26-08-26 서버 변경부터** 실제로 적용된다 (그 이전 배포본에서는 무시된다).
  같은 라운드에서 `GET /v1/products` 의 `sort` 가 항상 무시되던 서버 버그도 함께 고쳤다 — SDK 쪽 변경은 없다.


#### 누락 파라미터 보강 (Ruby SDK `d4c8989` parity)

동기화 봇이 dotnet 에는 이 변경도 내보내지 않아 손으로 맞춘다. 서버는 이미 읽고 있었는데 SDK 가 보내지 않아 쓸 수 없던 값들이다. **제거된 메서드·파라미터 없음.**

- ⚠️ **`UserList` 의 회원등급 필터 키 정정 (동작 변경).** 서버(`v1/users_controller#index`)가 읽는 이름은 `membership_type` 인데 `member_type` 을 보내고 있어 **필터가 에러 없이 조용히 무시되고 전체 목록이 돌아왔다.** `UserListParams.MembershipType` 을 추가하고, 기존 `MemberType` 은 `[Obsolete]` 별칭으로 남겨 `membership_type` 으로 실어 보낸다 (둘 다 주면 `MembershipType` 우선). 그동안 걸리지 않던 필터가 이제 실제로 걸리므로, 전체 목록을 기대하던 코드가 있다면 확인이 필요하다.
- `OrderSubscriptionListParams` 에 `OrderNumber` 추가 — `GET order_subscriptions?order_number=` 로 주문번호 역조회.
- `OrderSubscriptionUpdateParams` 에 `Memo` 추가 — 구독 변경이력(`SUBSCRIPTION_ACTION_UPDATE`)에 남길 사유다.
- `MallProductListParams` 에 `ExUid` 추가 — `GET products?ex_uid=` 로 외부 UID 조회.
- `ProductDetail(productId, userJwt = null, idempotencyKey = null)` — `ProductDetailMall` 과 동작을 맞췄다 (`Bootpay-User-JWT` 헤더 + `Idempotency-Key` 자동 부착). 인자 하나짜리 `ProductDetail(productId)` 호출은 그대로 동작한다.
  ⚠️ **현재 서버(`V1::ProductsController`)는 이 헤더를 읽지 않는다** — `decode_user_jwt` before_action 이 `orders` · `users/sessions` 컨트롤러에만 걸려 있고, `ProductDetailService` 도 `project` 와 `product_id` 만 받는다. 즉 지금은 회원 컨텍스트가 실제로 적용되지 않으며, 서버가 지원을 추가하면 SDK 변경 없이 동작한다.
- `OrderList` 의 빈 `status` / `payment_status` / `order_subscription_ids` 미전송은 이미 지원 중이라 변경 없다.
- `Bootpay.Tests/Wire/CommerceWireTest.cs` 에 회귀 테스트 7건을 추가했다.

### 2.6.0

#### 구독 가격 변경 · 범위로 회차조정 (Ruby SDK `9832af9` parity)

동기화 봇이 dotnet 에는 이 변경을 내보내지 않아 6개 서버 SDK 중 dotnet 만 이 기능이 빠져 있었다. 손으로 맞춘다.

- `OrderSubscriptionUpdateParams` 에 `Price` 추가 — 회차별 결제 금액의 **기준금액**이다. 바꾸면 결제예정(READY) 회차의 청구액이 즉시 다시 계산되고, 이후 회차도 이 금액으로 만들어진다. 이미 결제된 회차는 그대로다. 0 이하는 서버가 거절한다. 특정 회차만 가감하려면 `OrderSubscriptionAdjustmentCreate` 를 쓴다.
- `CommerceOrderSubscriptionAdjustment` 에 `DurationFrom` / `DurationTo` / `IsUnlimited` 추가 — 회차를 범위로 지정한다.
  - `Duration = 5` → 5회차 한 건만
  - `DurationFrom = 3, DurationTo = 7` → 3~7회차 각각 한 건씩 (총 5건)
  - `DurationFrom = 3, IsUnlimited = true` → 3회차부터 계약 끝까지 (레코드는 1건, `DurationTo` 는 무시)
  - 상한은 계약 총회차이며, 총회차가 무제한인 계약은 60회차까지다. 이미 결제가 끝난 회차는 거절되고, 범위 중 한 회차라도 최종 금액이 음수면 전부 거절된다 (부분 반영 없음).
- `IsUnlimited` 는 `bool?` 이라 명시적 `false` 도 전송된다. 미지정 필드는 기존대로 바디에서 빠지고, `duration` 은 기준 SDK 와 동일하게 기본값 1 이 항상 실린다.
- 요청 경로·동사·scope 는 변경 없다. 순수 추가라 기존 호출 결과는 바뀌지 않는다. `Bootpay.Tests/Wire/CommerceWireTest.cs` 에 회귀 테스트 6건을 추가했다.

### 2.5.0

#### Commerce scope(BOOTPAY-ROLE) 정합성 (동작 변경)

서버(commerce-api)가 `scope_invalid!` 로 supervisor / manager scope 를 요구하는 10개 엔드포인트가 `BOOTPAY-ROLE: user` 로 나가고 있었다. 요청 단위로 올바른 scope 를 붙인다. Java SDK 3.3.0 · Ruby SDK 와 같은 규약이다.

- `OrderSubscriptionSupervisorApprove` / `...Reject` / `...Terminate` / `...Pause` / `...Resume` → **supervisor**
- `CategoryCreate` / `CategoryUpdate` / `CategoryDestroy` → **supervisor**
- `UserGroupUserCreate` / `UserGroupUserDelete` → **manager**

부수 효과로 이 10개 호출에 `Idempotency-Key` 가 자동 부착된다 (다른 supervisor 메서드·Ruby SDK 와 동일). 요청 경로·바디는 변경 없다.
⚠️ 그동안 이 API 들은 올바른 키로도 scope 오류로 거절됐다. 우회하려고 role 을 직접 조작하던 코드가 있다면 제거해도 된다.

- 위 메서드에 `string idempotencyKey = null` 선택 인자를 추가했다 (`OrderSubscriptionSupervisorCharge` 와 같은 방식). 지정하면 그 값이 `Idempotency-Key` 헤더로 나간다.

#### 테스트

- `Bootpay.Tests/Wire/LegacyWireRegressionTest.cs` 에 `using Bootpay.Commerce;` 가 빠져 있어 **테스트 프로젝트가 컴파일되지 않았다** (2.4.0 작업 환경에 요구 SDK 가 없어 실행 검증이 누락된 결과). 정정 후 전체 스위트가 돈다.
- `Bootpay.Tests/Wire/CommerceScopeTest.cs` 신설 — 10개 엔드포인트의 scope·Idempotency-Key 회귀.
- `CommerceWireTest.PerRequestRole_...` 이 "scope 미지정 endpoint" 예시로 `OrderSubscriptionSupervisorApprove` 를 쓰고 있어 `UserList` 로 교체했다.

### 2.4.0
- NodeJS SDK 2.9.0 parity.
- PG: 우선순위(순차) 결제 빌링키 조회 `LookupSequentialBillingKey(widgetKey, billingKey, userId)` 추가 — `GET subscribe/sequential_billing_key/{billing_key}?widget_key=&user_id=`.
- Commerce: 몰 설정 모듈 추가 (supervisor 전용) — `GetMallSetting`/`MallSettingDetail`, `UpdateMallSetting`/`MallSettingUpdate` (flatten 바디, null 값 미전송).
- Commerce: 테스트 웹훅 발송 `WebhookSendTest` 추가 — `POST webhook/test` (`header_content_type`).
- Commerce: 수시결제(온디맨드) charge_key 결제/해지 추가 (supervisor 전용) — `OrderSubscriptionSupervisorCharge` (`POST order_subscriptions/charge`), `OrderSubscriptionSupervisorChargeRevoke` (`DELETE order_subscriptions/charge`). charge_key 는 body 로만 전송 (URL/query 금지).
- Commerce: V1 Mall 회원 API 추가/정정 — `UserLoginMall` (login_id/password/corporate_type 바디로 정정), `UserSession`, `UserLogout`, `UserJoinMall(MallUserJoinParams)`, `UserJoinCheckMall`, `UidExist`. 세션 계열은 `Bootpay-User-JWT` 헤더 (값 있을 때만 부착).
- Commerce: V1 Mall 상품 조회 parity — `Products` 는 page/limit 기본 1/20 을 항상 전송, `MallProductListParams` 로 category_id/sort/user_jwt 지원. `ProductDetailMall` 에 userJwt 인자 추가.
- Commerce: 구독 변경요청 추가 — `OrderSubscriptionPurchase` (중도인수, `POST .../requests/ing/purchase`), `OrderSubscriptionTransfer` (이전/승계, `POST .../requests/ing/transfer`).
- Commerce: multipart 전송 정정 — 상품 이미지 필드를 `images[0]`, `images[1]` … 인덱싱으로 전송 (반복 `images` 는 서버가 배열로 받지 않음). 이미지 없으면 JSON 전송.
- Commerce: 인자·응답 규약 정정
  - `InvoiceList` 응답은 `{ list, count }` (`InvoiceListResponse` 타입 추가), limit 기본값 24, `InvoiceListParams` 에 cs_type/user_id/product_type/css_at/cse_at 추가. `InvoiceNotify` 의 sendTypes 선택화.
  - `OrderCancelActionParams` 에 정식 이름 `OrderCancellationRequestId` 추가 (구 `OrderCancelRequestHistoryId` 도 계속 동작), 대상 ID 는 URL 로만 전송. `message` 필드 추가.
  - `OrderSubscriptionAdjustmentDelete` 는 대상 ID 를 query 가 아니라 body 로 전송. `OrderSubscriptionAdjustmentUpdate` 에 `adjustments` 배열 지원 (duration 회차 단위 교체, 미지정시 1).
  - `UserGroupLimitParams` 에 limit_month_purchase/limit_week_purchase 추가. `OrderListParams` 에 search_date_from/to 추가. `OrderSubscriptionListParams` 에 search_date_from/to/status 추가. `OrderSubscriptionRequestListParams` 에 order_subscription_id/user_id/user_group_id 추가.
- Commerce: endpoint 별 `BOOTPAY-ROLE` scope 명시 — 상품 쓰기/그룹 한도는 `manager`, 구독 계약변경·조정항목·취소 승인/반려·charge·몰설정·요청 승인은 `supervisor`, 나머지는 `user`. 요청 목록/단건은 project_id 유무로 supervisor/user 분기. 요청별 role 헤더는 기본 role 을 덮어쓰지 않고 해당 요청에만 적용.
- Commerce: `Idempotency-Key` 헤더 자동 생성 (모든 신규/scope 지정 endpoint, `idempotencyKey` 인자로 직접 지정 가능).
- Commerce: multipart 스칼라 직렬화 정정 — bool 은 소문자 `true`/`false` 로 전송 (.NET 기본 `True`/`False` 는 서버(Rails)가 boolean 으로 캐스팅하지 못해 false 가 true 로 읽히는 위험), 숫자는 InvariantCulture (로케일 소수점 콤마 방지).
- Commerce: 파라미터 모델 필드 보강 — `OrderSubscriptionUpdateParams` 에 계약 변경 12필드 (product_id/product_option_id/order_name/total_subscription_duration/quantity/address_id/username/phone/email/use_free_trial/free_trial_day/service_start_at), `ProductStatusParams` 에 기간 8필드 (status_frozen/status_review/use_display_period/display_start_at/display_end_at/use_sale_period/sale_start_at/sale_end_at) 추가.
- 테스트: 로컬 mock 서버 기반 wire-format(URL·헤더·바디) 검증 테스트 추가 (`Bootpay.Tests/Wire`).
- 테스트: 라이브 API 를 호출하는 기존 스위트에 `[LiveFact]` 게이트 도입 — `BOOTPAY_ENV=development` 가 아니면 skip 되어 전체 실행 시 production 호출이 발생하지 않음 (네트워크 없는 테스트는 `[Fact]` 유지).

### 2.3.0
- 인증: client_key/secret_key Basic Auth 지원 (PG + Commerce 공통).
  - 기존 application_id/private_key Bearer 방식 하위 호환 유지.
  - `BootpayApi(applicationId, privateKey, clientKey, secretKey, mode)` 오버로드 추가 — ck/sk 가 함께 지정되면 ck/sk 우선.
  - ck/sk 모드는 매 요청 자동 Basic Auth 헤더 부착 — `GetAccessToken()` 은 합성 응답을 반환하며 `request/token` 호출이 발생하지 않음.
  - `BootpayObject` 가 토큰 부재 시 ck/sk Basic Auth 로 fallback (PG + Commerce 공통).
  - `Commerce.BootpayCommerceObject` 의 모든 호출이 ck/sk 로 Basic Auth 사용.
- Commerce: store / supervisor / order-subscription action API 추가 (NodeJS 2.6.0 parity round).
  - store 엔드포인트 + mall 별칭 alias 정리.
  - supervisor 의 order-subscription action API 추가.
- Wallet API 추가 (`GetUserWallets`, `RequestWalletPayment`) — 다음 메이저 버전에서 deprecated 처리 예정.
- 직렬화 버그 수정: `cancel_username` 오타, `bank_code` 직렬화, `.json` 접미사 전체 제거.
- 테스트 인프라: `.env` / `BOOTPAY_AUTH_MODE=new|legacy` / `BOOTPAY_ENV` 토글로 ck/sk · legacy 양쪽 검증.

### 2.2.0
- Commerce API 추가 (User, UserGroup, Product, Invoice, Order, OrderCancel, OrderSubscription, OrderSubscriptionBill, OrderSubscriptionAdjustment).
- Test 콘솔 프로젝트 추가.

### 2.1.0
- 계좌 자동결제 api 추가 

### 2.0.2
- 현금영수증 api 추가 

### 2.0.1
- shipping user model update

### 2.0.0
- v1 -> v2 update 

### 1.0.2 
- request link 버그 수정 

### 1.0.1
- example update

### 1.0.0
- first release
