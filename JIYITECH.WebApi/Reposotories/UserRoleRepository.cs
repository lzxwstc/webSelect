using Dapper;
using JIYITECH.WebApi.Configs;
using JIYITECH.WebApi.Entities;
using JIYITECH.WebApi.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace JIYITECH.WebApi.Repositories
{
    public interface IUserRoleRepository : IBaseRepository<UserRole>
    {
        /// <summary>
        /// 更新用户权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roles">用户权限</param>
        /// <returns></returns>
        int ChangeRoleById(long userId, JArray roles);
        /// <summary>
        /// 移除用户角色
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roleId">角色ID</param>
        /// <returns></returns>
        int Delete(long userId, long roleId);
        new int Delete(long userId);
    }
    public class UserRoleRepository : BaseRepository<UserRole>, IUserRoleRepository
    {
        public UserRoleRepository(IDbTransaction transaction) : base(transaction)
        {
        }

        /// <summary>
        /// 更新用户权限
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="roles">用户权限</param>
        /// <returns></returns>
        public int ChangeRoleById(long userId, JArray roles)
        {
            throw new NotImplementedException();
        }
        public int Delete(long userId, long roleId)
        {
            return db.Execute($"delete [smartcoal].[dbo].[sys_user_role] where userId = {userId} and roleId = {roleId}", null, transaction: Transaction);
        }
        public new int Delete(long userId)
        {
            return db.Execute($"delete [smartcoal].[dbo].[sys_user_role] where userId = {userId}", null, transaction: Transaction);
        }
    }


    #region IRolePermissionRepository
    public interface IRolePermissionRepository : IBaseRepository<RolePermission>
    {

    }

    public class RolePermissionRepository : BaseRepository<RolePermission>, IRolePermissionRepository
    {
        public RolePermissionRepository(IDbTransaction transaction) : base(transaction)
        {

        }
    }
    #endregion


    #region IPermissionRepository
    public interface IPermissionRepository : IBaseRepository<Permission>
    {

    }

    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(IDbTransaction transaction) : base(transaction)
        {

        }
    }
    #endregion
}
