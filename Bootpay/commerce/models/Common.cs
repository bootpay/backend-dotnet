using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bootpay.Commerce.Models
{
    /// <summary>
    /// 공통 목록 조회 파라미터
    /// </summary>
    public class ListParams
    {
        [JsonProperty("page")]
        public int? Page { get; set; }

        [JsonProperty("limit")]
        public int? Limit { get; set; }

        [JsonProperty("keyword")]
        public string Keyword { get; set; }
    }

    /// <summary>
    /// 목록 응답
    /// </summary>
    public class ListResponse<T>
    {
        [JsonProperty("items")]
        public List<T> Items { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }

    /// <summary>
    /// Commerce API 응답
    /// </summary>
    public class CommerceResponse<T>
    {
        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("error_code")]
        public int? ErrorCode { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    /// <summary>
    /// 주소 정보
    /// </summary>
    public class CommerceAddress
    {
        [JsonProperty("address_id")]
        public string AddressId { get; set; }

        [JsonProperty("zipcode")]
        public string Zipcode { get; set; }

        [JsonProperty("addr1")]
        public string Addr1 { get; set; }

        [JsonProperty("addr2")]
        public string Addr2 { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("memo")]
        public string Memo { get; set; }

        [JsonProperty("is_default")]
        public bool? IsDefault { get; set; }
    }

    /// <summary>
    /// 주소 안내 정보
    /// </summary>
    public class CommerceAddressInstruction
    {
        [JsonProperty("instruction_type")]
        public int? InstructionType { get; set; }

        [JsonProperty("instruction")]
        public string Instruction { get; set; }
    }

    /// <summary>
    /// 토큰 응답
    /// </summary>
    public class CommerceTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("expired_at")]
        public string ExpiredAt { get; set; }
    }
}
