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
    public interface IDayDayUpRepository
    {

        IEnumerable<DayDayUp> Sel(string dayName);
        int DelData(long id);
        int Update(string DayName, long NewData,int num);
        int? Ins(DayDayUp day);
        ///²éÑ¯È«Ãû×Ö
        IEnumerable<DayDayUp> SelAll();
    }
    public class DayDayUpRepository : BaseRepository<DayDayUp>,IDayDayUpRepository
    {

        private readonly ILog log;
        public DayDayUpRepository(IDbTransaction transaction) : base(transaction)
        {
            log = LogManager.GetLogger(Startup.repository.Name, typeof(DayDayUpRepository));
        }


        public int DelData(long id)
        {
            db.Execute($"delete [smartcoal].[dbo].[up] where id = {id}", null, transaction: Transaction);
            int data = db.Delete<DayDayUp>(id, transaction: Transaction);
            return data;
        }

        public int? Ins(DayDayUp day)
        {
            int? data = db.Insert(day, transaction: Transaction);
            return data;
        }

        public IEnumerable<DayDayUp> Sel(string dayName)
        {
            IEnumerable<DayDayUp> data = db.Query<DayDayUp>($"select * from [smartcoal].[dbo].[up] where DayName = '{dayName}'", transaction: Transaction).ToList();
            return data;
        }

        public int Update(string DayName, long NewData,int num)
        {
            int data = db.Execute($"update up set [DayData{num}] = {NewData},[updateTime] = '{DateTime.Now}' where DayName = '{DayName}'", transaction: Transaction);
            return data;
        }

        public IEnumerable<DayDayUp> SelAll() {
            IEnumerable<DayDayUp> data = db.Query<DayDayUp>("select * from [smartcoal].[dbo].[up]", transaction: Transaction);
            return data;
        }
    }
}
