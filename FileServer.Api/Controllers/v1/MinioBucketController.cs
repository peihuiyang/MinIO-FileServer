using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FileServer.EntityDto.Minio;
using FileServer.IApplicationServer.Minio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Peihui.Core.Response;

namespace FileServer.Api.Controllers.v1
{
    [Route("api/fs/v1/bucket")]
    [ApiController]
    public class MinioBucketController : ControllerBase
    {
        private readonly IMinioBucketApplication _minioBucketApplication;
        public MinioBucketController(IMinioBucketApplication minioBucketApplication)
        {
            _minioBucketApplication = minioBucketApplication;
        }
        /// <summary>
        /// 新增一个存储桶
        /// </summary>
        /// <param name="BucketName">存储桶名称</param>
        /// <returns></returns>
        [HttpGet("createone")]
        public ResponseResult<BucketDto> CreateOne(string BucketName)
        {
             string token = Request.Headers["Authorization"].ToString();
             return _minioBucketApplication.CreateOne(token, BucketName);
        }
        /// <summary>
        /// 验证指定桶是否存在
        /// </summary>
        /// <param name="BucketName">存储桶名称</param>
        /// <returns></returns>
        [HttpGet("isexist")]
        public ResponseResult IsExistBucket(string BucketName)
        {
            string token = Request.Headers["Authorization"].ToString();
            return _minioBucketApplication.IsExistBucket(token, BucketName);
        }
        /// <summary>
        /// 删除指定桶
        /// </summary>
        /// <param name="BucketName">存储桶名称</param>
        /// <returns></returns>
        [HttpDelete("deleteone")]
        public ResponseResult Delete(string BucketName)
        {
            string token = Request.Headers["Authorization"].ToString();
            return _minioBucketApplication.Delete(token, BucketName);
        }
        /// <summary>
        /// 批量删除指定桶
        /// </summary>
        /// <param name="BucketNameList">存储桶名称列表</param>
        /// <returns></returns>
        [HttpDelete("batchdelete")]
        public ResponseResult BatchDelete(List<string> BucketNameList)
        {
            string token = Request.Headers["Authorization"].ToString();
            return _minioBucketApplication.BatchDelete(token, BucketNameList);
        }
        /// <summary>
        /// 获取存储桶列表
        /// </summary>
        /// <returns></returns>
        [HttpPost("search")]
        public ResponseResult<List<BucketDto>> Search()
        {
            string token = Request.Headers["Authorization"].ToString();
            return _minioBucketApplication.Search(token);
        }
    }
}
