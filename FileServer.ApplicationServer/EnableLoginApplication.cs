using FileServer.IApplicationServer;
using Peihui.Common.Security.Authorization;
using Peihui.Core.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileServer.ApplicationServer
{
    /// <summary>
    /// 登录校验封装实现类
    /// </summary>
    public class EnableLoginApplication : IEnableLoginApplication
    {
        /// <summary>
        /// 登录校验服务
        /// </summary>
        private readonly ILoginCheckService _loginCheckService;
        public EnableLoginApplication(ILoginCheckService loginCheckService)
        {
            _loginCheckService = loginCheckService;
        }

        /// <summary>
        /// 获取UserContext
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public UserContext GetUserContext(string token)
        {
            UserContext userContext;
            if (EnableVerification.Verification)
            {
                // =>校验登录
                userContext = this._loginCheckService.LoginCheck(token);
            }
            else
            {
                userContext = new UserContext()
                {
                    BizId = "334ac32f6cb748f7810da62d8adc6543",
                    Code = "sadmin",
                    Name = "超级管理员",
                    Token = "77f1753bbbbd4366ada57d6988444db3"
                };
            }
            return userContext;
        }
    }
}
