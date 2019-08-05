using Dapper;
using ServiceStack.DataAnnotations;
using System;

namespace JIYITECH.WebApi.Entities
{
    [Alias("reports"), Table("reports")]
    public class Reports : BaseEntity<long>
    {
        /// <summary>
        /// 燃料数据
        /// </summary>
        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float ad { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public string coalname { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float fcd { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public DateTime? firstdate { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float mtar { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float qnetar { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float shipcoalprice { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public string shipname { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public string shiptime { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public int st { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float std { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public string type { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float vdaf { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public long shipping_list_id { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public int hydstatue { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public long sample_info_id { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float sar { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float nd { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float aad { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float aar { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float adaf { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public string adatum { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float cad { get; set; }
        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float car { get; set; }
        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float cd { get; set; }
        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float cdaf { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public string cdatum { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float had { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float har { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float hd { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float hdaf { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public string hdatum { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float nad { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float nar { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float ndaf { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public string ndatum { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float oad { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float oar { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float od { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float odaf { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public string odatum { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float sad { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float sdaf { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public string sdatum { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float mad { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float qgrar { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public int dt { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public int ft { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public int ht { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float fcdaf { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float fcar { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float fcad { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float vad { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float vd { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public float var { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public string typename { get; set; }

        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public bool isDefault { get; set; }
    }
}
