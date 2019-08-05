using Dapper;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIYITECH.WebApi.Entities
{
    [Alias("Sys_Menu"), Table("Sys_Menu")]
    public class Menu : BaseEntity<long>
    {
        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        [StringLength(Int32.MaxValue)]
        public string menu { get; set; }
    }
}
