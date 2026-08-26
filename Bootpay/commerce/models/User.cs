using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bootpay.Commerce.Models
{
    /// <summary>
    /// 사용자 그룹 참조
    /// </summary>
    public class CommerceUserGroupRef
    {
        [JsonProperty("user_group_id")]
        public string UserGroupId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Commerce 사용자
    /// </summary>
    public class CommerceUser
    {
        [JsonProperty("user_id")]
        public string UserId { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }

        // 고객 유형
        [JsonProperty("membership_type")]
        public int? MembershipType { get; set; }

        // 고객 정보
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("tel")]
        public string Tel { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("bank_username")]
        public string BankUsername { get; set; }

        [JsonProperty("bank_account")]
        public string BankAccount { get; set; }

        [JsonProperty("bank_code")]
        public string BankCode { get; set; }

        [JsonProperty("comment")]
        public string Comment { get; set; }

        // 최종상태
        [JsonProperty("count")]
        public int? Count { get; set; }

        [JsonProperty("status")]
        public int? Status { get; set; }

        // 개인 고객
        [JsonProperty("gender")]
        public int? Gender { get; set; }

        [JsonProperty("birth")]
        public string Birth { get; set; }

        [JsonProperty("individual_extension")]
        public Dictionary<string, object> IndividualExtension { get; set; }

        // 쇼핑몰 회원
        [JsonProperty("login_id")]
        public string LoginId { get; set; }

        [JsonProperty("login_pw")]
        public string LoginPw { get; set; }

        [JsonProperty("login_type")]
        public int? LoginType { get; set; }

        [JsonProperty("group_tags")]
        public List<string> GroupTags { get; set; }

        [JsonProperty("metadata")]
        public Dictionary<string, object> Metadata { get; set; }

        // 인증정보
        [JsonProperty("auth_sms")]
        public bool? AuthSms { get; set; }

        [JsonProperty("auth_phone")]
        public bool? AuthPhone { get; set; }

        [JsonProperty("auth_email")]
        public bool? AuthEmail { get; set; }

        [JsonProperty("ci")]
        public string Ci { get; set; }

        [JsonProperty("cd")]
        public string Cd { get; set; }

        [JsonProperty("join_at")]
        public string JoinAt { get; set; }

        [JsonProperty("join_confirm_type")]
        public int? JoinConfirmType { get; set; }

        [JsonProperty("lasted_at")]
        public string LastedAt { get; set; }

        // 약관 동의
        [JsonProperty("marketing_accept_type")]
        public int? MarketingAcceptType { get; set; }

        [JsonProperty("marketing_accept_create_at")]
        public string MarketingAcceptCreateAt { get; set; }

        [JsonProperty("marketing_accept_update_at")]
        public string MarketingAcceptUpdateAt { get; set; }

        [JsonProperty("term_ids")]
        public List<string> TermIds { get; set; }

        [JsonProperty("group")]
        public CommerceUserGroupRef Group { get; set; }

        [JsonProperty("external_uid")]
        public string ExternalUid { get; set; }

        [JsonProperty("is_external")]
        public string IsExternal { get; set; }

        [JsonProperty("user_group_id")]
        public string UserGroupId { get; set; }
    }

    /// <summary>
    /// 사용자 목록 조회 파라미터
    /// </summary>
    public class UserListParams : ListParams
    {
        /// <summary>
        /// 회원등급 필터. 서버(<c>v1/users_controller#index</c>)가 읽는 정식 키는 <c>membership_type</c> 이다.
        /// </summary>
        [JsonProperty("membership_type")]
        public int? MembershipType { get; set; }

        /// <summary>
        /// <c>MembershipType</c> 의 구 이름. 서버가 읽지 않는 <c>member_type</c> 으로 나가고 있었다.
        /// 값을 채우면 <c>membership_type</c> 으로 대신 전송된다 (하위호환 별칭).
        /// </summary>
        [JsonProperty("member_type")]
        [Obsolete("서버가 읽지 않는 이름이었다. MembershipType 을 쓸 것 — 값을 채우면 membership_type 으로 전송된다.")]
        public int? MemberType { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }

    /// <summary>
    /// 사용자 토큰 응답
    /// </summary>
    public class UserTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expired_at")]
        public string ExpiredAt { get; set; }

        [JsonProperty("user")]
        public CommerceUser User { get; set; }
    }

    /// <summary>
    /// 사용자 로그인 응답
    /// </summary>
    public class UserLoginResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expired_at")]
        public string ExpiredAt { get; set; }

        [JsonProperty("user")]
        public CommerceUser User { get; set; }
    }

    /// <summary>
    /// 중복 체크 응답
    /// </summary>
    public class ExistsResponse
    {
        [JsonProperty("exists")]
        public bool Exists { get; set; }
    }

    /// <summary>
    /// 회원 로그인 파라미터 (V1 Mall API — POST /v1/users/login)
    /// </summary>
    public class MallUserLoginParams
    {
        [JsonProperty("login_id")]
        public string LoginId { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// 0: 개인, 1: 사업자 (미지정시 0 으로 전송된다)
        /// </summary>
        [JsonProperty("corporate_type")]
        public int? CorporateType { get; set; }
    }

    /// <summary>
    /// 회원가입 파라미터 (V1 Mall API — POST /v1/users/join)
    /// null 값은 전송하지 않는다.
    /// </summary>
    public class MallUserJoinParams
    {
        [JsonProperty("login_id")]
        public string LoginId { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("gender")]
        public int? Gender { get; set; }

        [JsonProperty("birth")]
        public string Birth { get; set; }

        /// <summary>
        /// 0: 개인, 1: 사업자 (미지정시 0 으로 전송된다)
        /// </summary>
        [JsonProperty("corporate_type")]
        public int? CorporateType { get; set; }

        [JsonProperty("group")]
        public Dictionary<string, object> Group { get; set; }
    }

    /// <summary>
    /// 회원 세션 조회 응답 (V1 Mall API — GET /v1/users/session)
    /// </summary>
    public class MallUserSessionResponse
    {
        [JsonProperty("user")]
        public CommerceUser User { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expired_at")]
        public string ExpiredAt { get; set; }
    }
}
