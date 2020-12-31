using FileServer.EntityDto.Minio;
using Minio.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FileServer.Common.Minio
{
    /// <summary>
    /// 功能描述    ： 帮助接口
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-10 14:56:05 
    /// 最后修改者  ：sh
    /// 最后修改日期：2020-12-10 14:56:05 
    /// </summary>
    public interface IMinioHelper
    {
        #region 桶操作
        /// <summary>
        /// 检查桶是否存在
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task<bool> CheckBucketExists(string bucketName);
        /// <summary>
        /// 创建桶
        /// </summary>
        /// <param name="bucketName"></param>
        /// <returns></returns>
        Task<BucketDto> CreateBucket(string bucketName);
        /// <summary>
        /// 获取桶列表
        /// </summary>
        /// <returns></returns>
        Task<List<Bucket>> GetBucketList();
        /// <summary>
        /// 删除桶
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task Delete(string item);
        /// <summary>
        /// 检查桶名
        /// </summary>
        /// <param name="bucketName"></param>
        void CheckBucketName(string bucketName);

        List<Item> GetFilesByBucket(string bucketName);
        #endregion

        #region 文件操作
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="fileName"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        Task<FileDto> SaveFileObjectAsync(string bucketName, string fileName, Stream stream);
        /// <summary>
        /// 获取文件
        /// </summary>
        /// <param name="findFileDto"></param>
        /// <returns></returns>
        Task<FileDto> FindFileAsync(FindFileDto findFileDto);
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="findFileDto"></param>
        /// <returns></returns>
        Task RemoveFile(FindFileDto findFileDto);
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="bucketName"></param>
        /// <param name="objectsList"></param>
        /// <returns></returns>
        Task BacthRemoveFile(string bucketName, List<string> objectsList);
        /// <summary>
        /// 查看文件详情
        /// </summary>
        /// <param name="findFileDto"></param>
        /// <returns></returns>
        Task<ObjectStat> FindDetail(FindFileDto findFileDto);
        #endregion
    }
}
