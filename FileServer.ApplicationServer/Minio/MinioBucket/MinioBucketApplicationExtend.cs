using FileServer.IApplicationServer;
using FileServer.IApplicationServer.Minio;
using FileServer.IDomainServer.Minio;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileServer.ApplicationServer.Minio.MinioBucket
{
    /// <summary>
    /// 功能描述    ：储存桶应用实现
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-14 14:56:46 
    /// </summary>
    public partial class MinioBucketApplication : IMinioBucketApplication
    {
        private readonly IMinioBucketDomain _minioBucketDomain;

        /// <summary>
        /// 登录拓展
        /// </summary>
        private readonly IEnableLoginApplication _enableLoginApplication;
        public MinioBucketApplication(IMinioBucketDomain minioBucketDomain,
            IEnableLoginApplication enableLoginApplication)
        {
            _minioBucketDomain = minioBucketDomain;

            _enableLoginApplication = enableLoginApplication;
        }
    }
}
