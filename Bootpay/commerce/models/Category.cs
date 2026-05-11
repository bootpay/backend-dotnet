using System.Collections.Generic;
using Newtonsoft.Json;

namespace Bootpay.Commerce.Models
{
    /// <summary>
    /// Commerce 카테고리
    /// </summary>
    public class CommerceCategory
    {
        [JsonProperty("category_id")]
        public string CategoryId { get; set; }

        [JsonProperty("seller_id")]
        public string SellerId { get; set; }

        [JsonProperty("project_id")]
        public string ProjectId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parent_category_id")]
        public string ParentCategoryId { get; set; }

        [JsonProperty("parent_categories")]
        public List<string> ParentCategories { get; set; }

        [JsonProperty("status_display")]
        public bool? StatusDisplay { get; set; }

        [JsonProperty("status_best")]
        public bool? StatusBest { get; set; }

        [JsonProperty("filter_color")]
        public int? FilterColor { get; set; }

        [JsonProperty("filter_size")]
        public int? FilterSize { get; set; }

        [JsonProperty("idx")]
        public int? Idx { get; set; }

        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public string UpdatedAt { get; set; }
    }

    /// <summary>
    /// 카테고리 생성 파라미터
    /// </summary>
    public class CategoryCreateParams
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parent_category_id")]
        public string ParentCategoryId { get; set; }

        [JsonProperty("status_display")]
        public bool? StatusDisplay { get; set; }

        [JsonProperty("status_best")]
        public bool? StatusBest { get; set; }

        [JsonProperty("filter_color")]
        public int? FilterColor { get; set; }

        [JsonProperty("filter_size")]
        public int? FilterSize { get; set; }
    }

    /// <summary>
    /// 카테고리 수정 파라미터
    /// </summary>
    public class CategoryUpdateParams
    {
        [JsonIgnore]
        public string CategoryId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("parent_category_id")]
        public string ParentCategoryId { get; set; }

        [JsonProperty("status_display")]
        public bool? StatusDisplay { get; set; }

        [JsonProperty("status_best")]
        public bool? StatusBest { get; set; }

        [JsonProperty("filter_color")]
        public int? FilterColor { get; set; }

        [JsonProperty("filter_size")]
        public int? FilterSize { get; set; }
    }
}
