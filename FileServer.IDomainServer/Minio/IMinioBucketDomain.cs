using FileServer.EntityDto.Minio;
using Peihui.Core.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileServer.IDomainServer.Minio
{
    /// <summary>
    /// 功能描述    ：文件服务接口
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-14 14:58:09 
    /// 最后修改者  ：sh
    /// 最后修改日期：2020-12-14 14:58:09 
    /// </summary>
    public interface IMinioBucketDomain
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        ResponseResult<BucketDto> CreateOne(UserContext userContext, string bucketName);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="bucketNameList"></param>
        /// <returns></returns>
        ResponseResult BatchDelete(UserContext userContext, List<string> bucketNameList);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        ResponseResult Delete(UserContext userContext, string bucketName);
        /// <summary>
        /// 检查是否存在
        /// </summary>
        /// <param name="userContext"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        ResponseResult IsExistBucket(UserContext userContext, string bucketName);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="userContext"></param>
        /// <returns></returns>
        ResponseResult<List<BucketDto>> Search(UserContext userContext);
    }
}
