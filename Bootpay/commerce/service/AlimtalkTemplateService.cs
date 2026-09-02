using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Bootpay.Commerce.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bootpay.Commerce.Service
{
    /// <summary>
    /// 가맹점 자체 알림톡 템플릿 CRUD·등록·검수 — /v1/alimtalk/templates 계열
    ///
    /// 흐름: (초안 생성 → 확인 → 대행사 등록) → 검수 요청 → 승인(APR) → 발송 가능.
    ///   Create(Register = false) 로 초안만 만들고, 내용을 확인한 뒤 Register() 로 올리는 것을 권장한다.
    ///
    /// ⚠️ register 를 명시적으로 false 로 주지 않으면 생성 즉시 대행사·카카오에 실제 등록된다.
    /// ⚠️ 본문 변수는 #{변수명} 형식이고 템플릿 전체에서 최대 40개다.
    /// </summary>
    public class AlimtalkTemplateService
    {
        /// <summary>
        /// 내 자체 템플릿 목록 조회 (GET /v1/alimtalk/templates)
        /// ins: 검수상태 필터 — 1 REG(등록) / 2 REQ(검수요청) / 3 APR(승인) / 4 KRR(등록거절) / 5 REJ(승인반려).
        ///      숫자·숫자문자열·벤더 문자열("APR" 등)을 모두 받는다. 해석 못 하는 값은 필터 없음으로 떨어진다.
        /// ⚠️ 페이지네이션이 없다 — 필터에 걸린 템플릿을 한 번에 모두 돌려준다.
        /// </summary>
        public static async Task<HttpResponseMessage> List(BootpayCommerceObject bootpay, AlimtalkTemplateListParams listParams = null)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (listParams != null)
            {
                if (!string.IsNullOrEmpty(listParams.Ins)) queryParams["ins"] = listParams.Ins;
                if (!string.IsNullOrEmpty(listParams.Sort)) queryParams["sort"] = listParams.Sort;
                if (!string.IsNullOrEmpty(listParams.Keyword)) queryParams["keyword"] = listParams.Keyword;
            }

            return await bootpay.SendAsync($"alimtalk/templates{AlimtalkQuery.Suffix(queryParams)}", HttpMethod.Get, null, CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 자체 템플릿 생성 (POST /v1/alimtalk/templates)
        /// ⚠️ Register 를 false 로 주지 않으면 대행사·카카오에 실제 등록된다(되돌리려면 삭제해야 한다).
        ///
        /// EmphasizeType: NONE·TEXT(강조표기형)·IMAGE(이미지형)·ITEM_LIST(아이템리스트형)
        ///   - TEXT 는 EmphasizeTitle·EmphasizeSubtitle 둘 다 필수 (각 50자·40자)
        ///   - IMAGE 는 이미지 필수 — Image() 로 올린 URL 을 StorageImageUrl 로 넘긴다
        ///   - ITEM_LIST 는 TemplateItem.list(2~10개) 필수 + TemplateHeader·ItemHighlight·이미지 중 하나 이상
        /// MsgType: BA(기본형)·EX(부가정보형, TemplateExtra 필수)·AD(채널추가형)·MI(복합형)
        ///   - AD·MI 는 채널추가(AC) 버튼이 필수다
        /// Examples: 변수 예문(표시용). 주면 모든 변수에 예문이 있어야 한다(없으면 3017).
        /// </summary>
        public static async Task<HttpResponseMessage> Create(BootpayCommerceObject bootpay, AlimtalkTemplateCreateParams createParams)
        {
            return await bootpay.SendAsync("alimtalk/templates", HttpMethod.Post, Payload(createParams), CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 자체 템플릿 상세 조회 (GET /v1/alimtalk/templates/{template_id})
        /// templateId 는 문서 id 이고, ObjectId 형식이 아니면 템플릿 코드로 해석한다.
        /// ⚠️ sync 는 서버 기본값이 true 라 조회만 해도 벤더 상태 동기화가 일어난다.
        ///    초안(등록 전)을 조회할 때는 sync 를 false 로 주는 것을 권장한다.
        /// </summary>
        /// <param name="bootpay">Bootpay Commerce 객체</param>
        /// <param name="templateId">템플릿 ID 또는 템플릿 코드</param>
        /// <param name="sync">벤더 동기화 여부 (선택, 서버 기본 true)</param>
        public static async Task<HttpResponseMessage> Detail(BootpayCommerceObject bootpay, string templateId, bool? sync = null)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            if (sync.HasValue) queryParams["sync"] = sync.Value ? "true" : "false";

            return await bootpay.SendAsync($"alimtalk/templates/{templateId}{AlimtalkQuery.Suffix(queryParams)}", HttpMethod.Get, null, CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 자체 템플릿 수정 (PUT /v1/alimtalk/templates/{template_id})
        /// ⚠️ 부분 수정이 아니다. 보내지 않은 필드는 null 로 덮어써지므로 항상 전체 필드를 보낸다.
        /// ⚠️ 등록된 템플릿을 수정하면 벤더에도 수정 요청이 나간다.
        ///    수정 가능 상태는 초안 / REG(등록) / REJ(승인반려) / KRR(등록거절) 뿐이다 — APR·REQ 는 거부된다.
        /// StorageImageUrl 을 빈 값으로 보내면 이미지 삭제로 처리되어 벤더에도 전달된다.
        /// </summary>
        public static async Task<HttpResponseMessage> Update(BootpayCommerceObject bootpay, string templateId, AlimtalkTemplateUpdateParams updateParams)
        {
            return await bootpay.SendAsync($"alimtalk/templates/{templateId}", HttpMethod.Put, Payload(updateParams), CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 자체 템플릿 삭제 (DELETE /v1/alimtalk/templates/{template_id})
        /// 초안(등록 전)은 대행사 거부와 무관하게 로컬에서 삭제된다.
        /// ⚠️ 등록분은 대행사 삭제가 성공해야 삭제된다 — 승인(APR) 템플릿은 카카오가 거부하므로
        ///    500(3013)이 오고 템플릿은 남는다. 같은 코드가 대행사에 선점된 채 로컬만 사라지는 것을 막기 위함이다.
        /// </summary>
        public static async Task<HttpResponseMessage> Delete(BootpayCommerceObject bootpay, string templateId)
        {
            return await bootpay.SendAsync($"alimtalk/templates/{templateId}", HttpMethod.Delete, null, CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 초안을 대행사에 등록 (POST /v1/alimtalk/templates/{template_id}/register)
        /// ⚠️ 대행사·카카오에 실제 등록된다. 등록 전(초안) 상태에서만 호출할 수 있다.
        /// </summary>
        public static async Task<HttpResponseMessage> Register(BootpayCommerceObject bootpay, string templateId)
        {
            return await bootpay.SendAsync($"alimtalk/templates/{templateId}/register", HttpMethod.Post, new JObject(), CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 검수 요청 (POST /v1/alimtalk/templates/{template_id}/inspect)
        /// ⚠️ 카카오에 검수를 요청하며 취소할 수 없다.
        /// 대행사 등록이 끝난 대기(R) + REG(등록) 상태에서만 호출할 수 있다 — 초안은 먼저 Register() 를 부른다.
        /// 반려(REJ/KRR)된 건은 재요청이 아니라 수정 후 재요청이다. 반려 사유는 응답의 comments 에 담긴다.
        /// </summary>
        public static async Task<HttpResponseMessage> Inspect(BootpayCommerceObject bootpay, string templateId)
        {
            return await bootpay.SendAsync($"alimtalk/templates/{templateId}/inspect", HttpMethod.Post, new JObject(), CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 템플릿 목록 내보내기 (GET /v1/alimtalk/templates/export)
        /// scope: private(기본, 내 채널 자체 템플릿)·official(공식 카탈로그)·all
        /// ⚠️ SDK 기본 format 을 json 으로 둔다 — 서버 기본은 csv 지만, csv 본문은 JSON 이 아니라서
        ///    JSON 을 기대하는 호출부에서 파싱이 깨진다. csv 를 주면 Accept 를 */* 로 바꿔 원문을 받는다.
        /// 1회 5,000건을 넘으면 3031 로 거부되므로 채널·상태 필터로 좁힌다.
        /// </summary>
        public static async Task<HttpResponseMessage> Export(BootpayCommerceObject bootpay, AlimtalkTemplateExportParams exportParams = null)
        {
            var format = string.IsNullOrEmpty(exportParams?.Format) ? "json" : exportParams.Format;

            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            queryParams["format"] = format;
            if (exportParams != null)
            {
                if (!string.IsNullOrEmpty(exportParams.Scope)) queryParams["scope"] = exportParams.Scope;
                if (!string.IsNullOrEmpty(exportParams.KspId)) queryParams["ksp_id"] = exportParams.KspId;
                if (!string.IsNullOrEmpty(exportParams.Status)) queryParams["status"] = exportParams.Status;
                if (exportParams.IncludeContent.HasValue) queryParams["include_content"] = exportParams.IncludeContent.Value ? "true" : "false";
            }

            var headers = string.Equals(format, "csv", System.StringComparison.OrdinalIgnoreCase)
                ? CommerceRequestHeaders.Alimtalk("*/*")
                : CommerceRequestHeaders.Alimtalk();

            return await bootpay.SendAsync($"alimtalk/templates/export{AlimtalkQuery.Suffix(queryParams)}", HttpMethod.Get, null, headers);
        }

        /// <summary>
        /// 이미지형 템플릿의 원본 이미지 업로드 (POST /v1/alimtalk/templates/image)
        /// 돌려받은 image_url 을 템플릿 생성/수정의 StorageImageUrl 로 넘긴다.
        /// 규격을 업로드 전에 서버가 검사한다 — jpg/png · 500KB 이하 · 가로 500px 이상 · 2:1.
        /// </summary>
        /// <param name="bootpay">Bootpay Commerce 객체</param>
        /// <param name="imagePath">이미지 파일 경로</param>
        /// <param name="replaceUrl">주면 업로드 성공 후에 기존 파일을 지운다</param>
        public static async Task<HttpResponseMessage> Image(BootpayCommerceObject bootpay, string imagePath, string replaceUrl = null)
        {
            return await Upload(bootpay, "alimtalk/templates/image", imagePath, replaceUrl);
        }

        /// <summary>
        /// 아이템리스트형의 하이라이트 썸네일 업로드 (POST /v1/alimtalk/templates/highlight_image)
        /// ⚠️ 본문 이미지와 규격이 다르다 — jpg/png · 500KB 이하 · 가로 108px 이상 · 1:1.
        ///    본문 이미지 endpoint 로 올리면 거부된다.
        /// 돌려받은 image_url 은 ItemHighlight 의 storage_image_url 로 넘긴다.
        /// ⚠️ 썸네일을 붙이면 하이라이트 글자 한도가 줄어든다 (타이틀 30→21, 설명 19→13).
        /// </summary>
        /// <param name="bootpay">Bootpay Commerce 객체</param>
        /// <param name="imagePath">이미지 파일 경로</param>
        /// <param name="replaceUrl">주면 업로드 성공 후에 기존 파일을 지운다</param>
        public static async Task<HttpResponseMessage> HighlightImage(BootpayCommerceObject bootpay, string imagePath, string replaceUrl = null)
        {
            return await Upload(bootpay, "alimtalk/templates/highlight_image", imagePath, replaceUrl);
        }

        private static async Task<HttpResponseMessage> Upload(BootpayCommerceObject bootpay, string uri, string imagePath, string replaceUrl)
        {
            var form = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(replaceUrl)) form["replace_url"] = replaceUrl;

            return await bootpay.SendMultipartFileAsync(uri, "image", imagePath, form, CommerceRequestHeaders.Alimtalk());
        }

        /// <summary>
        /// 템플릿 본문을 JSON 으로 만들고 Attrs 를 덮어쓴다 (Ruby SDK 의 .merge(attrs).compact 와 같은 순서).
        /// 지정하지 않은(null) 필드는 담기지 않는다.
        /// </summary>
        private static JObject Payload(AlimtalkTemplateParams templateParams)
        {
            if (templateParams == null) return new JObject();

            var serializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            var body = JObject.FromObject(templateParams, serializer);
            if (templateParams.Attrs != null)
            {
                foreach (var attr in templateParams.Attrs)
                {
                    if (attr.Value == null) continue;
                    body[attr.Key] = JToken.FromObject(attr.Value, serializer);
                }
            }
            return body;
        }
    }
}
