using FileServer.EntityDto.Minio;
using FileServer.IApplicationServer;
using Peihui.Core.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileServer.ApplicationServer.Minio.MinioObject
{
    /// <summary>
    /// 功能描述    ：
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-11 16:17:34 
    /// </summary>
    public partial class MinioObjectApplication : IMinioObjectApplication
    {
        public ResponseResult<FileDto> UploadFile(string token, UploadFileDto uploadFileDto, int uType)
        {
            // =>校验登录
            UserContext userContext = _enableLoginApplication.GetUserContext(token);
            return _minioObjectDomain.UploadFile(userContext, uploadFileDto,uType);
        }
        public ResponseResult<List<ItemDto>> FindByBucket(string token, string bucketName)
        {
            // =>校验登录
            UserContext userContext = _enableLoginApplication.GetUserContext(token);
            return _minioObjectDomain.FindByBucket(userContext, bucketName);
        }
        public ResponseResult<FileDto> FindFile(string token, FindFileDto findFileDto)
        {
            // =>校验登录
            UserContext userContext = _enableLoginApplication.GetUserContext(token);
            return _minioObjectDomain.FindFile(userContext, findFileDto);
        }
        public ResponseResult BatchDelete(string token, List<FindFileDto> findFileDtos)
        {
            // =>校验登录
            UserContext userContext = _enableLoginApplication.GetUserContext(token);
            return _minioObjectDomain.BatchDelete(userContext, findFileDtos);
        }

        public ResponseResult Delete(string token, FindFileDto findFileDto)
        {
            // =>校验登录
            UserContext userContext = _enableLoginApplication.GetUserContext(token);
            return _minioObjectDomain.Delete(userContext, findFileDto);
        }

        public ResponseResult<ItemDto> FindDetail(string token, FindFileDto findFileDto)
        {
            // =>校验登录
            UserContext userContext = _enableLoginApplication.GetUserContext(token);
            return _minioObjectDomain.FindDetail(userContext, findFileDto);
        }

        public ResponseResult<FileDto> ReplaceFile(string token, ReplaceFileDto replaceFileDto)
        {
            // =>校验登录
            UserContext userContext = _enableLoginApplication.GetUserContext(token);
            return _minioObjectDomain.ReplaceFile(userContext, replaceFileDto);
        }
    }
}
