using FileServer.EntityDto.Minio;
using Peihui.Core.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileServer.IDomainServer.File
{
    /// <summary>
    /// 功能描述    ： 
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-24 17:14:15 
    /// 最后修改者  ：sh
    /// 最后修改日期：2020-12-24 17:14:15 
    /// </summary>
    public interface IFileExtendDomain
    {
        byte[] DeleteWatermark(UserContext userContext, Stream stream, string watermark);
        /// <summary>
        /// 修改征询函文件名称
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="uploadFileDto"></param>
        /// <returns></returns>
        ResponseResult<FileDto> ModifyILFileName(UserContext userContext, UploadFileDto uploadFileDto);
    }
}
