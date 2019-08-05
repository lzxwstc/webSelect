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
    public class ReportsController : ControllerBase
    {
        private readonly IReportsService reportsService;
        public ReportsController(IReportsService reportsService)
        {
            this.reportsService = reportsService;
        }

        /// <summary>
        /// 全查
        /// </summary>
        [HttpGet, Route("[action]"), AllowAnonymous]
        public IActionResult SelAll()
        {
            return Ok(reportsService.SelName());
        }
        /// <summary>
        /// 按ID查询
        /// </summary>
        [HttpGet, Route("[action]"), AllowAnonymous]
        public IActionResult Sel(long shipId)
        {
            return Ok(reportsService.SelShip(shipId));
        }

        /// <summary>
        /// 按前4项查询
        /// </summary>
        [HttpGet, Route("[action]"), AllowAnonymous]
        public IActionResult SelByFour(string str)
        {
            return Ok(reportsService.SelFour(str));
        }

        /// <summary>
        /// 增加
        /// </summary>
        [HttpPut, Route("[action]"), AllowAnonymous]
        public IActionResult Ins([FromBody]JObject reports)
        {
            return Ok(reportsService.InsertRep(reports));
        }

        /// <summary>
        /// 删除
        /// </summary>
        [HttpPost, Route("[action]"), AllowAnonymous]
        public IActionResult Del(long id)
        {
            return Ok(reportsService.UpdateIsDefault(id));
        }

        /// <summary>
        /// 修改全部
        /// </summary>
        [HttpPost, Route("[action]"), AllowAnonymous]
        public IActionResult Update([FromBody]JObject reports)
        {
            return Ok(reportsService.UpdateAll(reports));
        }

    }
}
