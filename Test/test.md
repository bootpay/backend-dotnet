# .NET SDK 테스트 실행 가이드

## 환경 설정

`Test/Config.cs` 파일에서 환경을 설정합니다:

```csharp
// "production" 또는 "development"로 설정
public const string CurrentEnv = "production";
```

## 빠른 실행

```bash
cd /Users/taesupyoon/bootpay/server/sdk/dotnet
dotnet run --project Test/Test.csproj
```

## 상세 실행 방법

### Visual Studio / Rider에서 실행
1. `Test/Program.cs` 파일 열기
2. `Main()` 메서드에서 실행할 테스트 주석 해제
3. Run 버튼 클릭 또는 `F5`

### 개별 테스트 활성화

`Test/Program.cs`의 `Main()` 메서드에서 원하는 테스트의 주석을 해제합니다:

```csharp
static async Task Main(string[] args)
{
    bootpay = new Bootpay(Config.PG.GetApplicationId(), Config.PG.GetPrivateKey());

    await GetAccessToken();              // 토큰 발급 (필수)
    // await ReceiptPayment();           // 결제 조회
    // await Confirm();                  // 결제 승인
    // await Cancel();                   // 결제 취소
    // await LookupSubscribeBilling();   // 빌링키 조회 (receipt_id)
    // await LookupBilling();            // 빌링키 조회 (billing_key)
    // await RequestSubscribe();         // 빌링키 발급
    // await RequestSubscribePayment();  // 정기결제 실행
    // await ReserveSubscribe();         // 예약 결제
    // await ReserveCancelSubscribe();   // 예약 결제 취소
    // await DestroyBillingKey();        // 빌링키 삭제
    // await GetUserToken();             // 사용자 토큰 발급
    // await Certificate();              // 본인인증 조회
    // await ShippingStart();            // 에스크로 배송시작
    // await CashReceiptPublishOnReceipt();  // 결제건 현금영수증 발행
    // await CashReceiptCancelOnReceipt();   // 결제건 현금영수증 취소
    // await RequestCashReceipt();           // 현금영수증 발행
    // await CancelCashReceipt();            // 현금영수증 취소
}
```

## 테스트 데이터

`Test/Config.cs`의 `TestData` 클래스에서 테스트 데이터를 관리합니다:

```csharp
public static class TestData
{
    public const string ReceiptId = "628b2206d01c7e00209b6087";
    public const string ReceiptIdConfirm = "62876963d01c7e00209b6028";
    public const string ReceiptIdCash = "62e0f11f1fc192036b1b3c92";
    public const string ReceiptIdEscrow = "628ae7ffd01c7e001e9b6066";
    public const string ReceiptIdBilling = "62c7ccebcf9f6d001b3adcd4";
    public const string ReceiptIdTransfer = "66541bc4ca4517e69343e24c";
    public const string BillingKey = "628b2644d01c7e00209b6092";
    public const string BillingKey2 = "66542dfb4d18d5fc7b43e1b6";
    public const string ReserveId = "6490149ca575b40024f0b70d";
    public const string ReserveId2 = "628b316cd01c7e00219b6081";
    public const string UserId = "1234";
    public const string CertificateReceiptId = "61b009aaec81b4057e7f6ecd";
}
```

## Config 클래스 사용법

```csharp
// PG API 키 가져오기
string applicationId = Config.PG.GetApplicationId();
string privateKey = Config.PG.GetPrivateKey();

// Commerce API 키 가져오기
string clientKey = Config.Commerce.GetClientKey();
string secretKey = Config.Commerce.GetSecretKey();

// 테스트 데이터 사용
string receiptId = Config.TestData.ReceiptId;
string billingKey = Config.TestData.BillingKey;
```

## Commerce API 테스트

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
// await UserList();        // 사용자 목록
// await UserDetail();      // 사용자 상세

// UserGroup API
// await UserGroupCreate();     // 그룹 생성
// await UserGroupList();       // 그룹 목록

// Product API
// await ProductCreate();  // 상품 생성
// await ProductList();    // 상품 목록

// Order API
// await OrderList();    // 주문 목록
// await OrderDetail();  // 주문 상세

// OrderSubscription API
// await OrderSubscriptionList();        // 구독 목록
// await OrderSubscriptionPause();       // 구독 일시정지
// await OrderSubscriptionResume();      // 구독 재개
// await OrderSubscriptionTermination(); // 구독 해지
```

## 폴더 구조

```
Test/
├── Config.cs          # 환경 설정 및 테스트 데이터
├── Program.cs         # PG API 테스트 예제
├── CommerceExample.cs # Commerce API 테스트 예제
├── test.md            # 테스트 가이드
└── store/             # Commerce API 개별 테스트
    ├── User.cs
    ├── UserGroup.cs
    ├── Product.cs
    ├── Order.cs
    └── ...
```
