using System;
using System.Collections.Generic;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// Commerce API 요청별 헤더 빌더
    /// Idempotency-Key 는 미지정시 매 호출마다 자동 생성된다.
    /// 요청별 BOOTPAY-ROLE 은 기본 role 을 덮어쓰지 않고 해당 요청에만 적용된다.
    /// </summary>
    internal static class CommerceRequestHeaders
    {
        internal static Dictionary<string, string> Idempotency(string idempotencyKey = null)
        {
            return new Dictionary<string, string>
            {
                { "Idempotency-Key", string.IsNullOrEmpty(idempotencyKey) ? Guid.NewGuid().ToString() : idempotencyKey }
            };
        }

        internal static Dictionary<string, string> WithRole(string role, string idempotencyKey = null)
        {
            var headers = Idempotency(idempotencyKey);
            headers["BOOTPAY-ROLE"] = role;
            return headers;
        }

        internal static Dictionary<string, string> User(string idempotencyKey = null)
        {
            return WithRole("user", idempotencyKey);
        }

        internal static Dictionary<string, string> Manager(string idempotencyKey = null)
        {
            return WithRole("manager", idempotencyKey);
        }

        internal static Dictionary<string, string> Supervisor(string idempotencyKey = null)
        {
            return WithRole("supervisor", idempotencyKey);
        }

        /// <summary>
        /// V1 Mall API 요청 헤더 — Bootpay-User-JWT 는 값이 있을 때만 부착된다.
        /// </summary>
        internal static Dictionary<string, string> Mall(string userJwt = null, string idempotencyKey = null)
        {
            var headers = Idempotency(idempotencyKey);
            if (!string.IsNullOrEmpty(userJwt))
            {
                headers["Bootpay-User-JWT"] = userJwt;
            }
            return headers;
        }
    }
}
