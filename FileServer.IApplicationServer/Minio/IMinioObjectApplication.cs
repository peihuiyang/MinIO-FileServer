using FileServer.EntityDto.Minio;
using Peihui.Core.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileServer.IApplicationServer
{
    /// <summary>
    /// 功能描述    ：文件应用服务接口
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-11 16:05:01 
    /// 最后修改者  ：sh
    /// 最后修改日期：2020-12-11 16:05:01 
    /// </summary>
    public interface IMinioObjectApplication
    {
        /// <summary>
        /// 根据桶名获取所有文件
        /// </summary>
        /// <param name="token"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        ResponseResult<List<ItemDto>> FindByBucket(string token, string bucketName);
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="token"></param>
        /// <param name="uploadFileDto"></param>
        /// <param name="uType">1:压缩上传 2：无压缩上传</param>
        /// <returns></returns>
        ResponseResult<FileDto> UploadFile(string token, UploadFileDto uploadFileDto, int uType);
        /// <summary>
        /// 查看文件
        /// </summary>
        /// <param name="token"></param>
        /// <param name="findFileDto"></param>
        /// <returns></returns>
        ResponseResult<FileDto> FindFile(string token, FindFileDto findFileDto);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="token"></param>
        /// <param name="findFileDto"></param>
        /// <returns></returns>
        ResponseResult Delete(string token, FindFileDto findFileDto);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="token"></param>
        /// <param name="findFileDtos"></param>
        /// <returns></returns>
        ResponseResult BatchDelete(string token, List<FindFileDto> findFileDtos);
        /// <summary>
        /// 查看详情
        /// </summary>
        /// <param name="token"></param>
        /// <param name="findFileDto"></param>
        /// <returns></returns>
        ResponseResult<ItemDto> FindDetail(string token, FindFileDto findFileDto);
        /// <summary>
        /// 替换文件
        /// </summary>
        /// <param name="token"></param>
        /// <param name="replaceFileDto"></param>
        /// <returns></returns>
        ResponseResult<FileDto> ReplaceFile(string token, ReplaceFileDto replaceFileDto);
    }
}
