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
        /// 그룹 제한 설정
        /// </summary>
        public async Task<HttpResponseMessage> UserGroupLimit(UserGroupLimitParams limitParams)
        {
            return await UserGroupService.Limit(this, limitParams);
        }

        /// <summary>
        /// 그룹 거래 집계 설정
        /// </summary>
        public async Task<HttpResponseMessage> UserGroupAggregateTransaction(UserGroupAggregateTransactionParams aggregateParams)
        {
            return await UserGroupService.AggregateTransaction(this, aggregateParams);
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
        /// 상품 생성
        /// </summary>
        public async Task<HttpResponseMessage> ProductCreate(CommerceProduct product)
        {
            return await ProductService.Create(this, product);
        }

        /// <summary>
        /// 상품 생성 (이미지 파일 포함)
        /// </summary>
        /// <param name="product">상품 정보</param>
        /// <param name="imagePaths">이미지 파일 경로 리스트</param>
        public async Task<HttpResponseMessage> ProductCreateWithImages(CommerceProduct product, List<string> imagePaths)
        {
            return await ProductService.CreateWithImages(this, product, imagePaths);
        }

        /// <summary>
        /// 상품 상세 조회
        /// </summary>
        public async Task<HttpResponseMessage> ProductDetail(string productId)
        {
            return await ProductService.Detail(this, productId);
        }

        /// <summary>
        /// 상품 수정
        /// </summary>
        public async Task<HttpResponseMessage> ProductUpdate(CommerceProduct product)
        {
            return await ProductService.Update(this, product);
        }

        /// <summary>
        /// 상품 상태 변경
        /// </summary>
        public async Task<HttpResponseMessage> ProductStatus(ProductStatusParams statusParams)
        {
            return await ProductService.Status(this, statusParams);
        }

        /// <summary>
        /// 상품 삭제
        /// </summary>
        public async Task<HttpResponseMessage> ProductDelete(string productId)
        {
            return await ProductService.Delete(this, productId);
        }

        #endregion

        #region Invoice (청구서)

        /// <summary>
        /// 청구서 목록 조회
        /// </summary>
        public async Task<HttpResponseMessage> InvoiceList(ListParams listParams = null)
        {
            return await InvoiceService.List(this, listParams);
        }

        /// <summary>
        /// 청구서 생성
        /// </summary>
        public async Task<HttpResponseMessage> InvoiceCreate(CommerceInvoice invoice)
        {
            return await InvoiceService.Create(this, invoice);
        }

        /// <summary>
        /// 청구서 알림 발송
        /// </summary>
        public async Task<HttpResponseMessage> InvoiceNotify(string invoiceId, List<int> sendTypes)
        {
            return await InvoiceService.Notify(this, invoiceId, sendTypes);
        }

        /// <summary>
        /// 청구서 상세 조회
        /// </summary>
        public async Task<HttpResponseMessage> InvoiceDetail(string invoiceId)
        {
            return await InvoiceService.Detail(this, invoiceId);
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
        /// 취소 요청 목록 조회
        /// </summary>
        public async Task<HttpResponseMessage> OrderCancelList(OrderCancelListParams listParams = null)
        {
            return await OrderCancelService.List(this, listParams);
        }

        /// <summary>
        /// 취소 요청
        /// </summary>
        public async Task<HttpResponseMessage> OrderCancelRequest(OrderCancelParams cancelParams)
        {
            return await OrderCancelService.Request(this, cancelParams);
        }

        /// <summary>
        /// 취소 요청 철회
        /// </summary>
        public async Task<HttpResponseMessage> OrderCancelWithdraw(string orderCancelRequestHistoryId)
        {
            return await OrderCancelService.Withdraw(this, orderCancelRequestHistoryId);
        }

        /// <summary>
        /// 취소 승인
        /// </summary>
        public async Task<HttpResponseMessage> OrderCancelApprove(OrderCancelActionParams actionParams)
        {
            return await OrderCancelService.Approve(this, actionParams);
        }

        /// <summary>
        /// 취소 거절
        /// </summary>
        public async Task<HttpResponseMessage> OrderCancelReject(OrderCancelActionParams actionParams)
        {
            return await OrderCancelService.Reject(this, actionParams);
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
        /// 정기구독 수정
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionUpdate(OrderSubscriptionUpdateParams updateParams)
        {
            return await OrderSubscriptionService.Update(this, updateParams);
        }

        /// <summary>
        /// 정기구독 일시정지
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionPause(OrderSubscriptionPauseParams pauseParams)
        {
            return await OrderSubscriptionService.Pause(this, pauseParams);
        }

        /// <summary>
        /// 정기구독 재개
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionResume(OrderSubscriptionResumeParams resumeParams)
        {
            return await OrderSubscriptionService.Resume(this, resumeParams);
        }

        /// <summary>
        /// 해지 수수료 계산
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionCalculateTerminationFee(string orderSubscriptionId = null, string orderNumber = null)
        {
            return await OrderSubscriptionService.CalculateTerminationFee(this, orderSubscriptionId, orderNumber);
        }

        /// <summary>
        /// 주문번호로 해지 수수료 계산
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionCalculateTerminationFeeByOrderNumber(string orderNumber)
        {
            return await OrderSubscriptionService.CalculateTerminationFeeByOrderNumber(this, orderNumber);
        }

        /// <summary>
        /// 정기구독 해지
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionTermination(OrderSubscriptionTerminationParams terminationParams)
        {
            return await OrderSubscriptionService.Termination(this, terminationParams);
        }

        #endregion

        #region OrderSubscriptionBill (정기구독 청구)

        /// <summary>
        /// 정기구독 청구 목록 조회
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionBillList(OrderSubscriptionBillListParams listParams = null)
        {
            return await OrderSubscriptionBillService.List(this, listParams);
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

        #region OrderSubscriptionAdjustment (정기구독 조정)

        /// <summary>
        /// 정기구독 조정 생성
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionAdjustmentCreate(string orderSubscriptionId, CommerceOrderSubscriptionAdjustment adjustment)
        {
            return await OrderSubscriptionAdjustmentService.Create(this, orderSubscriptionId, adjustment);
        }

        /// <summary>
        /// 정기구독 조정 수정
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionAdjustmentUpdate(OrderSubscriptionAdjustmentUpdateParams updateParams)
        {
            return await OrderSubscriptionAdjustmentService.Update(this, updateParams);
        }

        /// <summary>
        /// 정기구독 조정 삭제
        /// </summary>
        public async Task<HttpResponseMessage> OrderSubscriptionAdjustmentDelete(string orderSubscriptionId, string orderSubscriptionAdjustmentId)
        {
            return await OrderSubscriptionAdjustmentService.Delete(this, orderSubscriptionId, orderSubscriptionAdjustmentId);
        }

        #endregion
    }
}
