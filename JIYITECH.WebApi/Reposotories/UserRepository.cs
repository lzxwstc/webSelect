using Dapper;
using JIYITECH.WebApi.Entities;
using JIYITECH.WebApi.Configs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Linq;
using log4net;
using JIYITECH.WebApi.Services;

namespace JIYITECH.WebApi.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        /// <summary>
        /// 获取用户实体
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        User Find(string userName, string password);
        /// <summary>
        /// 权限校验
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="permissionName">权限名</param>
        /// <returns></returns>
        bool CheckPermission(long userId, string permissionName);
        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        List<Permission> GetPermissionsByUserId(long userId);
        /// <summary>
        /// 获取用户盐值
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        byte[] GetSaltByUserName(string userName);
        string GetSaltByUserName2(string userName);
        ///// <summary>
        ///// 根据token获取用户信息
        ///// </summary>
        ///// <param name="token">token</param>
        ///// <returns></returns>
        //object GetUserInfoByToken(string token);
        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns></returns>
        int ChangePassword(long id, string oldPassword, string newPassword);
        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="status">撞态</param>
        /// <returns></returns>
        int ChangeStatus(long id, bool status);
        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="name">用户名称</param>
        /// <param name="roles">用户权限</param>
        /// <returns></returns>
        bool ChangeUserInfo(long id, string name, object roles);
        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="user">用户</param>
        /// <returns></returns>
        int? Add(User user);
        User GetValidUser(string userName, string password);
        new object GetListPaged(int pageNumber, int rowsPerPage, string strWhere, string orderBy, object parameters = null);
        new int Delete(long id);
    }
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly ILog log;
        public UserRepository(IDbTransaction transaction) : base(transaction)
        {
            log = LogManager.GetLogger(Startup.repository.Name, typeof(UserRepository));
        }

        public User Find(string userName, string password)
        {
            User user = db.QuerySingle<User>("select top 1 * from [smartcoal].[dbo].[sys_user] where userName = '{userName}' and password = '{password}'", transaction: Transaction);
            IEnumerable<UserRole> userRoles = db.GetList<UserRole>($"select * from [smartcoal].[dbo].[sys_user_role] where userId = {user.id}", transaction: Transaction);
            List<string> roles = new List<string>();
            userRoles.AsList().ForEach(x => roles.Add(db.Get<Role>(x.roleId, transaction: Transaction).roleName));
            return user;
        }

        public bool CheckPermission(long userId, string permissionName)
        {
            var user = GetModel(userId);
            if (user == null)
            {
                return false;
            }
            var permissions = GetPermissionsByUserId(userId);
            var permissionExist = permissions.FindAll(p => p.permissionName == permissionName).Count != 0;
            return permissionExist;
        }

        public List<Permission> GetPermissionsByUserId(long userId)
        {
            var sql = $"select p.* from Sys_Permission p inner join Sys_Role_Permission rp on p.id = rp.permissionId inner join Sys_User_Role ur on ur.roleId = rp.roleId where ur.id = {userId}";
            List<Permission> permissions = db.Query<Permission>(sql, transaction: Transaction).AsList();
            return permissions;
        }

        /// <summary>
        /// 获取指定用户盐值
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public byte[] GetSaltByUserName(string userName)
        {
            byte[] salt = db.QuerySingle<string>($"select top 1 salt from sys_user where userName = '{userName}'", transaction: Transaction).Split(",").AsList().ConvertAll(x => byte.Parse(x)).ToArray();
            return salt;
        }
        public string GetSaltByUserName2(string userName)
        {
           string salt = db.QuerySingle<string>($"select top 1 salt from sys_user where userName = '{userName}'", transaction: Transaction);
            return salt;
        }
        //public object GetUserInfoByToken(string token)
        //{
        //    try
        //    {
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        if (!(tokenHandler.ReadToken(token) is JwtSecurityToken jwtToken))
        //        {
        //            return null;
        //        }
        //        //var symmetricKey = Convert.FromBase64String(_appConfig.tokenSecret);
        //        //var validationParameters = new TokenValidationParameters()
        //        //{
        //        //    //去除时间钟摆
        //        //    ClockSkew = TimeSpan.Parse("00:00:00"),
        //        //    RequireExpirationTime = true,
        //        //    ValidateIssuer = false,
        //        //    ValidateAudience = false,
        //        //    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
        //        //};
        //        //var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
        //        User user = GetModel(long.Parse(jwtToken.Payload["id"].ToString()));
        //        return new
        //        {
        //            user.userName,
        //            roles = JArray.Parse(jwtToken.Payload["role"].ToString()),
        //            user.name,
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.ToString());
        //        return new ClaimsPrincipal();
        //    }
        //}

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newPassword">新密码</param>
        /// <param name="oldPassword">旧密码</param>
        /// <returns></returns>
        public int ChangePassword(long userId, string oldPassword, string newPassword)
        {
            return db.Execute($"update sys_user set [password] = '{newPassword}',[updateTime] = '{DateTime.Now}' where id = {userId}", transaction: Transaction);
        }
        /// <summary>
        /// 修改用户名
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="name">用户名称</param>
        /// <param name="roles">用户权限数组</param>
        /// <returns></returns>
        public bool ChangeUserInfo(long userId, string name, object roles)
        {
            try
            {
                db.Execute($"update [smartcoal].[dbo].[sys_user] set [name] = '{name}',[updateTime] = '{DateTime.Now}' where id = {userId}", null, transaction: Transaction);
                ChangeRole(db, userId, roles);
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
            return true;
        }
        /// <summary>
        /// 更改用户状态
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="status">是否启用</param>
        /// <returns></returns>
        public int ChangeStatus(long id, bool status)
        {
            return db.Execute($"update sys_user set [enabled] = {(status ? 1 : 0)},[updateTime] = '{DateTime.Now}' where id = {id}", transaction: Transaction);
        }
        /// <summary>
        /// 插入用户
        /// use 'new' to hide the same method from baseRepository
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public int? Add(User user)
        {
            return db.Insert(user, transaction: Transaction);
        }
        /// <summary>
        /// 修改用户权限
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userId">用户ID</param>
        /// <param name="roleNames">用户权限</param>
        /// <returns></returns>
        private void ChangeRole(IDbConnection db, long userId, object roleNames)
        {
            // todo 本方法应放在service层
            //JArray roleNameSelected = JArray.FromObject(roleNames);
            //JArray userRoleNames = JArray.FromObject(db.Query<string>($"select r.roleName from [smartcoal].[dbo].[sys_role] r left join [smartcoal].[dbo].[sys_user_role] ur " +
            //    $"on r.id = ur.roleId where ur.userId = {userId}", null, transaction: Transaction));
            //var roleTobeAdd = roleNameSelected.Except(userRoleNames);
            //var roleTobeDelete = userRoleNames.Except(roleNameSelected);
            //foreach (var item in roleTobeAdd)
            //{
            //    db.Insert(new UserRole
            //    {
            //        userId = userId,
            //        roleId = db.GetRoleByName(item.Value<string>()).id
            //    }, transaction);
            //}
            //foreach (var item in roleTobeDelete)
            //{
            //    db.Execute($"delete [smartcoal].[dbo].[sys_user_role] where userId = {userId} and roleId = {roleRepo.GetRoleByName(item.Value<string>()).id}", null, transaction);
            //}
        }

        public User GetValidUser(string userName, string password)
        {
            return db.QuerySingle<User>($"select top 1 * from [smartcoal].[dbo].[sys_user] where userName = '{userName}' and password = '{password}'", transaction: Transaction);
        }

        public new object GetListPaged(int pageNumber, int rowsPerPage, string strWhere, string orderBy, object parameters)
        {
            var list = db.GetListPaged<User>(pageNumber, rowsPerPage, strWhere, orderBy, parameters, transaction: Transaction);
            foreach (var item in list)
            {
                List<string> roles = new List<string>();
                db.Query<UserRole, Role, User, UserRole>($"select ur.*,r.*,u.* from sys_user_role ur left " +
                    $"join sys_role r on ur.roleId = r.id left join sys_user u on ur.userId = u.id where ur.userId = {item.id}", (userRole, role, user) =>
                    {
                        userRole.role = role;
                        userRole.user = user;
                        return userRole;
                    }, splitOn:
                "roleName,userName", transaction: Transaction).AsList().ForEach(x => roles.Add(x.role.roleName));
                item.roles = roles;
            }
            return new
            {
                total = Count(strWhere),
                list
            };
        }

        public new int Delete(long id)
        {
            try
            {
                db.Execute($"delete [smartcoal].[dbo].[sys_user_role] where userId = {id}", null, transaction: Transaction);
                var count = db.Delete<User>(id, transaction: Transaction);
                return count;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return 0;
            }
        }
    }
}
