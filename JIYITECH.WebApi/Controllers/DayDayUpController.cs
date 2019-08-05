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
    public class DayDayUpController : ControllerBase
    {
        private readonly IDayDayUpService dayDayUpService;
        public DayDayUpController(IDayDayUpService dayDayUpService)
        {
            this.dayDayUpService = dayDayUpService;
        }
        /// <summary>
        /// 查
        /// </summary>
        [HttpGet, Route("[action]"), AllowAnonymous]
        public IActionResult Sel(string dayName)
        {
            return Ok(dayDayUpService.SelData(dayName));
        }
        /// <summary>
        /// 全查
        /// </summary>
        [HttpGet, Route("[action]"), AllowAnonymous]
        public IActionResult SelAll()
        {
            return Ok(dayDayUpService.SelName());
        }

        /// <summary>
        /// 改
        /// </summary>
        [HttpPost, Route("[action]"), AllowAnonymous]
        public IActionResult UpdateData(string dayName,long newData,int num)
        {
            return Ok(dayDayUpService.UpdateDayData(dayName,newData,num));
        }
        /// <summary>
        /// 增
        /// </summary>
        [HttpPut, Route("[action]"), AllowAnonymous]
        public IActionResult Ins([FromBody]JObject day)
        {
            return Ok(dayDayUpService.InsertDay(day));
        }
        /// <summary>
        /// 删
        /// </summary>
        [HttpDelete("{dayName}"),AllowAnonymous]
        public IActionResult Delete(string dayName)
        {
            List<long> ids = new List<long>();
            IEnumerable<DayDayUp> message = dayDayUpService.SelData(dayName);
            foreach (DayDayUp d in message.ToList()) {
                ids.Add(d.id);
            }
            return Ok(dayDayUpService.DeleteDay(ids));
        }
    }
}
