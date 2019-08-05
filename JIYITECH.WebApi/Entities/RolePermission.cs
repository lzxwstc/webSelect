using Dapper;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JIYITECH.WebApi.Entities
{
    [Alias("Sys_Role_Permission"), Table("Sys_Role_Permission")]
    public class RolePermission : BaseEntity<long>
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        [ForeignKey(typeof(Role))]
        public long roleId { get; set; }
        /// <summary>
        /// 权限ID
        /// </summary>
        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        [ForeignKey(typeof(Permission))]
        public long permissionId { get; set; }
        /// <summary>
        /// 权限
        /// </summary>
        [Ignore]
        public Permission permission { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        [Ignore]
        public Role role { get; set; }
    }
}
