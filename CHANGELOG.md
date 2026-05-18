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
