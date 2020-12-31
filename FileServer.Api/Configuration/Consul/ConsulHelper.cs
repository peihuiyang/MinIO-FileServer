using Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileServer.Api.Configuration.Consul
{
    /// <summary>
    /// Consul帮助类
    /// </summary>
    public static class ConsulHelper
    {
        /// <summary>
        /// Consul注册
        /// </summary>
        /// <param name="configuration"></param>
        public static void ConsulRegister(this IConfiguration configuration)
        {
            string consulAddress = configuration["consul"];
            string ipAddress = configuration["apiadd"];
            int port = int.Parse(configuration["apiport"]);

            ConsulClient client = new ConsulClient(c =>
            {
                c.Address = new Uri(consulAddress);
                // 数据中心
                c.Datacenter = "dc1";
            });

            client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                // 服务标识
                ID = $"Api-{ipAddress}-{port}",
                // 服务分组【修改为对应】
                Name = "fileserverapi",
                Address = ipAddress,
                Port = port,
                Tags = null,
                Check = new AgentServiceCheck()
                {
                    // 每间隔12秒检测一次
                    Interval = TimeSpan.FromSeconds(12),
                    // 修改为对应的api地址
                    HTTP = $"http://{ipAddress}:{port}/api/fs/health/test",
                    // 5秒钟内没响应就超时，检测等待时间
                    Timeout = TimeSpan.FromSeconds(5),
                    // 60秒钟内还是不能成功就移除(最小60秒)
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(60)
                }
            });
        }
    }
}
