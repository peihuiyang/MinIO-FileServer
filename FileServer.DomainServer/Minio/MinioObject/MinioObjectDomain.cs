using FileServer.EntityDto.Minio;
using FileServer.IDomainServer.Minio;
using Minio.DataModel;
using Peihui.Core.CustomException;
using Peihui.Core.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileServer.DomainServer.Minio.MinioObject
{
    /// <summary>
    /// 功能描述    ：
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-11 16:55:48 
    /// </summary>
    public partial class MinioObjectDomain : IMinioObjectDomain
    {
        public ResponseResult BatchDelete(UserContext userContext, List<FindFileDto> findFileDtos)
        {
            // => 筛选出储存桶
            List<string> BucketList = findFileDtos.Select(l => l.BucketName).Distinct().ToList();
            if (BucketList != null && BucketList.Count > 0)
            {
                foreach (var item in BucketList)
                {
                    // => 筛选出文件名
                    List<string> FileList = findFileDtos.Where(l => l.BucketName == item).Select(l => l.FileName).Distinct().ToList();
                    // => 批量删除
                    _minioHelper.BacthRemoveFile(item, FileList);
                    // => 记录日志
                    foreach (var file in FileList)
                    {
                        _log.Info(string.Format("{0}删除了位于{1}的文件{2}", userContext.Name, item, file));
                    }
                }
                return ResponseResult.Success("批量删除成功");
            }
            else
            {
                return ResponseResult.Error("没有传入文件夹名称，操作中止");
            }
        }

        public ResponseResult Delete(UserContext userContext, FindFileDto findFileDto)
        {
            var result = _minioHelper.FindDetail(findFileDto).Result;
            if (result != null)
            {
                _minioHelper.RemoveFile(findFileDto);
                _log.Info(string.Format("{0}删除了位于{1}的文件{2}", userContext.Name, findFileDto.BucketName, findFileDto.FileName));
            }
            return ResponseResult.Success(string.Format("已删除了位于{0}的文件{1}", findFileDto.BucketName, findFileDto.FileName));
        }

        public ResponseResult<List<ItemDto>> FindByBucket(UserContext userContext, string bucketName)
        {
            var result = _minioHelper.GetFilesByBucket(bucketName);
            List<ItemDto> resultdto = this.ChangeEntityDto(result, bucketName);
            return ResponseResult<List<ItemDto>>.Success(resultdto, "数据获取成功");
        }

        public ResponseResult<ItemDto> FindDetail(UserContext userContext, FindFileDto findFileDto)
        {
            var result = _minioHelper.FindDetail(findFileDto).Result;
            if (result != null)
            {
                ItemDto itemDto = new ItemDto
                {
                    Name = result.ObjectName,
                    BucketName = findFileDto.BucketName,
                    Size = (ulong)result.Size,
                    IsDir = false,
                    LastModifiedDateTime = result.LastModified,
                    ContentType = result.ContentType
                };
                return ResponseResult<ItemDto>.Success(itemDto, "数据获取成功");
            }
            else
                return ResponseResult<ItemDto>.Error("数据获取失败");
        }

        public ResponseResult<FileDto> FindFile(UserContext userContext, FindFileDto findFileDto)
        {
            FileDto fileDto = _minioHelper.FindFileAsync(findFileDto).Result;
            if(fileDto==null)
                return ResponseResult<FileDto>.Error("文件读取失败或者不存在");
            return ResponseResult<FileDto>.Success(fileDto, "数据获取成功");
        }

        public ResponseResult<FileDto> ReplaceFile(UserContext userContext, ReplaceFileDto replaceFileDto)
        {
            // => 上传新的（先执行避免删除完上传失败）
            UploadFileDto uploadFileDto = MinioObjectConvert.UploadFileToFindFile(replaceFileDto);
            ResponseResult<FileDto> responseResult = this.UploadFile(userContext, uploadFileDto,1);

            if (responseResult != null && responseResult.Status == 1)
            {
                // => 删除旧的
                FindFileDto findFileDto = MinioObjectConvert.ReplaceToFindFile(replaceFileDto);
                this.Delete(userContext, findFileDto);
            }
            // => 返回
            return responseResult;
        }

        public ResponseResult<FileDto> UploadFile(UserContext userContext, UploadFileDto uploadFileDto, int uType)
        {
            try
            {
                // => 修改文件名称
                uploadFileDto.FileName = this.ChangeFileName(uploadFileDto.FileName);
                if(uType == 1)
                {
                    // => 修改文件大小
                    uploadFileDto = this.CustCompressFile(uploadFileDto);
                }
                
                // => 上传文件
                var fileDto = _minioHelper.SaveFileObjectAsync(uploadFileDto.BucketName, uploadFileDto.FileName, uploadFileDto.TheStream).Result;
                if (fileDto != null)
                {
                    _log.Info(string.Format("{0}上传了文件{1}到{2}", userContext.Name, fileDto.FileName, fileDto.Bucket));
                    return ResponseResult<FileDto>.Success(fileDto, "文件上传成功");
                }
                else
                    return ResponseResult<FileDto>.Error(fileDto, "文件上传失败");
            }
            catch (ExceptionHandle ee)
            {
                return ResponseResult<FileDto>.Error(uploadFileDto.FileName + "上传失败，失败原因：" + ee.ErrorMsg);
            }
            catch (Exception ee)
            {
                return ResponseResult<FileDto>.Error(uploadFileDto.FileName + "上传失败，失败原因：" + ee.Message);
            }
        }
    }
}
