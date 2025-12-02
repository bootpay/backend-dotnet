# Bootpay .NET SDK 테스트

## 빠른 실행

```bash
cd /Users/taesupyoon/bootpay/server/sdk/dotnet
dotnet run --project Test/Test.csproj
```

## 상세 실행 방법

### 1. PG API 테스트

```bash
# 1) Program.cs 열어서 테스트할 메서드 주석 해제
# 2) 실행
dotnet run --project Test/Test.csproj
```

### 2. Commerce API 테스트

```bash
# 1) Program.cs에서 아래 주석 해제
#    await CommerceExample.Run();
#
# 2) CommerceExample.cs에서 테스트할 메서드 주석 해제
#
# 3) 실행
dotnet run --project Test/Test.csproj
```

## 테스트 키

### PG API (`Test/Program.cs`)

```csharp
// Development
const string DEV_APP_ID = "59bfc738e13f337dbd6ca48a";
const string DEV_PRIVATE_KEY = "pDc0NwlkEX3aSaHTp/PPL/i8vn5E/CqRChgyEp/gHD0=";

// Production
const string PROD_APP_ID = "5b8f6a4d396fa665fdc2b5ea";
const string PROD_PRIVATE_KEY = "rm6EYECr6aroQVG2ntW0A6LpWnkTgP4uQ3H18sDDUYw=";
```

### Commerce API (`Test/CommerceExample.cs`)

```csharp
// Development
const string DEV_CLIENT_KEY = "hxS-Up--5RvT6oU6QJE0JA";
const string DEV_SECRET_KEY = "r5zxvDcQJiAP2PBQ0aJjSHQtblNmYFt6uFoEMhti_mg=";
```

## PG API 테스트 항목

`Program.cs`의 `Main` 메서드에서 주석 해제하여 테스트:

```csharp
await GoGetToken();                    // 토큰 발급
// await GetReceipt();                 // 결제 조회
// await ReceiptCancel();              // 결제 취소
// await GetBillingKey();              // 빌링키 발급 (카드)
// await GetBillingKeyTransfer();      // 빌링키 발급 (계좌이체)
// await PublishBillingKeyTransfer();  // 빌링키 발급 승인
// await RequestSubscribe();           // 정기결제 요청
// await ReserveSubscribe();           // 예약 정기결제
// await ReserveCancelSubscribe();     // 예약 정기결제 취소
// await DestroyBillingKey();          // 빌링키 삭제
// await GetUserToken();               // 사용자 토큰 발급
// await Confirm();                    // 결제 승인
// await Certificate();                // 본인인증 조회
// await RequestAuthentication();      // 본인인증 요청
// await ConfirmAuthentication();      // 본인인증 승인
// await RealarmAuthentication();      // 본인인증 재알림
// await ShippingStart();              // 배송 시작 (에스크로)
// await RequestCashReceipt();         // 현금영수증 발급 (자체)
// await RequestCashReceiptCancel();   // 현금영수증 취소 (자체)
// await RequestCashReceiptByBootpay();        // 현금영수증 발급 (부트페이)
// await RequestCashReceiptCancelByBootpay();  // 현금영수증 취소 (부트페이)
```

## Commerce API 테스트 항목

`Program.cs`의 `Main` 메서드에서 주석 해제:

```csharp
// === Commerce API 테스트 ===
// await CommerceExample.Run();
```

`CommerceExample.cs`의 `Run` 메서드에서 개별 API 주석 해제:

```csharp
// User API
// await UserJoin();        // 사용자 등록
// await UserToken();       // 사용자 토큰 발급
// await UserCheckExist();  // 사용자 존재 확인
// await UserList();        // 사용자 목록
// await UserDetail();      // 사용자 상세
// await UserUpdate();      // 사용자 수정
// await UserDelete();      // 사용자 삭제

// UserGroup API
// await UserGroupCreate();     // 그룹 생성
// await UserGroupList();       // 그룹 목록
// await UserGroupDetail();     // 그룹 상세
// await UserGroupUpdate();     // 그룹 수정
// await UserGroupUserCreate(); // 그룹에 사용자 추가
// await UserGroupUserDelete(); // 그룹에서 사용자 제거

// Product API
// await ProductCreate();  // 상품 생성
// await ProductList();    // 상품 목록
// await ProductDetail();  // 상품 상세
// await ProductUpdate();  // 상품 수정
// await ProductStatus();  // 상품 상태 변경
// await ProductDelete();  // 상품 삭제

// Order API
// await OrderList();    // 주문 목록
// await OrderDetail();  // 주문 상세

// OrderSubscription API
// await OrderSubscriptionList();        // 구독 목록
// await OrderSubscriptionDetail();      // 구독 상세
// await OrderSubscriptionPause();       // 구독 일시정지
// await OrderSubscriptionResume();      // 구독 재개
// await OrderSubscriptionTermination(); // 구독 해지
```

## 구조

Java SDK 예제와 동일한 구조:

```
Test/
├── Program.cs         # PG API 예제 (BootpayExample.java 대응)
├── CommerceExample.cs # Commerce API 예제 (store/*.java 대응)
└── test.md            # 테스트 가이드
```
