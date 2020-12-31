using FileServer.EntityDto.Minio;
using Peihui.Core.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileServer.IDomainServer.Minio
{
    /// <summary>
    /// 功能描述    ： 
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-11 16:50:57 
    /// 最后修改者  ：sh
    /// 最后修改日期：2020-12-11 16:50:57 
    /// </summary>
    public interface IMinioObjectDomain
    {
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="uploadFileDto"></param>
        /// <param name="uType">1:压缩上传 2：无压缩上传</param>
        /// <returns></returns>
        ResponseResult<FileDto> UploadFile(UserContext userContext, UploadFileDto uploadFileDto, int uType);
        /// <summary>
        /// 根据桶名获取所有文件
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        ResponseResult<List<ItemDto>> FindByBucket(UserContext userContext, string bucketName);
        /// <summary>
        /// 查看文件
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="findFileDto"></param>
        /// <returns></returns>
        ResponseResult<FileDto> FindFile(UserContext userContext, FindFileDto findFileDto);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="findFileDtos"></param>
        /// <returns></returns>
        ResponseResult BatchDelete(UserContext userContext, List<FindFileDto> findFileDtos);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="findFileDto"></param>
        /// <returns></returns>
        ResponseResult Delete(UserContext userContext, FindFileDto findFileDto);
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="findFileDto"></param>
        /// <returns></returns>
        ResponseResult<ItemDto> FindDetail(UserContext userContext, FindFileDto findFileDto);
        /// <summary>
        /// 替换文件
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="replaceFileDto"></param>
        /// <returns></returns>
        ResponseResult<FileDto> ReplaceFile(UserContext userContext, ReplaceFileDto replaceFileDto);
    }
}
