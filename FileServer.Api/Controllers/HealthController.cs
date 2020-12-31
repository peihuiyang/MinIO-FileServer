using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileServer.Api.Controllers
{
    /// <summary>
    /// 健康监控
    /// </summary>
    [Route("api/fs/health")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// 健康检查
        /// </summary>
        /// <returns></returns>
        [HttpGet("check")]
        public IActionResult Check()
        {
            return Ok();
        }

        /// <summary>
        /// 测试
        /// </summary>
        [HttpGet("test")]
        public void Test()
        {
            var ip = HttpContext.Connection.LocalIpAddress.ToString();
            Response.WriteAsync($"The IP is :{ip}, The CurTime is :{DateTime.Now:yyyy-MM-dd HH:mm:ss},From FileServer_HealthController.");
        }
    }
}
