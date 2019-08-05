using Dapper;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;

namespace JIYITECH.WebApi.Entities
{
    [Alias("Sys_User"), Table("Sys_User")]
    public class User : BaseEntity<long>
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public string userName { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public string name { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        [Dapper.IgnoreSelect, Ignore]
        public string password { get; set; }
        /// <summary>
        /// salt
        /// </summary>
        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        [Dapper.IgnoreSelect]
        public string salt { get; set; }
        /// <summary>
        /// 账号是否启用
        /// </summary>
        [ServiceStack.DataAnnotations.Required, Dapper.Required]
        public bool enabled { get; set; }
        /// <summary>
        /// 用户角色
        /// </summary>
        [Ignore]
        public List<string> roles { get; set; }
    }
}