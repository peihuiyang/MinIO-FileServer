using FileServer.EntityDto.Minio;
using Peihui.Core.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileServer.IApplicationServer.File
{
    /// <summary>
    /// 功能描述    ： 
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-24 16:57:25 
    /// 最后修改者  ：sh
    /// 最后修改日期：2020-12-24 16:57:25 
    /// </summary>
    public interface IFileExtendApplication
    {
        byte[] DeleteWatermark(string token, Stream stream, string watermark);
        /// <summary>
        /// 修改征询函文件名称
        /// </summary>
        /// <param name="token"></param>
        /// <param name="uploadFileDto"></param>
        /// <returns></returns>
        ResponseResult<FileDto> ModifyILFileName(string token, UploadFileDto uploadFileDto);
    }
}
