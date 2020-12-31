using FileServer.EntityDto.Minio;
using FileServer.IApplicationServer.Minio;
using Peihui.Core.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileServer.ApplicationServer.Minio.MinioBucket
{
    /// <summary>
    /// 功能描述    ：储存桶应用实现
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-14 14:54:28 
    /// </summary>
    public partial class MinioBucketApplication : IMinioBucketApplication
    {
        public ResponseResult BatchDelete(string token, List<string> bucketNameList)
        {
            // =>校验登录
            UserContext userContext = _enableLoginApplication.GetUserContext(token);
            return _minioBucketDomain.BatchDelete(userContext, bucketNameList);
        }

        public ResponseResult<BucketDto> CreateOne(string token, string bucketName)
        {
            // =>校验登录
            UserContext userContext = _enableLoginApplication.GetUserContext(token);
            return _minioBucketDomain.CreateOne(userContext, bucketName);
        }

        public ResponseResult Delete(string token, string bucketName)
        {
            // =>校验登录
            UserContext userContext = _enableLoginApplication.GetUserContext(token);
            return _minioBucketDomain.Delete(userContext, bucketName);
        }

        public ResponseResult IsExistBucket(string token, string bucketName)
        {
            // =>校验登录
            UserContext userContext = _enableLoginApplication.GetUserContext(token);
            return _minioBucketDomain.IsExistBucket(userContext, bucketName);
        }

        public ResponseResult<List<BucketDto>> Search(string token)
        {
            // =>校验登录
            UserContext userContext = _enableLoginApplication.GetUserContext(token);
            return _minioBucketDomain.Search(userContext);
        }
    }
}
