using Dapper;
using JIYITECH.WebApi.Configs;
using JIYITECH.WebApi.Entities;
using JIYITECH.WebApi.Repositories;
using JIYITECH.WebApi.Services;
using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace JIYITECH.WebApi.Controllers
{
    public interface IRoleService
    {
        IEnumerable<Role> GetAllRoles();
        IEnumerable<Role> GetRolesByUserId(long userId);
        /// <summary>
        /// 保存 Reole 对应 Permission API 
        /// pagePermission 可为 “,”隔开数组
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="pagePermission"></param>
        /// <returns></returns>
        IActionResult SaveRolePermission(long roleId, string pagePermission);
        /// <summary>
        /// 根据roleId 获取API菜单
        /// roleid若为多个 可为 “,”隔开数组
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        IEnumerable<string> GetRolePermission(string roleId);
        /// <summary>
        /// 获取所有API按钮权限操作
        /// </summary>
        /// <returns></returns>
        JArray GetPermission();
        /// <summary>
        /// Role列表分页
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        object GetRoleList(string input, int pageSize, int pageNumber);
        /// <summary>
        /// 是否具有重复名称
        /// </summary>
        /// <param name="role">role</param>
        /// <returns></returns>
        bool RepetName(Role role);
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="role">role</param>
        /// <returns></returns>
        IActionResult Add(Role role);
        /// <summary>
        /// 删除一个角色
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        IActionResult Delete(long id);
        /// <summary>
        /// 修改一个角色信息
        /// </summary>
        /// <param name="role">role</param>
        /// <returns></returns>
        IActionResult Update(Role role);
    }
    internal class RoleService : ControllerBase, IRoleService
    {

        private readonly AppConfig appConfig;
        private readonly ILog log;
        private readonly IUnitOfWork uow;
        public RoleService(IOptions<AppConfig> appConfig, IUnitOfWork uow)
        {
            this.appConfig = appConfig.Value;

            this.uow = uow;
            log = LogManager.GetLogger(Startup.repository.Name, typeof(RoleService));
        }
        public IEnumerable<Role> GetAllRoles()
        {
            // return roleRepo.GetAll();
            return uow.RoleRepository.GetModelList("", null);
        }

        public IEnumerable<Role> GetRolesByUserId(long userId)
        {
            return uow.RoleRepository.GetRolesByUserId(userId);
        }

        public object GetRoleList(string input, int pageSize, int pageNumber)
        {
            return uow.RoleRepository.GetListPaged(pageNumber, pageSize, $"where roleName like '%{input ?? ""}%'", "id");
        }

        public IActionResult SaveRolePermission(long roleId, string pagePermission)
        {
            try
            {
                //删除现有的 RolePermission 中记录
                uow.RolePermissionRepository.DeleteList($"where roleId={roleId}", null);
                //根据PermissionName 获取permission Id
                var permissionList = uow.PermissionRepository.GetModelList($"where permissionName in ({pagePermission.Replace("\"", "'")})", null);
                foreach (var item in permissionList)
                {
                    uow.RolePermissionRepository.Add(new RolePermission
                    {
                        roleId = roleId,
                        permissionId = item.id
                    });
                }
                uow.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                uow.Rollback();
                return BadRequest(ex);
                throw;
            }
        }

        public IEnumerable<string> GetRolePermission(string roleId)
        {
            using (IDbConnection db = new SqlConnection(appConfig.SQLConnectionStrings))
            {
                return db.Query<string>($"select distinct s2.permissionName  from [Sys_Role_Permission]  s1 LEFT JOIN Sys_Permission s2 on s1.permissionId=s2.id where s1.roleId in({roleId})");
            }
        }

        public JArray GetPermission()
        {
            using (IDbConnection db = new SqlConnection(appConfig.SQLConnectionStrings))
            {
                var jarr = new JArray();
                var arr = db.Query<string>("SELECT permissionType FROM [dbo].[Sys_Permission] GROUP BY permissionType");
                foreach (var item in arr)
                {
                    var list = db.Query<string>($"SELECT permissionName FROM [dbo].[Sys_Permission] where permissionType='{item}'").ToList();
                    var objList = new List<object>();
                    list.ForEach(i =>
                    {
                        objList.Add(new
                        {
                            name = i
                        });
                    });
                    var jobj = new JObject()
                    {
                        { "name",item},
                        { "children",JArray.FromObject(objList)}
                    };
                    jarr.Add(jobj);
                }
                return jarr;
            }
        }

        /// <summary>
        /// 判断是否有重名 roleName
        /// </summary>
        /// <param name="role">role</param>
        /// <returns></returns>
        public bool RepetName(Role role)
        {
            var strWhere = $"where roleName='{role.roleName}' or id={role.id}";
            var count = uow.RoleRepository.GetModelList(strWhere, null).Count();
            return role.id != 0 ? count > 1 : count > 0;
        }

        /// <summary>
        /// 是否存在重复名称或错误命名
        /// </summary>
        /// <param name="role"></param>
        private bool Exception(Role role)
        {
            if (role.roleName == null || string.IsNullOrWhiteSpace(role.roleName))
            {
                log.Error("RoleName错误命名");
                throw new Exception("RoleName错误命名");
            }
            if (RepetName(role))
            {
                log.Error("RoleName已存在该名称");
                throw new Exception("RoleName已存在该名称");
            }
            return false;
        }

        /// <summary>
        /// 添加一个角色
        /// </summary>
        /// <param name="role">role</param>
        /// <returns></returns>
        public IActionResult Add(Role role)
        {
            try
            {
                Exception(role);
                uow.RoleRepository.Add(role);
                return Ok();
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return BadRequest(ex);
            }
        }

        /// <summary>
        /// 删除一个角色
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public IActionResult Delete(long id)
        {
            try
            {
                if (uow.UserRoleRepository.GetModelList($"where roleId={id}", null).Count() != 0)
                {
                    throw new Exception("请先删除约束");
                }
                //Id 查询出对应Role 
                var model = uow.RoleRepository.GetModel(id);
                // 删除 rolepermission 中该 rolnane name 的所有记录
                uow.RolePermissionRepository.DeleteList($"where roleId={id}", null);
                // 清空 menu 表中该rolename 所有记录
                var menuModel = uow.MenuRepository.Head();
                var newMenu = uow.MenuRepository.MenuOperation((JArray)JsonConvert.DeserializeObject(menuModel.menu.Trim()), model.roleName, new List<string>());
                uow.MenuRepository.Update(new Menu
                {
                    id = menuModel.id,
                    menu = newMenu.ToString(),
                });
                // 删除该记录
                uow.RoleRepository.Delete(id);
                uow.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                uow.Rollback();
                return BadRequest(ex);
                throw;
            }
        }

        /// <summary>
        /// 修改一个角色信息
        /// </summary>
        /// <param name="role">role</param>
        /// <returns></returns>
        public IActionResult Update(Role role)
        {
            try
            {
                Exception(role);
                //是否 rolename 改变
                if (uow.RoleRepository.GetModelList($"where id={role.id} and roleName='{role.roleName}'", null).Count() == 0)
                {
                    var menuModel = uow.MenuRepository.Head();
                    // menu 中换名
                    var newMenu = uow.MenuRepository.ChangeName((JArray)JsonConvert.DeserializeObject(menuModel.menu.Trim()), uow.RoleRepository.GetModel(role.id).roleName, role.roleName);
                    uow.MenuRepository.Update(new Menu
                    {
                        id = menuModel.id,
                        menu = newMenu.ToString(),
                    });
                }
                uow.RoleRepository.Update(role);
                uow.Commit();
                return Ok();
            }
            catch (Exception ex)
            {
                uow.Rollback();
                return BadRequest(ex);
                throw;
            }
        }
    }
}
