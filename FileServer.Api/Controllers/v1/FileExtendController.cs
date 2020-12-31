using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FileServer.Api.Configuration.Common;
using FileServer.EntityDto.Minio;
using FileServer.EntityDto.Watermark;
using FileServer.IApplicationServer.File;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Peihui.Core.CustomException;
using Peihui.Core.Response;

namespace FileServer.Api.Controllers.v1
{
    [Route("api/fs/v1/fileextend")]
    [ApiController]
    public class FileExtendController : ControllerBase
    {
        private readonly IFileExtendApplication _fileExtendApplication;
        public FileExtendController(IFileExtendApplication fileExtendApplication)
        {
            _fileExtendApplication = fileExtendApplication;
        }
        /// <summary>
        /// 测试--去文字水印(PDF)
        /// </summary>
        /// <returns></returns>
        [HttpPost("deletewatermark")]
        public IActionResult DeleteWatermark([FromForm(Name = "file")] IFormFile file)
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString();
                string watermark = "xxxxx";
                byte[] result = _fileExtendApplication.DeleteWatermark(token, file.OpenReadStream(), watermark);
                if (result == null)
                {
                    throw new ExceptionHandle(ExceptionEnums.DATA_IS_NOT_FOUND);
                }
                else if (result != null || result.Length > 0)
                {
                    Response.Headers.Add("Content-Disposition", "attachment; filename=" + file.FileName);
                    Response.Headers.Add("MessageInfo", "Success");
                    return new FileStreamResult(ExportFile.CreateFile(result), "application/octet-stream");
                }
                else
                {
                    return NotFound("文件导出失败或没有数据");
                }
            }
            catch (Exception ee)
            {
                throw new ExceptionHandle(new ExceptionEntity(400, ee.Message));
            }
        }
        /// <summary>
        /// 去文字水印(PDF)
        /// </summary>
        /// <param name="watermarkDto"></param>
        /// <returns></returns>
        [HttpPost("removewatermark")]
        public IActionResult RemoveWatermark([FromBody] RemoveWatermarkDto watermarkDto)
        {
            try
            {
                string token = Request.Headers["Authorization"].ToString();
                byte[] result = _fileExtendApplication.DeleteWatermark(token, watermarkDto.TheStream, watermarkDto.Watermark);
                if (result == null)
                {
                    throw new ExceptionHandle(ExceptionEnums.DATA_IS_NOT_FOUND);
                }
                else if (result != null || result.Length > 0)
                {
                    Response.Headers.Add("Content-Disposition", "attachment; filename=" + ExportFile.CreateFileName(".pdf"));
                    Response.Headers.Add("MessageInfo", "Success");
                    return new FileStreamResult(ExportFile.CreateFile(result), "application/octet-stream");
                }
                else
                {
                    return NotFound("文件导出失败或没有数据");
                }
            }
            catch (Exception ee)
            {
                ee.Message.ToString();
                throw new ExceptionHandle(new ExceptionEntity(400, "未登录或其他异常"));
            }
        }
        /// <summary>
        /// 修改文件名称
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("modifyfilename")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ResponseResult<FileDto> ModifyILFileName([FromForm(Name = "file")] IFormFile file)
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
                    return _fileExtendApplication.ModifyILFileName(token, uploadFileDto);
                }
            }
            catch (Exception ex)
            {
                return ResponseResult<FileDto>.Error(ex.Message);
            }
        }
    }
}
