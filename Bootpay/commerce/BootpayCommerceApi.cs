using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bootpay.Commerce.Models;
using Bootpay.Commerce.Service;

namespace Bootpay.Commerce
{
    /// <summary>
    /// Bootpay Commerce API
    /// </summary>
    public class BootpayCommerceApi : BootpayCommerceObject
    {
        public BootpayCommerceApi(string clientKey, string secretKey, int mode = MODE_PRODUCTION)
            : base(clientKey, secretKey, mode) { }

        #region User (사용자)

        /// <summary>
        /// 사용자 토큰 발급
        /// </summary>
        public async Task<HttpResponseMessage> UserToken(string userId)
        {
            return await UserService.Token(this, userId);
        }

        /// <summary>
        /// 회원가입
        /// </summary>
        public async Task<HttpResponseMessage> UserJoin(CommerceUser user)
        {
            return await UserService.Join(this, user);
        }

        /// <summary>
        /// 중복 체크
        /// </summary>
        public async Task<HttpResponseMessage> UserCheckExist(string key, string value)
        {
            return await UserService.CheckExist(this, key, value);
        }

        /// <summary>
        /// 본인인증 데이터 조회
        /// </summary>
        public async Task<HttpResponseMessage> UserAuthenticationData(string standId)
        {
            return await UserService.AuthenticationData(this, standId);
        }

        /// <summary>
        /// 로그인
        /// </summary>
        public async Task<HttpResponseMessage> UserLogin(string loginId, string loginPw)
        {
            return await UserService.Login(this, loginId, loginPw);
        }

        /// <summary>
        /// 사용자 목록 조회
        /// </summary>
        public async Task<HttpResponseMessage> UserList(UserListParams listParams = null)
        {
            return await UserService.List(this, listParams);
        }

        /// <summary>
        /// 회원 로그인 (V1 Mall API — POST /v1/users/login)
        /// </summary>
        public async Task<HttpResponseMessage> UserLoginMall(MallUserLoginParams loginParams, string idempotencyKey = null)
        {
            return await UserService.MallLogin(this, loginParams, idempotencyKey);
        }

        /// <summary>
        /// 회원 로그인 (V1 Mall API alias) — corporate_type 미지정시 0 으로 전송된다
        /// </summary>
        public async Task<HttpResponseMessage> UserLoginMall(string loginId, string loginPw)
        {
            return await UserService.MallLogin(this, new MallUserLoginParams { LoginId = loginId, Password = loginPw });
        }

        /// <summary>
        /// 회원 세션 조회 (V1 Mall API — GET /v1/users/session)
        /// </summary>
        public async Task<HttpResponseMessage> UserSession(string userJwt = null, string idempotencyKey = null)
        {
            return await UserService.Session(this, userJwt, idempotencyKey);
        }

        /// <summary>
        /// 회원 로그아웃 (V1 Mall API — DELETE /v1/users/session)
        /// </summary>
        public async Task<HttpResponseMessage> UserLogout(string userJwt, string idempotencyKey = null)
        {
            return await UserService.Logout(this, userJwt, idempotencyKey);
        }

        /// <summary>
        /// 회원가입 (V1 Mall API — POST /v1/users/join)
        /// </summary>
        public async Task<HttpResponseMessage> UserJoinMall(MallUserJoinParams joinParams, string idempotencyKey = null)
        {
            return await UserService.MallJoin(this, joinParams, idempotencyKey);
        }

        /// <summary>
        /// 회원가입 (Mall API alias)
        /// </summary>
        public async Task<HttpResponseMessage> UserJoinMall(CommerceUser user)
        {
            return await UserJoin(user);
        }

        /// <summary>
        /// 회원가입 중복 체크 (V1 Mall API — GET /v1/users/join/{type}?pk={pk})
        /// type: email-exist, id-exist, phone-exist, uid-exist, group-business-number-exist
        /// </summary>
        public async Task<HttpResponseMessage> UserJoinCheckMall(string key, string value, string idempotencyKey = null)
        {
            return await UserService.JoinCheck(this, key, value, idempotencyKey);
        }

        /// <summary>
        /// 외부 uid(ex_uid) 중복 검사 (GET /v1/users/join/uid-exist?pk={uid})
        /// </summary>
        public async Task<HttpResponseMessage> UidExist(string uid, string idempotencyKey = null)
        {
            return await UserService.UidExist(this, uid, idempotencyKey);
        }

        /// <summary>
        /// 사용자 상세 조회
        /// </summary>
        public async Task<HttpResponseMessage> UserDetail(string userId)
        {
            return await UserService.Detail(this, userId);
        }

        /// <summary>
        /// 사용자 정보 수정
        /// </summary>
        public async Task<HttpResponseMessage> UserUpdate(CommerceUser user)
        {
            return await UserService.Update(this, user);
        }

        /// <summary>
        /// 사용자 삭제 (회원탈퇴)
        /// </summary>
        public async Task<HttpResponseMessage> UserDelete(string userId)
        {
            return await UserService.Delete(this, userId);
        }

        #endregion

        #region UserGroup (사용자 그룹)

        /// <summary>
        /// 사용자 그룹 생성
        /// </summary>
        public async Task<HttpResponseMessage> UserGroupCreate(CommerceUserGroup userGroup)
        {
            return await UserGroupService.Create(this, userGroup);
        }

        /// <summary>
        /// 사용자 그룹 목록 조회
        /// </summary>
        public async Task<HttpResponseMessage> UserGroupList(UserGroupListParams listParams = null)
        {
            return await UserGroupService.List(this, listParams);
        }

        /// <summary>
        /// 사용자 그룹 상세 조회
        /// </summary>
        public async Task<HttpResponseMessage> UserGroupDetail(string userGroupId)
        {
            return await UserGroupService.Detail(this, userGroupId);
        }

        /// <summary>
        /// 사용자 그룹 수정
        /// </summary>
        public async Task<HttpResponseMessage> UserGroupUpdate(CommerceUserGroup userGroup)
        {
            return await UserGroupService.Update(this, userGroup);
        }

        /// <summary>
        /// 그룹에 사용자 추가
        /// </summary>
        public async Task<HttpResponseMessage> UserGroupUserCreate(string userGroupId, string userId, string idempotencyKey = null)
        {
            return await UserGroupService.UserCreate(this, userGroupId, userId, idempotencyKey);
        }

        /// <summary>
        /// 그룹에서 사용자 제거
        /// </summary>
        public async Task<HttpResponseMessage> UserGroupUserDelete(string userGroupId, string userId, string idempotencyKey = null)
        {
            return await UserGroupService.UserDelete(this, userGroupId, userId, idempotencyKey);
        }

        /// <summary>
        /// 그룹 구매 한도 설정 — manager scope
        /// ⚠️ 한도는 이 전용 라우트로만 바뀐다 (update 로는 반영되지 않는다).
        /// </summary>
        public async Task<HttpResponseMessage> UserGroupLimit(UserGroupLimitParams limitParams, string idempotencyKey = null)
        {
            return await UserGroupService.Limit(this, limitParams, idempotencyKey);
        }

        /// <summary>
        /// 그룹 구독 합산청구(정산주기) 설정 — manager scope
        /// </summary>
        public async Task<HttpResponseMessage> UserGroupAggregateTransaction(UserGroupAggregateTransactionParams aggregateParams, string idempotencyKey = null)
        {
            return await UserGroupService.AggregateTransaction(this, aggregateParams, idempotencyKey);
        }

        #endregion

        #region Product (상품)

        /// <summary>
        /// 상품 목록 조회
        /// </summary>
        public async Task<HttpResponseMessage> ProductList(ProductListParams listParams = null)
        {
            return await ProductService.List(this, listParams);
        }

        /// <summary>
        /// 상품 생성 — manager scope
        /// </summary>
        public async Task<HttpResponseMessage> ProductCreate(CommerceProduct product, string idempotencyKey = null)
        {
            return await ProductService.Create(this, product, idempotencyKey);
        }

        /// <summary>
        /// 상품 생성 (이미지 파일 포함) — manager scope
        /// 이미지가 있으면 multipart/form-data (images[0], images[1] ... 인덱싱), 없으면 JSON 으로 전송된다.
        /// </summary>
        /// <param name="product">상품 정보</param>
        /// <param name="imagePaths">이미지 파일 경로 리스트</param>
        /// <param name="idempotencyKey">미지정시 자동 생성</param>
        public async Task<HttpResponseMessage> ProductCreateWithImages(CommerceProduct product, List<string> imagePaths, string idempotencyKey = null)
        {
            return await ProductService.CreateWithImages(this, product, imagePaths, idempotencyKey);
        }

        /// <summary>
        /// 상품 상세 조회
        ///
        /// <para><c>userJwt</c> 를 주면 회원 컨텍스트로 조회한다 (<c>Bootpay-User-JWT</c> 헤더).
        /// <c>ProductDetailMall</c> 과 uri·동작이 같다.</para>
        /// </summary>
        public async Task<HttpResponseMessage> ProductDetail(string productId, string userJwt = null, string idempotencyKey = null)
        {
            return await ProductService.Detail(this, productId, userJwt, idempotencyKey);
        }

        /// <summary>
        /// 상품 목록 조회 (V1 Mall API — GET /v1/products)
        /// page/limit 미지정시 각각 1 / 20 이 적용된다. MallProductListParams 로 category_id/sort/user_jwt 지정 가능.
        /// </summary>
        public async Task<HttpResponseMessage> Products(ProductListParams listParams = null, string idempotencyKey = null)
        {
            return await ProductService.Products(this, listParams, idempotencyKey);
        }

        /// <summary>
        /// 상품 상세 조회 (V1 Mall API — GET /v1/products/{product_id})
        /// </summary>
        public async Task<HttpResponseMessage> ProductDetailMall(string productId, string userJwt = null, string idempotencyKey = null)
        {
            return await ProductService.ProductDetail(this, productId, userJwt, idempotencyKey);
        }

        /// <summary>
        /// 상품 수정 — manager scope
        /// </summary>
        public async Task<HttpResponseMessage> ProductUpdate(CommerceProduct product, string idempotencyKey = null)
        {
            return await ProductService.Update(this, product, idempotencyKey);
        }

        /// <summary>
        /// 상품 상태 변경 — manager scope
        /// </summary>
        public async Task<HttpResponseMessage> ProductStatus(ProductStatusParams statusParams, string idempotencyKey = null)
        {
            return await ProductService.Status(this, statusParams, idempotencyKey);
        }

        /// <summary>
        /// 상품 삭제 — manager scope
        /// </summary>
        public async Task<HttpResponseMessage> ProductDelete(string productId, string idempotencyKey = null)
        {
            return await ProductService.Delete(this, productId, idempotencyKey);
        }

        #endregion

        #region Store (가맹점)

        /// <summary>
        /// 가맹점 기본 정보 조회
        /// </summary>
        public async Task<HttpResponseMessage> StoreInfo(string idempotencyKey = null)
        {
            return await StoreService.Info(this, idempotencyKey);
        }

        /// <summary>
        /// 가맹점 상세 정보 조회
        /// </summary>
        public async Task<HttpResponseMessage> StoreDetail(string idempotencyKey = null)
        {
            return await StoreService.Detail(this, idempotencyKey);
        }

        #endregion

        #region MallSetting (몰 설정)

        /// <summary>
        /// 몰 설정 조회 (GET /v1/mall-setting) — supervisor scope
        /// </summary>
        public async Task<HttpResponseMessage> GetMallSetting(string idempotencyKey = null)
        {
            return await MallSettingService.GetMallSetting(this, idempotencyKey);
        }

        /// <summary>
        /// 몰 설정 조회 alias
        /// </summary>
        public async Task<HttpResponseMessage> MallSettingDetail(string idempotencyKey = null)
        {
            return await GetMallSetting(idempotencyKey);
        }

        /// <summary>
        /// 몰 설정 수정 (PUT /v1/mall-setting) — supervisor scope
        /// 요청 바디는 flatten 형식이며, null 값은 전송되지 않는다.
        /// </summary>
        public async Task<HttpResponseMessage> UpdateMallSetting(MallSettingUpdateParams updateParams, string idempotencyKey = null)
        {
            return await MallSettingService.UpdateMallSetting(this, updateParams, idempotencyKey);
        }

        /// <summary>
        /// 몰 설정 수정 alias
        /// </summary>
        public async Task<HttpResponseMessage> MallSettingUpdate(MallSettingUpdateParams updateParams, string idempotencyKey = null)
        {
            return await UpdateMallSetting(updateParams, idempotencyKey);
        }

        #endregion

        #region Webhook (웹훅)

        /// <summary>
        /// 테스트 웹훅 발송 (POST /v1/webhook/test)
        /// 등록된 웹훅 URL 로 테스트 페이로드를 보내 연동을 확인할 때 쓴다.
        /// </summary>
        public async Task<HttpResponseMessage> WebhookSendTest(SendTestWebhookParams sendParams = null, string idempotencyKey = null)
        {
            return await WebhookService.SendTest(this, sendParams, idempotencyKey);
        }

        #endregion

        #region Invoice (청구서)

        /// <summary>
        /// 청구서 목록 조회 (GET /v1/invoices)
        /// 응답은 { list, count } 구조다 ({ items, total } 아님). limit 미지정시 24 를 보낸다.
        /// InvoiceListParams 로 cs_type/user_id/product_type/css_at/cse_at 지정 가능.
        /// </summary>
        public async Task<HttpResponseMessage> InvoiceList(ListParams listParams = null, string idempotencyKey = null)
        {
            return await InvoiceService.List(this, listParams, idempotencyKey);
        }

        /// <summary>
        /// 청구서 생성
        /// </summary>
        public async Task<HttpResponseMessage> InvoiceCreate(CommerceInvoice invoice)
        {
            return await InvoiceService.Create(this, invoice);
        }

        /// <summary>
        /// 청구서 알림 재발송 — sendTypes 미전달시 서버가 빈 배열로 처리한다.
        /// ⚠️ 실제 고객에게 알림이 발송되므로 테스트 호출 주의.
        /// </summary>
        public async Task<HttpResponseMessage> InvoiceNotify(string invoiceId, List<int> sendTypes = null, string idempotencyKey = null)
        {
            return await InvoiceService.Notify(this, invoiceId, sendTypes, idempotencyKey);
        }

        /// <summary>
        /// 청구서 상세 조회
        /// </summary>
        public async Task<HttpResponseMessage> InvoiceDetail(string invoiceId, string idempotencyKey = null)
        {
            return await InvoiceService.Detail(this, invoiceId, idempotencyKey);
        }

        #endregion

        #region Order (주문)

        /// <summary>
        /// 주문 목록 조회
        /// </summary>
        public async Task<HttpResponseMessage> OrderList(OrderListParams listParams = null)
        {
            return await OrderService.List(this, listParams);
        }

        /// <summary>
        /// 주문 상세 조회
        /// </summary>
        public async Task<HttpResponseMessage> OrderDetail(string orderId)
        {
            return await OrderService.Detail(this, orderId);
        }

        /// <summary>
        /// 월별 주문 조회
        /// </summary>
        public async Task<HttpResponseMessage> OrderMonth(string userGroupId, string searchDate)
        {
            return await OrderService.Month(this, userGroupId, searchDate);
        }

        #endregion

        #region OrderCancel (주문 취소)

        /// <summary>
        /// 취소 요청 목록 조회 — approve / reject / withdraw 에 넘길 order_cancellation_request_id 를 여기서 얻는다.
        /// </summary>
        public async Task<HttpResponseMessage> OrderCancelList(OrderCancelListParams listParams = null, string idempotencyKey = null)
        {
            return await OrderCancelService.List(this, listParams, idempotencyKey);
        }

        /// <summary>
        /// 취소 요청
        /// </summary>
        public async Task<HttpResponseMessage> OrderCancelRequest(OrderCancelParams cancelParams)
        {
            return await OrderCancelService.Request(this, cancelParams);
        }

        /// <summary>
        /// (구매자) 취소 요청 철회 — 정식 인자명은 order_cancellation_request_id (구 이름과 같은 값이다)
        /// </summary>
        public async Task<HttpResponseMessage> OrderCancelWithdraw(string orderCancelRequestHistoryId, string idempotencyKey = null)
        {
            return await OrderCancelService.Withdraw(this, orderCancelRequestHistoryId, idempotencyKey);
        }

        /// <summary>
        /// (관리자) 취소 승인 — supervisor scope
        /// </summary>
        public async Task<HttpResponseMessage> OrderCancelApprove(OrderCancelActionParams actionParams, string idempotencyKey = null)
        {
            return await OrderCancelService.Approve(this, actionParams, idempotencyKey);
        }

        /// <summary>
        /// (관리자) 취소 거절 — supervisor scope
        /// </summary>
        public async Task<HttpResponseMessage> OrderCancelReject(OrderCancelActionParams actionParams, string idempotencyKey = null)
        {
            return await OrderCancelService.Reject(this, actionParams, idempotencyKey);
        }

        #endregion

        #region OrderSubscription (정기구독)

        /// <summary>
        /// 정기구독 목록 조회
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionList(OrderSubscriptionListParams listParams = null)
        {
            return await OrderSubscriptionService.List(this, listParams);
        }

        /// <summary>
        /// 정기구독 상세 조회
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionDetail(string orderSubscriptionId)
        {
            return await OrderSubscriptionService.Detail(this, orderSubscriptionId);
        }

        /// <summary>
        /// 구독 계약 내용 변경 — supervisor scope. 바뀐 값만 보내면 된다.
        ///
        /// <para><c>Price</c> 는 회차별 결제 금액의 <b>기준금액</b>이다. 바꾸면 결제예정(READY) 회차의 청구액이
        /// 즉시 다시 계산되고, 이후 회차도 이 금액으로 만들어진다. 이미 결제된 회차는 그대로다. 0 이하는 받지 않는다.
        /// 특정 회차만 가감하려면 <c>OrderSubscriptionAdjustmentCreate</c> 를 쓴다.</para>
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionUpdate(OrderSubscriptionUpdateParams updateParams, string idempotencyKey = null)
        {
            return await OrderSubscriptionService.Update(this, updateParams, idempotencyKey);
        }

        /// <summary>
        /// 정기구독 일시정지 요청
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionPause(OrderSubscriptionPauseParams pauseParams, string idempotencyKey = null)
        {
            return await OrderSubscriptionService.Pause(this, pauseParams, idempotencyKey);
        }

        /// <summary>
        /// 정기구독 재개 요청
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionResume(OrderSubscriptionResumeParams resumeParams, string idempotencyKey = null)
        {
            return await OrderSubscriptionService.Resume(this, resumeParams, idempotencyKey);
        }

        /// <summary>
        /// 중도인수 요청 (POST /v1/order_subscriptions/requests/ing/purchase)
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionPurchase(OrderSubscriptionPurchaseParams purchaseParams, string idempotencyKey = null)
        {
            return await OrderSubscriptionService.Purchase(this, purchaseParams, idempotencyKey);
        }

        /// <summary>
        /// 구독 이전/승계 요청 (POST /v1/order_subscriptions/requests/ing/transfer)
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionTransfer(OrderSubscriptionTransferParams transferParams, string idempotencyKey = null)
        {
            return await OrderSubscriptionService.Transfer(this, transferParams, idempotencyKey);
        }

        /// <summary>
        /// 해지 수수료 계산
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionCalculateTerminationFee(string orderSubscriptionId = null, string orderNumber = null, string idempotencyKey = null)
        {
            return await OrderSubscriptionService.CalculateTerminationFee(this, orderSubscriptionId, orderNumber, idempotencyKey);
        }

        /// <summary>
        /// 주문번호로 해지 수수료 계산
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionCalculateTerminationFeeByOrderNumber(string orderNumber)
        {
            return await OrderSubscriptionService.CalculateTerminationFeeByOrderNumber(this, orderNumber);
        }

        /// <summary>
        /// 정기구독 해지 요청
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionTermination(OrderSubscriptionTerminationParams terminationParams, string idempotencyKey = null)
        {
            return await OrderSubscriptionService.Termination(this, terminationParams, idempotencyKey);
        }

        /// <summary>
        /// 수시결제(온디맨드) charge_key 즉시 결제 (POST /v1/order_subscriptions/charge) — supervisor scope
        /// charge_key 는 body 로만 전송된다 (URL/query 금지)
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionSupervisorCharge(SupervisorOrderSubscriptionChargeParams chargeParams, string idempotencyKey = null)
        {
            return await OrderSubscriptionService.SupervisorCharge(this, chargeParams, idempotencyKey);
        }

        /// <summary>
        /// 수시결제(온디맨드) charge_key 해지 (DELETE /v1/order_subscriptions/charge) — supervisor scope
        /// 해지 이후 해당 키로의 재결제는 불가능하다
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionSupervisorChargeRevoke(SupervisorOrderSubscriptionChargeRevokeParams revokeParams, string idempotencyKey = null)
        {
            return await OrderSubscriptionService.SupervisorChargeRevoke(this, revokeParams, idempotencyKey);
        }

        public async Task<HttpResponseMessage> OrderSubscriptionSupervisorApprove(string orderSubscriptionId, SupervisorOrderSubscriptionApproveParams approveParams = null, string idempotencyKey = null)
        {
            return await OrderSubscriptionService.SupervisorApprove(this, orderSubscriptionId, approveParams, idempotencyKey);
        }

        public async Task<HttpResponseMessage> OrderSubscriptionSupervisorReject(string orderSubscriptionId, SupervisorOrderSubscriptionRejectParams rejectParams = null, string idempotencyKey = null)
        {
            return await OrderSubscriptionService.SupervisorReject(this, orderSubscriptionId, rejectParams, idempotencyKey);
        }

        public async Task<HttpResponseMessage> OrderSubscriptionSupervisorTerminate(string orderSubscriptionId, SupervisorOrderSubscriptionTerminateParams terminateParams = null, string idempotencyKey = null)
        {
            return await OrderSubscriptionService.SupervisorTerminate(this, orderSubscriptionId, terminateParams, idempotencyKey);
        }

        public async Task<HttpResponseMessage> OrderSubscriptionSupervisorPause(string orderSubscriptionId, SupervisorOrderSubscriptionPauseParams pauseParams, string idempotencyKey = null)
        {
            return await OrderSubscriptionService.SupervisorPause(this, orderSubscriptionId, pauseParams, idempotencyKey);
        }

        public async Task<HttpResponseMessage> OrderSubscriptionSupervisorResume(string orderSubscriptionId, SupervisorOrderSubscriptionResumeParams resumeParams = null, string idempotencyKey = null)
        {
            return await OrderSubscriptionService.SupervisorResume(this, orderSubscriptionId, resumeParams, idempotencyKey);
        }

        #endregion

        #region OrderSubscriptionBill (정기구독 청구)

        /// <summary>
        /// 정기구독 빌(회차) 목록 조회 — page/limit 미지정시 각각 1 / 20 이 적용된다.
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionBillList(OrderSubscriptionBillListParams listParams = null, string idempotencyKey = null)
        {
            return await OrderSubscriptionBillService.List(this, listParams, idempotencyKey);
        }

        /// <summary>
        /// 정기구독 청구 상세 조회
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionBillDetail(string orderSubscriptionBillId)
        {
            return await OrderSubscriptionBillService.Detail(this, orderSubscriptionBillId);
        }

        /// <summary>
        /// 정기구독 청구 수정
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionBillUpdate(CommerceOrderSubscriptionBill orderSubscriptionBill)
        {
            return await OrderSubscriptionBillService.Update(this, orderSubscriptionBill);
        }

        #endregion

        #region Category (카테고리)

        /// <summary>
        /// 카테고리 트리 조회
        /// </summary>
        public async Task<HttpResponseMessage> CategoryList()
        {
            return await CategoryService.List(this);
        }

        /// <summary>
        /// 카테고리 단건 조회
        /// </summary>
        public async Task<HttpResponseMessage> CategoryDetail(string categoryId)
        {
            return await CategoryService.Detail(this, categoryId);
        }

        /// <summary>
        /// 카테고리 생성
        /// </summary>
        public async Task<HttpResponseMessage> CategoryCreate(CategoryCreateParams createParams, string idempotencyKey = null)
        {
            return await CategoryService.Create(this, createParams, idempotencyKey);
        }

        /// <summary>
        /// 카테고리 수정
        /// </summary>
        public async Task<HttpResponseMessage> CategoryUpdate(CategoryUpdateParams updateParams, string idempotencyKey = null)
        {
            return await CategoryService.Update(this, updateParams, idempotencyKey);
        }

        /// <summary>
        /// 카테고리 삭제
        /// </summary>
        public async Task<HttpResponseMessage> CategoryDestroy(string categoryId, string idempotencyKey = null)
        {
            return await CategoryService.Destroy(this, categoryId, idempotencyKey);
        }

        #endregion

        #region Coupon (쿠폰)

        /// <summary>
        /// 사용자 보유 쿠폰 목록
        /// </summary>
        public async Task<HttpResponseMessage> CouponList(CouponListParams listParams = null)
        {
            return await CouponService.List(this, listParams);
        }

        /// <summary>
        /// 다운로드 가능한 쿠폰 목록
        /// </summary>
        public async Task<HttpResponseMessage> CouponAvailable()
        {
            return await CouponService.Available(this);
        }

        /// <summary>
        /// 쿠폰 다운로드
        /// </summary>
        public async Task<HttpResponseMessage> CouponDownload(CouponDownloadParams downloadParams)
        {
            return await CouponService.Download(this, downloadParams);
        }

        #endregion

        #region Point (적립금)

        /// <summary>
        /// 적립금 잔액 조회
        /// </summary>
        public async Task<HttpResponseMessage> PointBalance()
        {
            return await PointService.Balance(this);
        }

        /// <summary>
        /// 적립금 내역 조회
        /// </summary>
        public async Task<HttpResponseMessage> PointTransactions(PointTransactionsParams listParams = null)
        {
            return await PointService.Transactions(this, listParams);
        }

        #endregion

        #region Cart (주문 미리보기)

        /// <summary>
        /// 주문 미리보기 (배송비/할인 권위적 계산)
        /// </summary>
        public async Task<HttpResponseMessage> CartOrderPreview(OrderPreviewParams previewParams = null)
        {
            return await CartService.OrderPreview(this, previewParams);
        }

        #endregion

        #region OrderSubscriptionRequest (정기구독 요청 조회/승인)

        /// <summary>
        /// 정기구독 요청 목록 조회 — project_id 가 있으면 supervisor, 없으면 user scope
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionRequestList(OrderSubscriptionRequestListParams listParams = null, string idempotencyKey = null)
        {
            return await OrderSubscriptionRequestService.List(this, listParams, idempotencyKey);
        }

        /// <summary>
        /// 정기구독 요청 단건 조회 — project_id 가 있으면 supervisor, 없으면 user scope
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionRequestDetail(string orderSubscriptionRequestHistoryId, string projectId = null, string idempotencyKey = null)
        {
            return await OrderSubscriptionRequestService.Detail(this, orderSubscriptionRequestHistoryId, projectId, idempotencyKey);
        }

        /// <summary>
        /// 정기구독 요청 승인/거절 (supervisor 전용) — approval: "approve" | "reject"
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionRequestUpdate(OrderSubscriptionRequestUpdateParams updateParams, string idempotencyKey = null)
        {
            return await OrderSubscriptionRequestService.Update(this, updateParams, idempotencyKey);
        }

        #endregion

        #region OrderSubscriptionAdjustment (정기구독 조정)

        /// <summary>
        /// 정기구독 조정 생성 — supervisor scope
        ///
        /// <para>회차 지정 방법 3가지 (아래로 갈수록 넓다).</para>
        /// <list type="bullet">
        ///   <item><description><c>Duration = 5</c> → 5회차 한 건만</description></item>
        ///   <item><description><c>DurationFrom = 3, DurationTo = 7</c> → 3~7회차 각각 한 건씩 (총 5건)</description></item>
        ///   <item><description><c>DurationFrom = 3, IsUnlimited = true</c> → 3회차부터 계약 끝까지 (레코드는 1건, <c>DurationTo</c> 는 무시)</description></item>
        /// </list>
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionAdjustmentCreate(string orderSubscriptionId, CommerceOrderSubscriptionAdjustment adjustment, string idempotencyKey = null)
        {
            return await OrderSubscriptionAdjustmentService.Create(this, orderSubscriptionId, adjustment, idempotencyKey);
        }

        /// <summary>
        /// 정기구독 조정 수정 — supervisor scope. 서버는 duration(회차) 단위로 adjustments 배열을 통째로 교체한다.
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionAdjustmentUpdate(OrderSubscriptionAdjustmentUpdateParams updateParams, string idempotencyKey = null)
        {
            return await OrderSubscriptionAdjustmentService.Update(this, updateParams, idempotencyKey);
        }

        /// <summary>
        /// 정기구독 조정 삭제 — supervisor scope. ⚠️ 대상 ID 는 query 가 아니라 body 로 전송된다.
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionAdjustmentDelete(string orderSubscriptionId, string orderSubscriptionAdjustmentId, string idempotencyKey = null)
        {
            return await OrderSubscriptionAdjustmentService.Delete(this, orderSubscriptionId, orderSubscriptionAdjustmentId, idempotencyKey);
        }

        #endregion

        #region Alimtalk (카카오 알림톡 v1)

        // 알림톡 API 는 Idempotency-Key 를 읽지 않고(멱등은 발송의 ref_id 로만 성립),
        // 스코프 키가 전부 user:alimtalk_* 라 BOOTPAY-ROLE 을 항상 user 로 고정해 보낸다.
        // 그래서 다른 Commerce 메서드와 달리 idempotencyKey 인자가 없다.

        /// <summary>
        /// 알림톡 단건 발송 (POST /v1/alimtalk/send)
        /// ⚠️ 실제로 카카오톡이 발송되고 과금된다. 샌드박스가 없다.
        /// ⚠️ Fallback 은 미지정(null)과 false 가 다르다 — 미지정이면 프로젝트 기본값을 따르고 false 는 명시적으로 끈다.
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkSend(AlimtalkSendParams sendParams)
        {
            return await AlimtalkSendService.Send(this, sendParams);
        }

        /// <summary>
        /// 알림톡 벌크 발송 (POST /v1/alimtalk/send/bulk) — 1요청 = N수신자
        /// ⚠️ 수신자 수만큼 실제 발송되고 과금된다. 쿼터를 넘으면 요청 시점에 전체 거부된다(3022).
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkSendBulk(AlimtalkSendBulkParams bulkParams)
        {
            return await AlimtalkSendService.Bulk(this, bulkParams);
        }

        /// <summary>
        /// 알림톡 예약 발송 취소 (DELETE /v1/alimtalk/send/{receipt_id})
        /// 접수(READY) 상태의 예약 건만 취소할 수 있다 — 이미 전송에 들어갔으면 3023 이다.
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkSendCancel(string receiptId)
        {
            return await AlimtalkSendService.Cancel(this, receiptId);
        }

        /// <summary>
        /// 알림톡 발송내역 목록 조회 (GET /v1/alimtalk/messages)
        /// ⚠️ 기간 기본값은 최근 30일, 최대 조회 폭은 92일이다 — 초과분은 시작일을 당겨 잘라내고 응답 period 로 알려 준다.
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkMessageList(AlimtalkMessageListParams listParams = null)
        {
            return await AlimtalkMessageService.List(this, listParams);
        }

        /// <summary>
        /// 알림톡 기간 집계 조회 (GET /v1/alimtalk/messages/stats)
        /// ⚠️ billing.unit_price_source 가 'default' 면 잠정 단가다(확정 청구액이 아니다).
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkMessageStats(AlimtalkMessageStatsParams statsParams = null)
        {
            return await AlimtalkMessageService.Stats(this, statsParams);
        }

        /// <summary>
        /// 알림톡 단건 발송 결과 조회 (GET /v1/alimtalk/messages/{receipt_id})
        /// 다른 프로젝트의 건이거나 없으면 404(3025).
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkMessageDetail(string receiptId)
        {
            return await AlimtalkMessageService.Detail(this, receiptId);
        }

        /// <summary>
        /// 공식 알림톡 템플릿 검색 (GET /v1/alimtalk/official)
        /// Keyword 는 서버 정본 키인 q 로 전송된다.
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkOfficialList(AlimtalkOfficialListParams listParams = null)
        {
            return await AlimtalkOfficialService.List(this, listParams);
        }

        /// <summary>
        /// 보내려는 문구로 공식 템플릿 추천받기 (POST /v1/alimtalk/official/recommend)
        /// 유사도 score(0~1) 내림차순으로 돌려준다.
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkOfficialRecommend(AlimtalkOfficialRecommendParams recommendParams)
        {
            return await AlimtalkOfficialService.Recommend(this, recommendParams);
        }

        /// <summary>
        /// 공식 알림톡 템플릿 상세 조회 (GET /v1/alimtalk/official/{code})
        /// </summary>
        /// <param name="code">공식 템플릿 코드</param>
        /// <param name="kspId">변수 예문을 채워 볼 채널 ID (선택)</param>
        public async Task<HttpResponseMessage> AlimtalkOfficialDetail(string code, string kspId = null)
        {
            return await AlimtalkOfficialService.Detail(this, code, kspId);
        }

        /// <summary>
        /// 내 자체 알림톡 템플릿 목록 조회 (GET /v1/alimtalk/templates)
        /// ⚠️ 페이지네이션이 없다 — 필터에 걸린 템플릿을 한 번에 모두 돌려준다.
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkTemplateList(AlimtalkTemplateListParams listParams = null)
        {
            return await AlimtalkTemplateService.List(this, listParams);
        }

        /// <summary>
        /// 자체 알림톡 템플릿 생성 (POST /v1/alimtalk/templates)
        /// ⚠️ Register 를 false 로 주지 않으면 대행사·카카오에 실제 등록된다(되돌리려면 삭제해야 한다).
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkTemplateCreate(AlimtalkTemplateCreateParams createParams)
        {
            return await AlimtalkTemplateService.Create(this, createParams);
        }

        /// <summary>
        /// 자체 알림톡 템플릿 상세 조회 (GET /v1/alimtalk/templates/{template_id})
        /// ⚠️ sync 는 서버 기본값이 true 라 조회만 해도 벤더 상태 동기화가 일어난다 — 초안 조회는 false 권장.
        /// </summary>
        /// <param name="templateId">템플릿 ID 또는 템플릿 코드</param>
        /// <param name="sync">벤더 동기화 여부 (선택, 서버 기본 true)</param>
        public async Task<HttpResponseMessage> AlimtalkTemplateDetail(string templateId, bool? sync = null)
        {
            return await AlimtalkTemplateService.Detail(this, templateId, sync);
        }

        /// <summary>
        /// 자체 알림톡 템플릿 수정 (PUT /v1/alimtalk/templates/{template_id})
        /// ⚠️ 부분 수정이 아니다 — 보내지 않은 필드는 null 로 덮어써지므로 항상 전체 필드를 보낸다.
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkTemplateUpdate(string templateId, AlimtalkTemplateUpdateParams updateParams)
        {
            return await AlimtalkTemplateService.Update(this, templateId, updateParams);
        }

        /// <summary>
        /// 자체 알림톡 템플릿 삭제 (DELETE /v1/alimtalk/templates/{template_id})
        /// ⚠️ 등록분은 대행사 삭제가 성공해야 삭제된다 — 승인(APR) 템플릿은 카카오가 거부한다(3013).
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkTemplateDelete(string templateId)
        {
            return await AlimtalkTemplateService.Delete(this, templateId);
        }

        /// <summary>
        /// 알림톡 템플릿 초안을 대행사에 등록 (POST /v1/alimtalk/templates/{template_id}/register)
        /// ⚠️ 대행사·카카오에 실제 등록된다. 등록 전(초안) 상태에서만 호출할 수 있다.
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkTemplateRegister(string templateId)
        {
            return await AlimtalkTemplateService.Register(this, templateId);
        }

        /// <summary>
        /// 알림톡 템플릿 검수 요청 (POST /v1/alimtalk/templates/{template_id}/inspect)
        /// ⚠️ 카카오에 검수를 요청하며 취소할 수 없다. 초안은 먼저 AlimtalkTemplateRegister() 를 부른다.
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkTemplateInspect(string templateId)
        {
            return await AlimtalkTemplateService.Inspect(this, templateId);
        }

        /// <summary>
        /// 알림톡 템플릿 목록 내보내기 (GET /v1/alimtalk/templates/export)
        /// ⚠️ SDK 기본 Format 은 json 이다(서버 기본 csv). 1회 5,000건을 넘으면 3031 로 거부된다.
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkTemplateExport(AlimtalkTemplateExportParams exportParams = null)
        {
            return await AlimtalkTemplateService.Export(this, exportParams);
        }

        /// <summary>
        /// 이미지형 알림톡 템플릿의 원본 이미지 업로드 (POST /v1/alimtalk/templates/image)
        /// jpg/png · 500KB 이하 · 가로 500px 이상 · 2:1.
        /// </summary>
        /// <param name="imagePath">이미지 파일 경로</param>
        /// <param name="replaceUrl">주면 업로드 성공 후에 기존 파일을 지운다</param>
        public async Task<HttpResponseMessage> AlimtalkTemplateImage(string imagePath, string replaceUrl = null)
        {
            return await AlimtalkTemplateService.Image(this, imagePath, replaceUrl);
        }

        /// <summary>
        /// 아이템리스트형 알림톡 템플릿의 하이라이트 썸네일 업로드 (POST /v1/alimtalk/templates/highlight_image)
        /// ⚠️ 본문 이미지와 규격이 다르다 — jpg/png · 500KB 이하 · 가로 108px 이상 · 1:1.
        /// </summary>
        /// <param name="imagePath">이미지 파일 경로</param>
        /// <param name="replaceUrl">주면 업로드 성공 후에 기존 파일을 지운다</param>
        public async Task<HttpResponseMessage> AlimtalkTemplateHighlightImage(string imagePath, string replaceUrl = null)
        {
            return await AlimtalkTemplateService.HighlightImage(this, imagePath, replaceUrl);
        }

        /// <summary>
        /// 카카오 카테고리 목록 조회 (GET /v1/alimtalk/categories)
        /// 발신프로필 등록 시 필요한 category_code 후보다.
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkSenderCategories()
        {
            return await AlimtalkSenderService.Categories(this);
        }

        /// <summary>
        /// 채널 관리자폰으로 OTP 발송 (POST /v1/alimtalk/senders/otp)
        /// ⚠️ 실제로 문자가 나간다.
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkSenderOtp(AlimtalkSenderOtpParams otpParams)
        {
            return await AlimtalkSenderService.Otp(this, otpParams);
        }

        /// <summary>
        /// 알림톡 발신프로필 등록 (POST /v1/alimtalk/senders)
        /// ⚠️ 카카오에 발신프로필이 실제 등록된다. 등록 성공 시 그룹키 등록까지 서버가 수행한다.
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkSenderCreate(AlimtalkSenderCreateParams createParams)
        {
            return await AlimtalkSenderService.Create(this, createParams);
        }

        /// <summary>
        /// 연동한 알림톡 채널 목록 조회 (GET /v1/alimtalk/senders)
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkSenderList()
        {
            return await AlimtalkSenderService.List(this);
        }

        /// <summary>
        /// 알림톡 채널 상세 조회 (GET /v1/alimtalk/senders/{ksp_id})
        /// ⚠️ 미연동/미존재 채널은 404, 다른 프로젝트의 채널은 403 으로 오며 둘 다 error_code 는 3024 다.
        /// </summary>
        /// <param name="kspId">채널 ID</param>
        /// <param name="sync">벤더 동기화 여부 (선택)</param>
        public async Task<HttpResponseMessage> AlimtalkSenderDetail(string kspId, bool? sync = null)
        {
            return await AlimtalkSenderService.Detail(this, kspId, sync);
        }

        /// <summary>
        /// 알림톡 채널 연동 해지 (DELETE /v1/alimtalk/senders/{ksp_id})
        /// 이 프로젝트와의 연동만 끊는다 — 채널 모델과 템플릿은 보존된다.
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkSenderRelease(string kspId)
        {
            return await AlimtalkSenderService.Release(this, kspId);
        }

        /// <summary>
        /// 알림톡 채널 변수 예문 사전 갱신 (PUT /v1/alimtalk/senders/{ksp_id}/variable_examples)
        /// ⚠️ 표시용 값이다 — 벤더로 전송되지 않으므로 검수 상태와 무관하다. 보낸 키만 덮어쓴다(부분 갱신).
        /// </summary>
        /// <param name="kspId">채널 ID</param>
        /// <param name="examples">{ "user_name": "홍길동" } — 키에 '.' 이나 선행 '$' 는 쓸 수 없다</param>
        public async Task<HttpResponseMessage> AlimtalkSenderVariableExamples(string kspId, Dictionary<string, string> examples)
        {
            return await AlimtalkSenderService.VariableExamples(this, kspId, examples);
        }

        /// <summary>
        /// 알림톡 수신거부 목록 조회 (GET /v1/alimtalk/optouts)
        /// phone 은 숫자만 남겨 부분일치로 찾는다(정확 매칭이 아니다).
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkOptoutList(AlimtalkOptoutListParams listParams = null)
        {
            return await AlimtalkOptoutService.List(this, listParams);
        }

        /// <summary>
        /// 알림톡 수신거부 등록 (POST /v1/alimtalk/optouts)
        /// 같은 번호를 다시 등록해도 멱등이다.
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkOptoutCreate(AlimtalkOptoutCreateParams createParams)
        {
            return await AlimtalkOptoutService.Create(this, createParams);
        }

        /// <summary>
        /// 발송 전 수신거부 사전 확인 (POST /v1/alimtalk/optouts/check)
        /// ⚠️ 1회 최대 1,000건이고 넘으면 -48 이다.
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkOptoutCheck(AlimtalkOptoutCheckParams checkParams)
        {
            return await AlimtalkOptoutService.Check(this, checkParams);
        }

        /// <summary>
        /// 알림톡 수신거부 해제 (DELETE /v1/alimtalk/optouts/{phone})
        /// ⚠️ 전역 차단은 해제되지 않고 global_blocked: true 로 알려 준다.
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkOptoutRelease(string phone)
        {
            return await AlimtalkOptoutService.Release(this, phone);
        }

        /// <summary>
        /// 알림톡 웹훅 설정 조회 (GET /v1/alimtalk/webhook)
        /// 시크릿은 앞 12자만 노출된다. 미설정이면 { configured: false } 로 온다.
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkWebhookDetail()
        {
            return await AlimtalkWebhookService.Detail(this);
        }

        /// <summary>
        /// 알림톡 웹훅 설정 저장 (PUT /v1/alimtalk/webhook)
        /// ⚠️ 주문·구독 통합 웹훅(WebhookSendTest)과 완전히 별개 경로다. url 은 https 만 허용한다(아니면 3028).
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkWebhookUpdate(AlimtalkWebhookUpdateParams updateParams = null)
        {
            return await AlimtalkWebhookService.Update(this, updateParams);
        }

        /// <summary>
        /// 알림톡 웹훅 테스트 이벤트 1건 발송 (POST /v1/alimtalk/webhook/test)
        /// ⚠️ 설정된 URL 로 실제 HTTP 요청이 나간다. 미설정이면 3029.
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkWebhookTest()
        {
            return await AlimtalkWebhookService.Test(this);
        }

        /// <summary>
        /// 알림톡 웹훅 서명 시크릿 재발급 (POST /v1/alimtalk/webhook/secret)
        /// ⚠️ 이 응답에서만 secret 원문을 돌려준다(이후 조회는 마스킹된다).
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkWebhookRotateSecret()
        {
            return await AlimtalkWebhookService.RotateSecret(this);
        }

        /// <summary>
        /// 알림톡 웹훅 전송 이력 조회 (GET /v1/alimtalk/webhook/deliveries)
        /// 성공·실패를 모두 남긴다.
        /// </summary>
        public async Task<HttpResponseMessage> AlimtalkWebhookDeliveries(AlimtalkWebhookDeliveriesParams deliveriesParams = null)
        {
            return await AlimtalkWebhookService.Deliveries(this, deliveriesParams);
        }

        #endregion
    }
}
