using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileServer.Common.File;
using FileServer.EntityDto.Minio;
using FileServer.IApplicationServer;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Peihui.Core.CustomException;
using Peihui.Core.Response;

namespace FileServer.Api.Controllers.v1
{
    [Route("api/fs/v1/file")]
    [ApiController]
    public class MinioObjectController : ControllerBase
    {
        private readonly IMinioObjectApplication _minioObjectApplication;
        public MinioObjectController(IMinioObjectApplication minioObjectApplication)
        {
            _minioObjectApplication = minioObjectApplication;
        }
        /// <summary>
        /// 测试--文件上传
        /// </summary>
        /// <returns></returns>
        [HttpPost("savefile")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ResponseResult<FileDto> SaveFile([FromForm(Name = "file")] IFormFile file)
        {
            string token = Request.Headers["Authorization"].ToString();
            try
            {
                if (file == null)
                {
                    return ResponseResult<FileDto>.Error("获取不到提交的文件");
                }
                else
                {
                    UploadFileDto uploadFileDto = new UploadFileDto
                    {
                        BucketName = "temp",
                        FileName = file.FileName,
                        TheStream = file.OpenReadStream()
                    };
                    return _minioObjectApplication.UploadFile(token, uploadFileDto,1);
                }
            }
            catch (Exception ex)
            {
                return ResponseResult<FileDto>.Error(ex.Message);
            }
        }

        /// <summary>
        /// 根据桶名获取所有文件
        /// </summary>
        /// <param name="BucketName">桶名</param>
        /// <returns></returns>
        [HttpGet("findbybucket")]
        public ResponseResult<List<ItemDto>> FindByBucket(string BucketName)
        {
            string token = Request.Headers["Authorization"].ToString();
            return _minioObjectApplication.FindByBucket(token,BucketName);
        }
        /// <summary>
        /// 查看单个文件详情
        /// </summary>
        /// <returns></returns>
        [HttpPost("finddetail")]
        public ResponseResult<ItemDto> FindDetail([FromBody] FindFileDto findFileDto)
        {
            string token = Request.Headers["Authorization"].ToString();
            return _minioObjectApplication.FindDetail(token, findFileDto);
        }
        /// <summary>
        /// 文件上传(压缩)
        /// </summary>
        /// <returns></returns>
        [HttpPost("uploadfile")]
        public ResponseResult<FileDto> UploadFile([FromBody] UploadFileDto uploadFileDto)
        {
            string token = Request.Headers["Authorization"].ToString();
            return _minioObjectApplication.UploadFile(token, uploadFileDto,1);
        }
        /// <summary>
        /// 文件上传(无压缩)
        /// </summary>
        /// <returns></returns>
        [HttpPost("uploadcfile")]
        public ResponseResult<FileDto> UploadCFile([FromBody] UploadFileDto uploadFileDto)
        {
            string token = Request.Headers["Authorization"].ToString();
            return _minioObjectApplication.UploadFile(token, uploadFileDto,2);
        }
        /// <summary>
        /// 替换文件
        /// </summary>
        /// <returns></returns>
        [HttpPost("replacefile")]
        public ResponseResult<FileDto> ReplaceFile([FromBody] ReplaceFileDto replaceFileDto)
        {
            string token = Request.Headers["Authorization"].ToString();
            return _minioObjectApplication.ReplaceFile(token, replaceFileDto);
        }
        /// <summary>
        /// 查看文件（含数据）
        /// </summary>
        /// <returns></returns>
        [HttpPost("findfile")]
        public ResponseResult<FileDto> FindFile([FromBody] FindFileDto findFileDto)
        {
            string token = Request.Headers["Authorization"].ToString();
            return _minioObjectApplication.FindFile(token, findFileDto);
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <returns></returns>
        [HttpPost("download")]
        public IActionResult DownloadFile([FromBody] FindFileDto findFileDto)
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString();
                ResponseResult<FileDto> responseResult = _minioObjectApplication.FindFile(token, findFileDto);
                byte[] buffer = responseResult.Data.TheStream;
                if (buffer == null)
                {
                    throw new ExceptionHandle(ExceptionEnums.DATA_IS_NOT_FOUND);
                }
                else if (buffer != null || buffer.Length > 0)
                {
                    Response.Headers.Add("Content-Disposition", "attachment; filename=" + responseResult.Data.FileName);
                    Response.Headers.Add("MessageInfo", "Success");
                    return new FileStreamResult(FileOptExtend.BytesToStream(buffer), "application/octet-stream");
                }
                else
                {
                    return NotFound("文件导出失败或没有数据");
                }
            }
            catch (ExceptionHandle ee)
            {
                return BadRequest(ee.ErrorMsg);
            }
            catch (Exception ee)
            {
                return BadRequest(ee.Message);
            }
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete")]
        public ResponseResult Delete([FromBody] FindFileDto findFileDto)
        {
            string token = Request.Headers["Authorization"].ToString();
            return _minioObjectApplication.Delete(token, findFileDto);
        }
        /// <summary>
        /// 批量删除文件
        /// </summary>
        /// <returns></returns>
        [HttpDelete("batchdelete")]
        public ResponseResult BatchDelete([FromBody] List<FindFileDto> findFileDtos)
        {
            string token = Request.Headers["Authorization"].ToString();
            return _minioObjectApplication.BatchDelete(token, findFileDtos);
        }
    }
}
