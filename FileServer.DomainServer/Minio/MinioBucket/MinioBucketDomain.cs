using FileServer.EntityDto.Minio;
using FileServer.IDomainServer.Minio;
using Minio.DataModel;
using Peihui.Core.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileServer.DomainServer.Minio.MinioBucket
{
    /// <summary>
    /// 功能描述    ：
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-14 15:02:27 
    /// </summary>
    public partial class MinioBucketDomain : IMinioBucketDomain
    {
        public ResponseResult BatchDelete(UserContext userContext, List<string> bucketNameList)
        {
            if (bucketNameList != null && bucketNameList.Count > 0)
            {
                foreach (var item in bucketNameList)
                {
                    _minioHelper.CheckBucketName(item);
                    _minioHelper.Delete(item);
                    _log.Info(userContext.Name + "删除了存储桶" + item);
                }
            }
            return ResponseResult.Success("删除成功");
        }

        public ResponseResult<BucketDto> CreateOne(UserContext userContext, string bucketName)
        {
            BucketDto bucketDto = _minioHelper.CreateBucket(bucketName).Result;
            if (bucketDto != null)
                return ResponseResult<BucketDto>.Success(bucketDto, bucketName + "新增成功");
            else
                return ResponseResult<BucketDto>.Error("新增失败");
        }

        public ResponseResult Delete(UserContext userContext, string bucketName)
        {
            _minioHelper.CheckBucketName(bucketName);
            _minioHelper.Delete(bucketName);
            _log.Info(userContext.Name + "删除了存储桶" + bucketName);
            return ResponseResult.Success("删除成功");
        }

        public ResponseResult IsExistBucket(UserContext userContext, string bucketName)
        {
            bool isExist = _minioHelper.CheckBucketExists(bucketName).Result;
            return ResponseResult.Success(bucketName + (isExist ? "" : "不") + "存在");
        }

        public ResponseResult<List<BucketDto>> Search(UserContext userContext)
        {
            List<BucketDto> bucketDtos = new List<BucketDto>();
            var result = _minioHelper.GetBucketList().Result;
            if (result != null && result.Count > 0)
            {
                bucketDtos = this.ChangeEntityDto(result);
            }
            return ResponseResult<List<BucketDto>>.Success(bucketDtos);
        }
    }
}
