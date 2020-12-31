using Peihui.Core.QueryEntity.MongoDB;
using Peihui.Core.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileServer.IApplicationServer
{
    /// <summary>
    /// 功能描述    ： 
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-11 16:36:04 
    /// 最后修改者  ：sh
    /// 最后修改日期：2020-12-11 16:36:04 
    /// </summary>
    public interface IEnableLoginApplication
    {
        /// <summary>
        /// 获取UserContext
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        UserContext GetUserContext(string token);
       
    }
}
