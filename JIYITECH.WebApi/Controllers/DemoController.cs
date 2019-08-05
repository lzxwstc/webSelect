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
    public class DemoController : ControllerBase
    {
        private readonly IDemoService demoService;
        public DemoController(IDemoService demoService)
        {
            this.demoService = demoService;
        }

        /// <summary>
        /// 获取Menu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("[action]")]
        [HttpGet, AllowAnonymous]
        public IActionResult GetSalt([FromQuery]string userName)
        {
            return Ok(demoService.GetSalt(userName));
        }
   
    }
}
