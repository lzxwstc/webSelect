using JIYITECH.WebApi.Configs;
using JIYITECH.WebApi.Entities;
using JIYITECH.WebApi.Repositories;
using JIYITECH.WebApi.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace JIYITECH.WebApi.Controllers
{
    public interface IMenuService
    {
        object GetMenu();
        bool PostMenu(JArray menu);
        /// <summary>
        /// rolename
        /// </summary>
        /// <param name="oldRoleName">旧名字</param>
        /// <param name="newRoleName">新名字</param>
        bool UpdateRoleName(string oldRoleName, string newRoleName);
        /// <summary>
        /// 保存 角色对应菜单
        /// </summary>
        /// <param name="roleName">角色名</param>
        /// <param name="pageArr">拥有页面数组</param>
        /// <returns></returns>
        bool SaveMenu(string roleName, List<string> pageArr);
    }
    internal class MenuService : IMenuService
    {
        private readonly IUnitOfWork uow;
        public MenuService(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public object GetMenu()
        {
            return uow.MenuRepository.Head().menu;
        }

        public bool PostMenu(JArray menu)
        {
            return uow.MenuRepository.Add(menu.ToObject<Menu>()) > 0;
        }

        /// <summary>
        /// rolename 换名
        /// </summary>
        /// <param name="oldRoleName">旧名字</param>
        /// <param name="newRoleName">新名字</param>
        /// <returns></returns>
        public bool UpdateRoleName(string oldRoleName, string newRoleName)
        {
            try
            {
                var model = uow.MenuRepository.Head();
                var menu = model.menu;
                if (menu.IndexOf(oldRoleName) == -1) { return true; }
                var newMenu = uow.MenuRepository.ChangeName((JArray)JsonConvert.DeserializeObject(menu.Trim()), oldRoleName, newRoleName);
                uow.MenuRepository.Update(new Menu
                {
                    id = model.id,
                    menu = newMenu.ToString(),
                });
                uow.Commit();
                return true;
            }
            catch (Exception ex)
            {
                uow.Rollback();
                return false;
                // throw;
            }
        }


        /// <summary>
        /// 保存menu
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="pageArr"></param>
        /// <returns></returns>
        public bool SaveMenu(string roleName, List<string> pageArr)
        {
            try
            {
                var model = uow.MenuRepository.Head();
                var menu = model.menu;
                if (menu.IndexOf(roleName) == -1) { return true; }
                var newMenu = uow.MenuRepository.MenuOperation((JArray)JsonConvert.DeserializeObject(menu.Trim()), roleName, pageArr);
                uow.MenuRepository.Update(new Menu
                {
                    id = model.id,
                    menu = newMenu.ToString(),
                });
                uow.Commit();
                return true;
            }
            catch (Exception ex)
            {
                uow.Rollback();
                throw;
            }
        }
    }
}
