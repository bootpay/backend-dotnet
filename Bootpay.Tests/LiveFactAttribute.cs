using Xunit;

namespace Bootpay.Tests
{
    /// <summary>
    /// 라이브 API 를 호출하는 테스트 전용 게이트 — BOOTPAY_ENV=development 일 때만 실행된다.
    /// 기본값(production)으로 전체 스위트를 돌려도 production 호출이 0건이 되도록 발견 시점에 skip 처리한다.
    /// 네트워크를 쓰지 않는 테스트(wire mock, reflection)는 일반 [Fact] 를 유지한다.
    /// </summary>
    public sealed class LiveFactAttribute : FactAttribute
    {
        public LiveFactAttribute()
        {
            if (TestConfig.Env != "development")
            {
                Skip = "development 환경 전용 — BOOTPAY_ENV=development 로 실행 (production 호출 금지)";
            }
        }
    }
}
