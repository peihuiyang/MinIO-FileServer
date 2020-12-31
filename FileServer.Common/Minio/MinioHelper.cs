using FileServer.Common.File;
using FileServer.EntityDto.Minio;
using log4net;
using Minio;
using Minio.DataModel;
using Minio.Exceptions;
using Peihui.Core.Config;
using Peihui.Core.CustomException;
using Peihui.Core.EnDecrypt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileServer.Common.Minio
{
    /// <summary>
    /// 功能描述    ：Minio文件服务帮助类
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-08 14:30:38 
    /// </summary>
    public class MinioHelper : IMinioHelper
    {
        private readonly ILog _log;
        public string endpoint;
        public string accessKey;
        public string secretKey;

        public MinioHelper()
        {
            _log = LogManager.GetLogger(typeof(MinioHelper));
            endpoint = JsonConfigHelper.Configuration["FileServer:Endpoint"];
            accessKey = AesHelper.Decrypt(JsonConfigHelper.Configuration["FileServer:AccessKey"]);
            secretKey = AesHelper.Decrypt(JsonConfigHelper.Configuration["FileServer:SecretKey"]);
        }
        #region 存储桶
        public async Task<bool> CheckBucketExists(string bucketName)
        {
                
            var minio = new MinioClient(endpoint, accessKey, secretKey);
            bool found = await minio.BucketExistsAsync(bucketName);
            return found;
        }

        public void CheckBucketName(string bucketName)
        {
            if (string.IsNullOrWhiteSpace(bucketName))
                throw new ExceptionHandle(new ExceptionEntity(400, "文件夹名不能为空"));
            // => 检查桶是否存在
            bool found = this.CheckBucketExists(bucketName).Result;
            if (!found)
                throw new ExceptionHandle(new ExceptionEntity(400, bucketName + "文件夹不存在"));
        }

        public async Task<BucketDto> CreateBucket(string bucketName)
        {
            try
            {
                var minio = new MinioClient(endpoint, accessKey, secretKey);

                bool found = await minio.BucketExistsAsync(bucketName);
                if (found)
                {
                    // 返回提示信息
                    throw new ExceptionHandle(new ExceptionEntity(400, bucketName + "已存在"));
                }
                else
                {
                    await minio.MakeBucketAsync(bucketName);
                    BucketDto bucketDto = new BucketDto
                    {
                        Name = bucketName,
                        CreationDate = DateTime.Now.ToLongDateString(),
                        CreationDateDateTime = DateTime.Now
                    };
                    return bucketDto;
                }
            }
            catch (MinioException e)
            {
                _log.Error("存储桶创建失败原因："+ e.Message);
                return null;
            }
        }

        public async Task Delete(string item)
        {
            try
            {
                var minio = new MinioClient(endpoint, accessKey, secretKey);

                this.CheckBucketName(item);
                await minio.RemoveBucketAsync(item);
            }
            catch (MinioException e)
            {
                _log.Error("删除桶"+ item + "失败原因：" + e.Message);
            }
        }

        public async Task<List<Bucket>> GetBucketList()
        {
            try
            {
                var minio = new MinioClient(endpoint, accessKey, secretKey);
                var list = await minio.ListBucketsAsync();
                return list.Buckets;
            }
            catch (MinioException e)
            {
                _log.Error("获取存储桶列表错误原因：" + e.Message);
                return null;
            }
        }
        public List<Item> GetFilesByBucket(string bucketName)
        {
            try
            {
                List<Item> items = new List<Item>();
                var minio = new MinioClient(endpoint, accessKey, secretKey);
                // 校验桶名
                this.CheckBucketName(bucketName);
                
                // 获取文件列表
                IObservable<Item> observable = minio.ListObjectsAsync(bucketName);
                IDisposable subscription = observable.Subscribe(
                            item => items.Add(item),
                            ex => _log.Error(string.Format("文件读取错误: {0}", ex.Message)));
                Thread.Sleep(500);
                return items;
            }
            catch (MinioException e)
            {
                _log.Error("获取桶"+ bucketName + "文件列表错误原因：" + e.Message);
                return null;
            }
        }
        #endregion

        #region 文件对象操作
        public async Task<FileDto> SaveFileObjectAsync(string bucketName,string fileName, Stream stream)
        {
            try
            {
                var minio = new MinioClient(endpoint, accessKey, secretKey);
                // => 检查桶是否存在
                this.CheckBucketName(bucketName);       
                //Aes aesEncryption = Aes.Create();
                //aesEncryption.KeySize = 256;
                //aesEncryption.GenerateKey();
                //var ssec = new SSEC(aesEncryption.Key);
                await minio.PutObjectAsync(bucketName, fileName,stream,stream.Length, contentType: "application / octet - stream");
                FileDto fileDto = new FileDto
                {
                    Bucket = bucketName,
                    FileName = fileName,
                };
                return fileDto;
            }
            catch (MinioException e)
            {
                _log.Error(fileName + "上传错误原因：" + e.Message);
                return null;
            }
        }

        public async Task<FileDto> FindFileAsync(FindFileDto findFileDto)
        {
            try
            {
                FileDto fileDto = new FileDto
                {
                    Bucket = findFileDto.BucketName,
                    FileName = findFileDto.FileName
                };
                Stream temp_stream = new MemoryStream();
                var minio = new MinioClient(endpoint, accessKey, secretKey);
                // => 检查桶是否存在
                this.CheckBucketName(findFileDto.BucketName);
                // 获取文件信息
                // await minio.StatObjectAsync(bucketName, fileName);
                // 获取文件流
                await minio.GetObjectAsync(findFileDto.BucketName, findFileDto.FileName,
                                    (stream) =>
                                    {
                                        stream.CopyTo(temp_stream);
                                    });
                // => 将stream转化为byte[]
                fileDto.TheStream = FileOptExtend.StreamToBytes(temp_stream);
                return fileDto; 
            }
            catch (MinioException e)
            {
                _log.Error(findFileDto.FileName + "读取错误原因：" + e.Message);
                return null;
            }
        }

        public async Task RemoveFile(FindFileDto findFileDto)
        {
            try
            {
                var minio = new MinioClient(endpoint, accessKey, secretKey);
                // => 检查桶是否存在
                this.CheckBucketName(findFileDto.BucketName);
                await minio.RemoveObjectAsync(findFileDto.BucketName, findFileDto.FileName);
            }
            catch (MinioException e)
            {
                _log.Error(findFileDto.FileName + "删除错误原因：" + e.Message);
            }
        }

        public async Task BacthRemoveFile(string bucketName,List<string> objectsList)
        {
            try
            {
                var minio = new MinioClient(endpoint, accessKey, secretKey);
                // => 检查桶是否存在
                this.CheckBucketName(bucketName);
                IObservable<DeleteError> observable = await minio.RemoveObjectAsync(bucketName, objectsList);
                IDisposable subscription = observable.Subscribe(
                    deleteError => _log.Error(string.Format("未删除文件: {0}", deleteError.Key)),
                    ex => _log.Error(string.Format("错误原因: {0}", ex)));
            }
            catch (MinioException e)
            {
                _log.Error("批量删除错误原因：" + e.Message);
            }
        }

        public async Task<ObjectStat> FindDetail(FindFileDto findFileDto)
        {
            try
            {
                var minio = new MinioClient(endpoint, accessKey, secretKey);
                // => 检查桶是否存在
                this.CheckBucketName(findFileDto.BucketName);
                ObjectStat objectStat = await minio.StatObjectAsync(findFileDto.BucketName, findFileDto.FileName);
                return objectStat;
            }
            catch (MinioException e)
            {
                _log.Error(findFileDto.FileName + "查询错误原因：" + e.Message);
                return null;
            }
        }
        #endregion
    }
}
