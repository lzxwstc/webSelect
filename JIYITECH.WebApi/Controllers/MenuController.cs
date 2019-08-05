using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JIYITECH.WebApi.Entities;
using JIYITECH.WebApi.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using JIYITECH.WebApi.Repositories;

namespace JIYITECH.WebApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService menuService;
        public MenuController(IMenuService menuService)
        {
            this.menuService = menuService;
        }

        /// <summary>
        /// 获取Menu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetMenu"), AllowAnonymous]
        public IActionResult GetMenu(int id)
        {
            return Ok(menuService.GetMenu());
        }

        /// <summary>
        /// 更新Menu
        /// </summary>
        /// <param name="menu">菜单</param>
        /// <returns></returns>
        [HttpPost("{id}", Name = "PostMenu")]
        public IActionResult PostMenu(JArray menu)
        {
            return Ok(menuService.PostMenu(menu));
        }

        /// <summary>
        /// 保存Menu 
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <param name="pageArr">拥有的page 页</param>
        /// <returns></returns>
        [HttpGet(Name = "saveMenu"), AllowAnonymous]
        public IActionResult SaveMenu(string roleName,List<string> pageArr)
        {
           
            return Ok(menuService.SaveMenu(roleName, pageArr));
        }

        /// <summary>
        /// 角色换名 
        /// </summary>
        /// <param name="oldroleName">旧名字</param>
        /// <param name="newroleName">新名字</param>
        /// <returns></returns>
        [HttpGet, Route("[action]"), AllowAnonymous]
        public IActionResult UpdateRoleName(string oldroleName, string newroleName)
        {
            return Ok(menuService.UpdateRoleName(oldroleName, newroleName));
        }       
    }
}
