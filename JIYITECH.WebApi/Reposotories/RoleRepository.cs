using JIYITECH.WebApi.Configs;
using JIYITECH.WebApi.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using JIYITECH.WebApi.Reposotories;
using System.Data.Common;
using JIYITECH.WebApi.Services;

namespace JIYITECH.WebApi.Repositories
{
    public interface IRoleRepository : IBaseRepository<Role>
    {
        IEnumerable<Role> GetRolesByUserId(long userId);
        IEnumerable<Role> GetAll();
        Role GetRoleByName(string roleName);
    }
    public class RoleRepository : BaseRepository<Role>, IRoleRepository
    {
        public RoleRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        /// <summary>
        /// 获取全部用户
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Role> GetAll()
        {
            return db.Query<Role>("select * from [smartcoal].[dbo].[sys_role]", transaction: Transaction);
        }

        /// <summary>
        /// 根据角色名获取角色
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <returns></returns>
        public Role GetRoleByName(string roleName)
        {
            return db.QuerySingle<Role>($"select * from [smartcoal].[dbo].[sys_role] where roleName = '{roleName}'", transaction: Transaction);
        }

        /// <summary>                                      
        /// 根据用户id获取权限
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <returns></returns>
        public IEnumerable<Role> GetRolesByUserId(long userId)
        {
            var res = db.Query<Role>($"select r.* from [smartcoal].[dbo].[sys_user_role] as ur inner join [smartcoal].[dbo].[sys_role] as r on ur.roleId = r.id where ur.userId = {userId}", transaction: Transaction);
            return res;
        }
    }
}
