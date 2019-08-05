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
    public interface IReportsRepository :IBaseRepository<Reports>
    {

     

        //int? Insert(Reports rep);
        int UpdateZero(long id, string isDefault);
    }
    public class ReportsRepository : BaseRepository<Reports>,IReportsRepository
    {

        private readonly ILog log;
        public ReportsRepository(IDbTransaction transaction) : base(transaction)
        {
            log = LogManager.GetLogger(Startup.repository.Name, typeof(ReportsRepository));
        }
       
        ////插入新数据
        //public int? Insert(Reports rep)
        //{
        //    int? data = db.Insert(rep, transaction: Transaction);
        //    return data;
        //}
        //删除数据（修改isDefault）
        public int UpdateZero(long id,string isDefault)
        {
            int data = db.Execute($"update reports set [isDefault] = '{isDefault}',[updateTime] = '{DateTime.Now}' where id = {id}", transaction: Transaction);
            return data;
        }
    }
}
