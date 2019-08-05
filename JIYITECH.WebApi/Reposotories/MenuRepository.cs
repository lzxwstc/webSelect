using Dapper;
using JIYITECH.WebApi.Configs;
using JIYITECH.WebApi.Entities;
using log4net;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JIYITECH.WebApi.Repositories
{
    public interface IMenuRepository : IBaseRepository<Menu>
    {

        /// <summary>
        /// roleName角色换名
        /// </summary>
        /// <param name="jObj">数据库菜单</param>
        /// <param name="oldName">旧名字</param>
        /// <param name="nowName">新名字</param>
        JArray ChangeName(JArray jObj, string oldName, string nowName);

        /// <summary>
        /// 修改 Menu 中角色对应菜单
        /// </summary>
        /// <param name="jObj">数据库菜单</param>
        /// <param name="roleName">角色名</param>
        /// <param name="pageArr">拥有菜单数组</param>
        /// <returns></returns>
        JArray MenuOperation(JArray jObj, string roleName, List<string> pageArr);
    }

    public class MenuRepository : BaseRepository<Menu>, IMenuRepository
    {
        private readonly ILog log;
        public MenuRepository(IDbTransaction transaction) : base(transaction)
        {
            log = LogManager.GetLogger(Startup.repository.Name, typeof(MenuRepository));
        }

        #region 操作menu
        /// <summary>
        /// roleName角色换名
        /// </summary>
        /// <param name="jObj">数据库菜单</param>
        /// <param name="oldName">旧名字</param>
        /// <param name="nowName">新名字</param>
        public JArray ChangeName(JArray jObj, string oldName, string nowName)
        {
            try
            {
                foreach (JObject item in jObj)
                {
                    if (item.ContainsKey("meta") && ((JObject)item["meta"]).ContainsKey("roles"))
                    {
                        var metaArr = JsonConvert.DeserializeObject<List<string>>(item["meta"]["roles"].ToString());
                        if (metaArr.Contains(oldName))
                        {
                            for (int j = 0; j < metaArr.Count(); j++)
                            {
                                if (metaArr[j] == oldName)
                                {
                                    item["meta"]["roles"][j] = nowName;
                                    break;
                                }
                            }
                        }
                        // 递归条件
                        if (item.ContainsKey("children"))
                        {
                            ChangeName((JArray)item["children"], oldName, nowName);
                        }
                    }
                }
                return jObj;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }

        /// <summary>
        /// 保存(增、删，改) Menu 中角色对应菜单
        /// </summary>
        /// <param name="jObj">数据库菜单</param>
        /// <param name="roleName">角色名</param>
        /// <param name="pageArr">拥有菜单数组</param>
        /// <returns></returns>
        public JArray MenuOperation(JArray jObj, string roleName, List<string> pageArr)
        {
            try
            {
                foreach (JObject item in jObj)
                {
                    if (item.ContainsKey("meta") && ((JObject)item["meta"]).ContainsKey("roles"))
                    {
                        var metaArr = JsonConvert.DeserializeObject<List<string>>(item["meta"]["roles"].ToString());
                        if (string.Join(',', pageArr.ToArray()).Contains(item["meta"]["title"].ToString()))
                        {
                            if (!metaArr.Contains(roleName))
                            {
                                item["meta"]["roles"][0].AddAfterSelf(roleName);
                            }
                        }
                        else
                        {
                            if (metaArr.Contains(roleName))
                            {
                                for (int j = 0; j < metaArr.Count(); j++)
                                {
                                    if (metaArr[j] == roleName)
                                    {
                                        item["meta"]["roles"][j].Remove();
                                        break;
                                    }
                                }
                            }
                        }
                        // 递归条件
                        if (item.ContainsKey("children"))
                        {
                            MenuOperation((JArray)item["children"], roleName, pageArr);
                        }
                    }
                }
                return jObj;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return null;
            }
        }
        #endregion
    }
}
