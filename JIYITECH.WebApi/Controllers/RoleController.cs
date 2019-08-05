using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JIYITECH.WebApi.Entities;
using JIYITECH.WebApi.Filters;
using JIYITECH.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace JIYITECH.WebApi.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService roleService;
        private readonly IMenuService IMenuService;
        public RoleController(IRoleService roleService, IMenuService IMenuService)
        {
            this.roleService = roleService;
            this.IMenuService = IMenuService;
        }

        [HttpPost, Route("[action]"), AllowAnonymous]
        public object GetAll(string input, int pageSize, int pageNumber)
        {
            return roleService.GetRoleList(input, pageSize, pageNumber);
        }

        /// <summary>
        /// 获取所有 API 构造树形结构
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("[action]"), AllowAnonymous]
        public JArray GetPermission()
        {
            return roleService.GetPermission();
        }

        /// <summary>
        /// 根据roleId 获取 API 权限
        /// roleid若为多个 可为 “,”隔开数组
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet, Route("[action]"), AllowAnonymous]
        public IEnumerable<string> GetRolePermission(string roleId)
        {
            return roleService.GetRolePermission(roleId);
        }

        /// <summary>
        /// 保存 Permission API 操作
        /// pagePermission 为 “,”隔开数组
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="pagePermission"></param>
        /// <returns></returns>
        [HttpPut, Route("[action]"), AllowAnonymous]
        public IActionResult SaveRolePermission(long roleId, string pagePermission)
        {
            return roleService.SaveRolePermission(roleId, pagePermission);
        }

        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="role">角色名</param>
        [HttpPost, Route("[action]")]
        public IActionResult Put(Role role)
        {
            return roleService.Add(role);
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpDelete, Route("[action]")]
        public IActionResult Delete(long id)
        {
            return roleService.Delete(id);
        }

        /// <summary>
        /// 修改角色
        /// </summary>
        /// <param name="role">role</param>
        /// <returns></returns>
        [HttpPut, Route("[action]")]
        public IActionResult Update(Role role)
        {
            return roleService.Update(role);
        }
    }
}
