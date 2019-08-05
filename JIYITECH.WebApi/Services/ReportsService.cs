using JIYITECH.WebApi.Configs;
using JIYITECH.WebApi.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using IdentityModel;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace JIYITECH.WebApi.Services
{

    public interface IReportsService
    {
      
        /// <summary>
        /// 查全部名字
        /// </summary>
        /// <returns></returns>
        IEnumerable<Reports> SelName();

        IEnumerable<Reports> SelShip(long shipId);
        IEnumerable<Reports> SelFour(string str);
        int? InsertRep(JObject rep);

        int UpdateIsDefault(long id);
        int UpdateAll(JObject rep);

    }
    public class ReportsService : IReportsService
    {
        private readonly string tokenSecret;
        private readonly AppConfig appConfig;
        private readonly IUnitOfWork uow;
        public ReportsService(IOptions<AppConfig> appConfig, IUnitOfWork uow)
        {
            this.appConfig = appConfig.Value;
            tokenSecret = this.appConfig.tokenSecret;
            this.uow = uow;
        }

    
        //查询据船号全数据
        public IEnumerable<Reports> SelName() {
            return uow.ReportsRepository.GetModelList("where isDefault=0 order by shipping_list_id desc",null);
        }
        //按船号ID查询
        public IEnumerable<Reports> SelShip(long shipId)
        {
            return uow.ReportsRepository.GetModelList($"where shipping_list_id = '{shipId}' and isDefault = 0", null);
        }
        //按前4项查询
        public IEnumerable<Reports> SelFour(string str)
        {
            long longstr = 9999;
            if(Regex.IsMatch(str, @"^[+-]?\d*[.]?\d*$"))
            {
                longstr = long.Parse(str);

            }
            return uow.ReportsRepository.GetModelList($"where shipping_list_id like '%{longstr}%' or coalname like '%{str}%' or shiptime like '%{str}%' or shipname like '%{str}%' and isDefault = 0 order by shipping_list_id desc", null);
        }
        //插入数据
        public int? InsertRep(JObject rep)
        {
            Reports reptoin = rep.ToObject<Reports>();
            int? res = uow.ReportsRepository.Add(reptoin);
            uow.Commit();
            return res;
        }
        //修改isDefault的值
        public int UpdateIsDefault(long id)
        {
            int res= uow.ReportsRepository.UpdateZero(id, "true");
            uow.Commit();
            return res;
        }
        //修改全部的值
        public int UpdateAll(JObject rep)
        {
            Reports reports = rep.ToObject<Reports>();
            int res = uow.ReportsRepository.Update(reports);
            uow.Commit();
            return res;
        }
    }
}
