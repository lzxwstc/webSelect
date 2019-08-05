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
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        /// <summary>
        /// 获取User
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetUser"), AllowAnonymous]
        //[Authorize(Policy = Permissions.UserUpdate)]
        //[PermissionFilter(Permissions.UserRead)]
        public IActionResult Get(long id)
        {
            return Ok(userService.GetUser(id));
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet, AllowAnonymous]
        public IActionResult Login([FromQuery]string userName, string password)
        {
            return Ok(userService.Login(userName, password));
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="user">axios对象，id用户ID,name用户名,roles用户权限数组</param>
        [HttpPost, Route("[action]")]
        [PermissionFilter(Permissions.UserUpdate)]
        public IActionResult ChangeUserInfo([FromBody]JObject user)
        {
            return Ok(userService.ChangeUserInfo(user));
        }

        /// <summary>
        /// 获取用户分页
        /// </summary>
        /// <param name="pageNumber">页码，0开始</param>
        /// <param name="rowsPerPage">每页数量</param>
        /// <param name="strWhere">where条件</param>
        /// <param name="orderBy">Order by排序</param>
        /// <param name="parameters">parameters参数</param>
        [PermissionFilter(Permissions.UserRead)]
        [HttpGet, Route("[action]")]
        public IActionResult GetListPaged([FromQuery]int pageNumber, int rowsPerPage, string strWhere, string orderBy, object parameters = null)
        {
            return Ok(userService.GetListPaged(pageNumber, rowsPerPage, strWhere, orderBy, parameters));
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="user">用户</param>
        [HttpPut, Route("[action]")]
        public IActionResult Put([FromBody]JObject user)
        {
            return Ok(userService.CreateUser(user));
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            return Ok(userService.RemoveUser(id));
        }
        /// <summary>
        /// 根据token获取用户信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet, Route("[action]")]
        public IActionResult GetUserInfoByToken(string token)
        {
            return Ok(userService.GetUserInfoByToken(token));
        }
        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <param name="oldPassword">旧密码</param>
        /// <param name="newPassword">新密码</param>
        /// <returns></returns>
        [HttpPost, Route("[action]")]
        public IActionResult ChangePassword(int id, string oldPassword, string newPassword)
        {
            return Ok(userService.ChangePassword(id, oldPassword, newPassword));
        }
        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="data">用户</param>
        /// <returns></returns>
        [HttpPost, Route("[action]")]
        [PermissionFilter(Permissions.UserUpdate)]
        public IActionResult ChangeStatus([FromBody]JObject data)
        {
            return Ok(userService.ChangeStatus(data));
        }
    }
}
