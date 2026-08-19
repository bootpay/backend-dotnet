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
