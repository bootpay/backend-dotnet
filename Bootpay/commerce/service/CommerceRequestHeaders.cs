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
        /// BOOTPAY-ROLE 만 싣는다 (Idempotency-Key 없음).
        /// role 고정 전에 헤더가 없던 엔드포인트용 — role 을 고정하면서 멱등 키까지 새로 붙이지 않도록 nodejs · go 와 맞춘다.
        /// </summary>
        internal static Dictionary<string, string> RoleOnly(string role)
        {
            return new Dictionary<string, string> { { "BOOTPAY-ROLE", role } };
        }

        /// <summary>
        /// user 또는 supervisor 를 받는 엔드포인트용 BOOTPAY-ROLE (Idempotency-Key 없음).
        /// 인스턴스 role 이 supervisor 면 supervisor, 그 밖(manager 포함)은 user 로 고정한다.
        /// </summary>
        internal static Dictionary<string, string> UserOrSupervisorRoleOnly(BootpayCommerceObject bootpay)
        {
            return RoleOnly(IsSupervisor(bootpay) ? "supervisor" : "user");
        }

        internal static bool IsSupervisor(BootpayCommerceObject bootpay)
        {
            return bootpay != null && bootpay.GetCurrentRole() == "supervisor";
        }

        /// <summary>
        /// 알림톡 요청 헤더.
        /// ★Idempotency-Key 를 싣지 않는다★ 알림톡 API 는 이 헤더를 읽지 않는다 —
        ///   멱등은 발송의 ref_id 로만 성립하므로, 헤더를 붙이면 서버가 주지 않는 보장을 주는 것처럼 보인다.
        /// ★BOOTPAY-ROLE 은 항상 user★ 알림톡 스코프 키가 전부 user:alimtalk_* 다.
        ///   인스턴스 role 이 manager/supervisor 로 바뀌어 있어도 여기서 고정한다.
        /// </summary>
        /// <param name="accept">CSV 원문 수신처럼 JSON 이 아닌 응답을 받을 때 지정한다</param>
        internal static Dictionary<string, string> Alimtalk(string accept = null)
        {
            var headers = new Dictionary<string, string>
            {
                { "BOOTPAY-ROLE", "user" }
            };
            if (!string.IsNullOrEmpty(accept))
            {
                headers["Accept"] = accept;
            }
            return headers;
        }

        /// <summary>
        /// V1 Mall API 요청 헤더 — Bootpay-User-JWT 는 값이 있을 때만 부착된다.
        /// BOOTPAY-ROLE 은 항상 user (Mall API 엔드포인트는 전부 user 스코프).
        /// </summary>
        internal static Dictionary<string, string> Mall(string userJwt = null, string idempotencyKey = null)
        {
            var headers = WithRole("user", idempotencyKey);
            if (!string.IsNullOrEmpty(userJwt))
            {
                headers["Bootpay-User-JWT"] = userJwt;
            }
            return headers;
        }
    }
}
