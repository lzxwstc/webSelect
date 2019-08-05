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
using System.Text;
using System.Threading.Tasks;

namespace JIYITECH.WebApi.Controllers
{
    public interface IDemoService
    {
        object GetSalt(string username);
        

    }
    internal class DemoService : IDemoService
    {
        private readonly IUnitOfWork uow;
        public DemoService(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public object GetSalt(string username)
        {
           Console.WriteLine(uow.UserRepository.GetSaltByUserName2(username));
            return uow.UserRepository.GetSaltByUserName2(username);
        }
    }
}
