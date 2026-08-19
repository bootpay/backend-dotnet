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
        public async Task<HttpResponseMessage> UserGroupUserCreate(string userGroupId, string userId)
        {
            return await UserGroupService.UserCreate(this, userGroupId, userId);
        }

        /// <summary>
        /// 그룹에서 사용자 제거
        /// </summary>
        public async Task<HttpResponseMessage> UserGroupUserDelete(string userGroupId, string userId)
        {
            return await UserGroupService.UserDelete(this, userGroupId, userId);
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
        /// </summary>
        public async Task<HttpResponseMessage> ProductDetail(string productId)
        {
            return await ProductService.Detail(this, productId);
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

        public async Task<HttpResponseMessage> OrderSubscriptionSupervisorApprove(string orderSubscriptionId, SupervisorOrderSubscriptionApproveParams approveParams = null)
        {
            return await OrderSubscriptionService.SupervisorApprove(this, orderSubscriptionId, approveParams);
        }

        public async Task<HttpResponseMessage> OrderSubscriptionSupervisorReject(string orderSubscriptionId, SupervisorOrderSubscriptionRejectParams rejectParams = null)
        {
            return await OrderSubscriptionService.SupervisorReject(this, orderSubscriptionId, rejectParams);
        }

        public async Task<HttpResponseMessage> OrderSubscriptionSupervisorTerminate(string orderSubscriptionId, SupervisorOrderSubscriptionTerminateParams terminateParams = null)
        {
            return await OrderSubscriptionService.SupervisorTerminate(this, orderSubscriptionId, terminateParams);
        }

        public async Task<HttpResponseMessage> OrderSubscriptionSupervisorPause(string orderSubscriptionId, SupervisorOrderSubscriptionPauseParams pauseParams)
        {
            return await OrderSubscriptionService.SupervisorPause(this, orderSubscriptionId, pauseParams);
        }

        public async Task<HttpResponseMessage> OrderSubscriptionSupervisorResume(string orderSubscriptionId, SupervisorOrderSubscriptionResumeParams resumeParams = null)
        {
            return await OrderSubscriptionService.SupervisorResume(this, orderSubscriptionId, resumeParams);
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
        public async Task<HttpResponseMessage> CategoryCreate(CategoryCreateParams createParams)
        {
            return await CategoryService.Create(this, createParams);
        }

        /// <summary>
        /// 카테고리 수정
        /// </summary>
        public async Task<HttpResponseMessage> CategoryUpdate(CategoryUpdateParams updateParams)
        {
            return await CategoryService.Update(this, updateParams);
        }

        /// <summary>
        /// 카테고리 삭제
        /// </summary>
        public async Task<HttpResponseMessage> CategoryDestroy(string categoryId)
        {
            return await CategoryService.Destroy(this, categoryId);
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
    }
}
