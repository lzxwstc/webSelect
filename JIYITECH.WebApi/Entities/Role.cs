using Dapper;
using ServiceStack.DataAnnotations;
using System;

namespace JIYITECH.WebApi.Entities
{
    [Alias("Sys_Role"), Table("Sys_Role")]
    public class Role : BaseEntity<long>
    {
        /// <summary>
        /// 角色
        /// </summary>
        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public string roleName { get; set; }
    }
}
