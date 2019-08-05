using Dapper;
using ServiceStack.DataAnnotations;
using System;

namespace JIYITECH.WebApi.Entities
{
    [Alias("Sys_User_Role"), Table("Sys_User_Role")]
    public class UserRole : BaseEntity<long>
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        [ForeignKey(typeof(User))]
        public long userId { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        [ForeignKey(typeof(Role))]
        public long roleId { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        [Ignore]
        public Role role { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        [Ignore]
        public User user { get; set; }
    }
}
