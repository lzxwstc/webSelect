using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using ServiceStack.DataAnnotations;

namespace JIYITECH.WebApi.Entities
{
    [Alias("Up"), Table("Up")]
    public class DayDayUp : BaseEntity<long>
    {
        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public string DayName { get; set; }
        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public long DayData1 { get; set; }
        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public long DayData2 { get; set; }
        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public long DayData3 { get; set; }


    }
}
