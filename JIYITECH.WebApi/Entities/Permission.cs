using Dapper;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIYITECH.WebApi.Entities
{
    [Alias("Sys_Permission"), Table("Sys_Permission")]

    public class Permission : BaseEntity<long>
    {
        /// <summary>
        /// 权限
        /// </summary>
        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public string permissionName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string permissionType { get; set; }
    }
}
