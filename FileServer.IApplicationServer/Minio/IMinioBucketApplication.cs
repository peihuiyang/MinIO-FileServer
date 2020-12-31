using FileServer.EntityDto.Minio;
using Peihui.Core.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileServer.IApplicationServer.Minio
{
    /// <summary>
    /// 功能描述    ：储存桶应用接口
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-14 14:22:41 
    /// 最后修改者  ：sh
    /// 最后修改日期：2020-12-14 14:22:41 
    /// </summary>
    public interface IMinioBucketApplication
    {
        /// <summary>
        /// 新增一个
        /// </summary>
        /// <param name="token"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        ResponseResult<BucketDto> CreateOne(string token, string bucketName);
        /// <summary>
        /// 检查是否存在
        /// </summary>
        /// <param name="token"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        ResponseResult IsExistBucket(string token, string bucketName);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="token"></param>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        ResponseResult Delete(string token, string bucketName);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="token"></param>
        /// <param name="bucketNameList"></param>
        /// <returns></returns>
        ResponseResult BatchDelete(string token, List<string> bucketNameList);
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        ResponseResult<List<BucketDto>> Search(string token);
    }
}
