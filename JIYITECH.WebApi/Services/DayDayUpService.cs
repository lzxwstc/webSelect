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

namespace JIYITECH.WebApi.Services
{

    public interface IDayDayUpService
    {
        IEnumerable<DayDayUp> SelData(string dayName);
        int UpdateDayData(string dayName,long newData,int num);
        int? InsertDay(JObject day);
        int DeleteDay(List<long> id);
        /// <summary>
        /// 查全部名字
        /// </summary>
        /// <returns></returns>
        IEnumerable<DayDayUp> SelName();

    }
    public class DayDayUpService : IDayDayUpService
    {
        private readonly string tokenSecret;
        private readonly AppConfig appConfig;
        private readonly IUnitOfWork uow;
        public DayDayUpService(IOptions<AppConfig> appConfig, IUnitOfWork uow)
        {
            this.appConfig = appConfig.Value;
            tokenSecret = this.appConfig.tokenSecret;
            this.uow = uow;
        }

    

        public int? InsertDay(JObject day)
        {
            DayDayUp dayToIns = day.ToObject<DayDayUp>();
            int? res= uow.DayDayUpRepository.Ins(dayToIns);
            uow.Commit();
            return res;
        }

        public int DeleteDay(List<long> id)
        {
            int res = 0;
            foreach(long i in id) {
                uow.DayDayUpRepository.DelData(i);
                res++;
            }
            uow.Commit();
            return res;
        }

        public IEnumerable<DayDayUp> SelData(string dayName)
        {
            return uow.DayDayUpRepository.Sel(dayName);
        }

        public IEnumerable<DayDayUp> SelName() {
            return uow.DayDayUpRepository.SelAll();
        }


        public int UpdateDayData(string dayName,long newData,int num)
        {
            int res= uow.DayDayUpRepository.Update(dayName, newData,num);
            uow.Commit();
            return res;
        }
    }
}
