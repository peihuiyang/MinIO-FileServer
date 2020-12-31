using FileServer.EntityDto.Minio;
using FileServer.IApplicationServer;
using FileServer.IApplicationServer.File;
using FileServer.IDomainServer.File;
using Peihui.Core.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileServer.ApplicationServer.File
{
    /// <summary>
    /// 功能描述    ：
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-24 16:56:53 
    /// </summary>
    public class FileExtendApplication : IFileExtendApplication
    {
        private readonly IFileExtendDomain _fileExtendDomain;
        /// <summary>
        /// 登录拓展
        /// </summary>
        private readonly IEnableLoginApplication _enableLoginApplication;
        public FileExtendApplication(IFileExtendDomain fileExtendDomain, IEnableLoginApplication enableLoginApplication)
        {
            _fileExtendDomain = fileExtendDomain;
            _enableLoginApplication = enableLoginApplication;
        }
        public byte[] DeleteWatermark(string token, Stream stream, string watermark)
        {
            // =>校验登录
            UserContext userContext = _enableLoginApplication.GetUserContext(token);
            return _fileExtendDomain.DeleteWatermark(userContext, stream, watermark);
        }

        public ResponseResult<FileDto> ModifyILFileName(string token, UploadFileDto uploadFileDto)
        {
            // =>校验登录
            UserContext userContext = _enableLoginApplication.GetUserContext(token);
            return _fileExtendDomain.ModifyILFileName(userContext, uploadFileDto);
        }
    }
}
