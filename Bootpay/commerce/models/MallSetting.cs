using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bootpay.Commerce.Models
{
    /// <summary>
    /// 몰 설정 수정 파라미터 (PUT /v1/mall-setting)
    /// 요청 바디는 flatten 형식이며, null 값은 전송되지 않는다.
    /// supervisor scope 전용 endpoint.
    /// </summary>
    public class MallSettingUpdateParams
    {

        // 위젯
        [JsonProperty("normal_widget_key")]
        public string NormalWidgetKey { get; set; }

        [JsonProperty("subscription_widget_key")]
        public string SubscriptionWidgetKey { get; set; }

        // 사업자 정보
        [JsonProperty("seller_name")]
        public string SellerName { get; set; }

        [JsonProperty("seller_name_en")]
        public string SellerNameEn { get; set; }

        [JsonProperty("biz_email")]
        public string BizEmail { get; set; }

        [JsonProperty("biz_tel")]
        public string BizTel { get; set; }

        [JsonProperty("biz_fax")]
        public string BizFax { get; set; }

        [JsonProperty("registration_no")]
        public string RegistrationNo { get; set; }

        [JsonProperty("corp_reg_no")]
        public string CorpRegNo { get; set; }

        [JsonProperty("mail_order_sales_number")]
        public string MailOrderSalesNumber { get; set; }

        [JsonProperty("owner_name")]
        public string OwnerName { get; set; }

        [JsonProperty("zip")]
        public string Zip { get; set; }

        [JsonProperty("addr_1")]
        public string Addr1 { get; set; }

        [JsonProperty("addr_2")]
        public string Addr2 { get; set; }

        [JsonProperty("privacy_name")]
        public string PrivacyName { get; set; }

        [JsonProperty("privacy_email")]
        public string PrivacyEmail { get; set; }

        // 몰 기본 정보
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

        [JsonProperty("invoice_title")]
        public string InvoiceTitle { get; set; }

        // 브랜딩
        [JsonProperty("use_logo")]
        public bool? UseLogo { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("use_favicon")]
        public bool? UseFavicon { get; set; }

        [JsonProperty("favicon")]
        public string Favicon { get; set; }

        [JsonProperty("use_open_graph")]
        public bool? UseOpenGraph { get; set; }

        [JsonProperty("og_image")]
        public string OgImage { get; set; }

        [JsonProperty("use_signature")]
        public bool? UseSignature { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; }

        // 고객센터 운영시간
        [JsonProperty("use_operation_time")]
        public bool? UseOperationTime { get; set; }

        [JsonProperty("customer_service_center_operation_time")]
        public string CustomerServiceCenterOperationTime { get; set; }

        [JsonProperty("rest_start_hour")]
        public int? RestStartHour { get; set; }

        [JsonProperty("rest_start_minute")]
        public int? RestStartMinute { get; set; }

        [JsonProperty("rest_end_hour")]
        public int? RestEndHour { get; set; }

        [JsonProperty("rest_end_minute")]
        public int? RestEndMinute { get; set; }

        /** 휴무일 (요일 코드 배열 또는 서버 정의 문자열) */
        [JsonProperty("rest_day")]
        public object RestDay { get; set; }

        [JsonProperty("hosting_service")]
        public string HostingService { get; set; }

        // 주문/연령 정책
        [JsonProperty("use_non_member_order")]
        public bool? UseNonMemberOrder { get; set; }

        [JsonProperty("use_age_accept_19")]
        public bool? UseAgeAccept19 { get; set; }

        [JsonProperty("use_age_accept_14")]
        public bool? UseAgeAccept14 { get; set; }

        [JsonProperty("use_age_accept_parent_name")]
        public bool? UseAgeAcceptParentName { get; set; }

        [JsonProperty("use_age_accept_parent_birth")]
        public bool? UseAgeAcceptParentBirth { get; set; }

        [JsonProperty("use_age_accept_parent_email")]
        public bool? UseAgeAcceptParentEmail { get; set; }

        // 회원가입 수집 항목
        [JsonProperty("use_membership_collect_phone")]
        public bool? UseMembershipCollectPhone { get; set; }

        [JsonProperty("use_membership_collect_tel")]
        public bool? UseMembershipCollectTel { get; set; }

        [JsonProperty("use_membership_collect_email")]
        public bool? UseMembershipCollectEmail { get; set; }

        [JsonProperty("use_membership_collect_address")]
        public bool? UseMembershipCollectAddress { get; set; }

        [JsonProperty("use_membership_collect_bank")]
        public bool? UseMembershipCollectBank { get; set; }

        [JsonProperty("use_membership_collect_birth")]
        public bool? UseMembershipCollectBirth { get; set; }

        [JsonProperty("use_membership_collect_gender")]
        public bool? UseMembershipCollectGender { get; set; }

        [JsonProperty("use_membership_collect_interest")]
        public bool? UseMembershipCollectInterest { get; set; }

        [JsonProperty("membership_collect_interest_number")]
        public int? MembershipCollectInterestNumber { get; set; }

        [JsonProperty("use_membership_collect_customs")]
        public bool? UseMembershipCollectCustoms { get; set; }

        [JsonProperty("use_membership_collect_nickname")]
        public bool? UseMembershipCollectNickname { get; set; }

        [JsonProperty("use_membership_collect_recommend_id")]
        public bool? UseMembershipCollectRecommendId { get; set; }

        [JsonProperty("recommend_id_point_to")]
        public int? RecommendIdPointTo { get; set; }

        [JsonProperty("recommend_id_point_from")]
        public int? RecommendIdPointFrom { get; set; }

        [JsonProperty("use_membership_collect_business")]
        public bool? UseMembershipCollectBusiness { get; set; }

        [JsonProperty("use_membership_collect_register")]
        public bool? UseMembershipCollectRegister { get; set; }

        [JsonProperty("membership_only_business")]
        public bool? MembershipOnlyBusiness { get; set; }

        // 기업(그룹) 회원
        [JsonProperty("use_corporate_department")]
        public bool? UseCorporateDepartment { get; set; }

        [JsonProperty("sub_group_type")]
        public int? SubGroupType { get; set; }

        [JsonProperty("use_corporate_signup_approval")]
        public bool? UseCorporateSignupApproval { get; set; }

        /** 기업 회원 허용 이메일 도메인 목록 */
        [JsonProperty("corporate_email_domains")]
        public object CorporateEmailDomains { get; set; }

        [JsonProperty("use_corporate_auto_approve")]
        public bool? UseCorporateAutoApprove { get; set; }

        [JsonProperty("use_corporate_invite_only")]
        public bool? UseCorporateInviteOnly { get; set; }

        // 회원 정보 노출 항목
        [JsonProperty("use_member_info_phone")]
        public bool? UseMemberInfoPhone { get; set; }

        [JsonProperty("use_member_info_tel")]
        public bool? UseMemberInfoTel { get; set; }

        [JsonProperty("use_member_info_email")]
        public bool? UseMemberInfoEmail { get; set; }

        [JsonProperty("use_member_info_address")]
        public bool? UseMemberInfoAddress { get; set; }

        [JsonProperty("use_member_info_bank")]
        public bool? UseMemberInfoBank { get; set; }

        [JsonProperty("use_member_info_birth")]
        public bool? UseMemberInfoBirth { get; set; }

        [JsonProperty("use_member_info_gender")]
        public bool? UseMemberInfoGender { get; set; }

        [JsonProperty("use_member_info_customs")]
        public bool? UseMemberInfoCustoms { get; set; }

        [JsonProperty("use_member_info_nickname")]
        public bool? UseMemberInfoNickname { get; set; }

        [JsonProperty("use_member_info_register")]
        public bool? UseMemberInfoRegister { get; set; }

        // 주문자 수집 항목
        [JsonProperty("orderer_collect_phone")]
        public bool? OrdererCollectPhone { get; set; }

        [JsonProperty("orderer_collect_tel")]
        public bool? OrdererCollectTel { get; set; }

        [JsonProperty("orderer_collect_email")]
        public bool? OrdererCollectEmail { get; set; }

        // 주문/취소 정책
        [JsonProperty("order_prefix")]
        public string OrderPrefix { get; set; }

        [JsonProperty("use_order_cancel")]
        public bool? UseOrderCancel { get; set; }

        /** 취소 승인 사용 여부 (서버 필드명 오타 그대로 유지) */
        [JsonProperty("use_oder_cancel_approval")]
        public bool? UseOderCancelApproval { get; set; }

        /** 취소 사유 목록 */
        [JsonProperty("order_cancel_reasons")]
        public object OrderCancelReasons { get; set; }

        [JsonProperty("order_cancel_reason_required_type")]
        public int? OrderCancelReasonRequiredType { get; set; }

        [JsonProperty("order_cancel_request_message")]
        public string OrderCancelRequestMessage { get; set; }

        [JsonProperty("order_cancel_done_message")]
        public string OrderCancelDoneMessage { get; set; }

        // 회원 가입/인증 방식
        [JsonProperty("use_general_membership")]
        public bool? UseGeneralMembership { get; set; }

        [JsonProperty("general_membership_duplication")]
        public int? GeneralMembershipDuplication { get; set; }

        [JsonProperty("use_certification")]
        public bool? UseCertification { get; set; }

        [JsonProperty("certification_type")]
        public int? CertificationType { get; set; }

        [JsonProperty("general_membership_id_type")]
        public int? GeneralMembershipIdType { get; set; }

        [JsonProperty("use_membership_duplication_email")]
        public bool? UseMembershipDuplicationEmail { get; set; }

        [JsonProperty("use_membership_duplication_phone")]
        public bool? UseMembershipDuplicationPhone { get; set; }

        [JsonProperty("use_social_membership")]
        public bool? UseSocialMembership { get; set; }

        /** 사용 소셜 로그인 타입 */
        [JsonProperty("social_membership_type")]
        public object SocialMembershipType { get; set; }

        // 적립금
        [JsonProperty("use_point")]
        public bool? UsePoint { get; set; }

        [JsonProperty("use_point_transaction")]
        public bool? UsePointTransaction { get; set; }

        [JsonProperty("point_display_name")]
        public string PointDisplayName { get; set; }

        [JsonProperty("point_min_balance")]
        public int? PointMinBalance { get; set; }

        /** 적립 제외 조건 */
        [JsonProperty("point_not_condition")]
        public object PointNotCondition { get; set; }

        /** 적립 조건 */
        [JsonProperty("point_condition")]
        public object PointCondition { get; set; }

        [JsonProperty("use_point_max_rate")]
        public bool? UsePointMaxRate { get; set; }

        [JsonProperty("point_max_rate")]
        public double? PointMaxRate { get; set; }

        [JsonProperty("use_point_max_amount")]
        public bool? UsePointMaxAmount { get; set; }

        [JsonProperty("point_max_amount")]
        public int? PointMaxAmount { get; set; }

        [JsonProperty("point_rate")]
        public double? PointRate { get; set; }

        [JsonProperty("point_calc_type1")]
        public int? PointCalcType1 { get; set; }

        [JsonProperty("point_calc_type2")]
        public int? PointCalcType2 { get; set; }

        [JsonProperty("use_point_advance_discount")]
        public bool? UsePointAdvanceDiscount { get; set; }

        [JsonProperty("point_advance_discount_rate")]
        public double? PointAdvanceDiscountRate { get; set; }

        [JsonProperty("use_point_expire")]
        public bool? UsePointExpire { get; set; }

        [JsonProperty("point_expire_type")]
        public int? PointExpireType { get; set; }

        [JsonProperty("point_issue_event_type")]
        public int? PointIssueEventType { get; set; }

        [JsonProperty("point_issue_delay_days")]
        public int? PointIssueDelayDays { get; set; }

        // 오픈마켓 / 상품
        [JsonProperty("use_open_market")]
        public bool? UseOpenMarket { get; set; }

        [JsonProperty("use_product_approval")]
        public bool? UseProductApproval { get; set; }

        [JsonProperty("use_product_review")]
        public bool? UseProductReview { get; set; }

        [JsonProperty("use_product_review_point")]
        public bool? UseProductReviewPoint { get; set; }

        [JsonProperty("product_review_point")]
        public int? ProductReviewPoint { get; set; }

        [JsonProperty("product_review_photo_point")]
        public int? ProductReviewPhotoPoint { get; set; }

        [JsonProperty("use_product_review_answer")]
        public bool? UseProductReviewAnswer { get; set; }

        [JsonProperty("use_product_review_auto_answer")]
        public bool? UseProductReviewAutoAnswer { get; set; }

        [JsonProperty("product_review_auto_answer_minute")]
        public int? ProductReviewAutoAnswerMinute { get; set; }

        [JsonProperty("product_review_auto_answer_text")]
        public string ProductReviewAutoAnswerText { get; set; }

        [JsonProperty("use_product_qna")]
        public bool? UseProductQna { get; set; }

        [JsonProperty("product_qna_member_auth")]
        public int? ProductQnaMemberAuth { get; set; }

        [JsonProperty("use_product_qna_answer_option")]
        public bool? UseProductQnaAnswerOption { get; set; }

        // 게시판 / 상담
        [JsonProperty("use_notice")]
        public bool? UseNotice { get; set; }

        [JsonProperty("use_qna")]
        public bool? UseQna { get; set; }

        [JsonProperty("use_faq")]
        public bool? UseFaq { get; set; }

        [JsonProperty("use_chat_support")]
        public bool? UseChatSupport { get; set; }

        [JsonProperty("chat_support_type")]
        public int? ChatSupportType { get; set; }

        [JsonProperty("chat_support_key")]
        public string ChatSupportKey { get; set; }

        // 휴면 / 탈퇴
        [JsonProperty("use_dormant")]
        public bool? UseDormant { get; set; }

        [JsonProperty("dormant_year")]
        public int? DormantYear { get; set; }

        [JsonProperty("dormant_restore")]
        public int? DormantRestore { get; set; }

        [JsonProperty("use_withdrawal")]
        public bool? UseWithdrawal { get; set; }

        [JsonProperty("use_withdrawal_guide_message")]
        public bool? UseWithdrawalGuideMessage { get; set; }

        [JsonProperty("use_withdrawal_guide_message_after")]
        public bool? UseWithdrawalGuideMessageAfter { get; set; }

        [JsonProperty("withdrawal_guide_message_after")]
        public string WithdrawalGuideMessageAfter { get; set; }

        [JsonProperty("use_withdrawal_auto")]
        public bool? UseWithdrawalAuto { get; set; }

        [JsonProperty("withdrawal_auto_year")]
        public int? WithdrawalAutoYear { get; set; }

        // 정기구독 정산
        [JsonProperty("use_subscription_aggregate_transaction")]
        public bool? UseSubscriptionAggregateTransaction { get; set; }

        [JsonProperty("subscription_month_day")]
        public int? SubscriptionMonthDay { get; set; }

        [JsonProperty("subscription_week_day")]
        public int? SubscriptionWeekDay { get; set; }

        // 구매 한도
        [JsonProperty("use_limit")]
        public bool? UseLimit { get; set; }

        [JsonProperty("limit_month_purchase")]
        public int? LimitMonthPurchase { get; set; }

        [JsonProperty("limit_week_purchase")]
        public int? LimitWeekPurchase { get; set; }

        [JsonProperty("use_limit_payment")]
        public bool? UseLimitPayment { get; set; }

        [JsonProperty("use_limit_message")]
        public bool? UseLimitMessage { get; set; }

        // 약관
        [JsonProperty("terms_of_service")]
        public string TermsOfService { get; set; }

        [JsonProperty("terms_of_privacy_policy")]
        public string TermsOfPrivacyPolicy { get; set; }

        [JsonProperty("terms_of_privacy_collect")]
        public string TermsOfPrivacyCollect { get; set; }

        [JsonProperty("terms_of_privacy_third")]
        public string TermsOfPrivacyThird { get; set; }

        // 결제 / 노출
        [JsonProperty("payment_timeout")]
        public int? PaymentTimeout { get; set; }

        [JsonProperty("product_sort_type")]
        public int? ProductSortType { get; set; }

        [JsonProperty("mall_theme_type")]
        public int? MallThemeType { get; set; }

        [JsonProperty("catalog_display_type")]
        public int? CatalogDisplayType { get; set; }

        [JsonProperty("catalog_headline")]
        public string CatalogHeadline { get; set; }

        [JsonProperty("catalog_bg_color")]
        public string CatalogBgColor { get; set; }

        [JsonProperty("catalog_view_type_pc")]
        public int? CatalogViewTypePc { get; set; }

        [JsonProperty("catalog_view_type_mobile")]
        public int? CatalogViewTypeMobile { get; set; }

        [JsonProperty("catalog_product_sort_type")]
        public int? CatalogProductSortType { get; set; }

        // 장바구니 / 위시리스트
        [JsonProperty("use_cart")]
        public bool? UseCart { get; set; }

        [JsonProperty("cart_storage_period")]
        public int? CartStoragePeriod { get; set; }

        [JsonProperty("cart_max_limit")]
        public int? CartMaxLimit { get; set; }

        [JsonProperty("cart_add_action")]
        public int? CartAddAction { get; set; }

        [JsonProperty("cart_direct_purchase")]
        public bool? CartDirectPurchase { get; set; }

        [JsonProperty("cart_option_change")]
        public bool? CartOptionChange { get; set; }

        [JsonProperty("cart_discount_display")]
        public bool? CartDiscountDisplay { get; set; }

        [JsonProperty("use_wishlist")]
        public bool? UseWishlist { get; set; }

        [JsonProperty("wishlist_max_limit")]
        public int? WishlistMaxLimit { get; set; }

        [JsonProperty("cart_wishlist_display")]
        public bool? CartWishlistDisplay { get; set; }
    }

    /// <summary>
    /// 몰 설정 (GET /v1/mall-setting 응답)
    /// </summary>
    public class CommerceMallSetting : MallSettingUpdateParams
    {
        [JsonProperty("mall_setting_id")]
        public string MallSettingId { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("seller_id")]
        public string SellerId { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        [JsonExtensionData]
        public IDictionary<string, JToken> ExtensionData { get; set; }
    }
}
