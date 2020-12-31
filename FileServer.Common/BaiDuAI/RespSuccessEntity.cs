using System;
using System.Collections.Generic;
using System.Text;

namespace FileServer.Common.BaiDuAI
{
    /// <summary>
    /// 功能描述    ：请求成功的实体
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-29 11:49:58 
    /// </summary>
    public class RespSuccessEntity
    {
         public string Refresh_token { get;set;}
         public long Expires_in { get;set;}
         public string Scope { get;set;}
         public string Session_key { get;set;}
         public string Access_token { get;set;}
         public string Session_secret { get;set;}
    }
    /// <summary>
    /// 请求错误的实体
    /// </summary>
    public class RespErrorEntity
    {
        public string Error { get; set; }
        public string Error_description { get; set; }
    }
   
}
