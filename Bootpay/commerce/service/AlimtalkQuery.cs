using System.Collections.Specialized;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 알림톡 조회 계열이 공유하는 query string 헬퍼.
    /// 값이 없으면 "?" 자체를 붙이지 않는다 — 빈 "?" 는 다른 SDK 가 보내는 경로와 달라진다.
    /// </summary>
    internal static class AlimtalkQuery
    {
        internal static string Suffix(NameValueCollection queryParams)
        {
            var query = queryParams == null ? "" : queryParams.ToString();
            return string.IsNullOrEmpty(query) ? "" : $"?{query}";
        }
    }
}
