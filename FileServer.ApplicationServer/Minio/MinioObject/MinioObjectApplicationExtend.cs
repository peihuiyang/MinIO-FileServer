using FileServer.EntityDto.Minio;
using FileServer.IApplicationServer;
using FileServer.IDomainServer.Minio;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileServer.ApplicationServer.Minio.MinioObject
{
    /// <summary>
    /// 功能描述    ：
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-11 16:18:43 
    /// </summary>
    public partial class MinioObjectApplication : IMinioObjectApplication
    {
        private readonly IMinioObjectDomain _minioObjectDomain;
        
        /// <summary>
        /// 登录拓展
        /// </summary>
        private readonly IEnableLoginApplication _enableLoginApplication;
        public MinioObjectApplication(IMinioObjectDomain minioObjectDomain,
            IEnableLoginApplication enableLoginApplication)
        {
            _minioObjectDomain = minioObjectDomain;

            _enableLoginApplication = enableLoginApplication;
        }
    }
}
