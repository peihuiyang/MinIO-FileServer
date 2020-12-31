using FileServer.Common.BaiDuAI;
using FileServer.Common.File;
using FileServer.EntityDto.Minio;
using FileServer.IDomainServer.File;
using Newtonsoft.Json;
using Peihui.Core.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace FileServer.DomainServer.File
{
    /// <summary>
    /// 功能描述    ：
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-24 17:17:32 
    /// </summary>
    public class FileExtendDomain : IFileExtendDomain
    {
        public byte[] DeleteWatermark(UserContext userContext, Stream stream, string watermark)
        {
            byte[] temp = FileOptExtend.StreamToBytes(stream);
            MemoryStream memoryStream = new MemoryStream(temp);
            Stream stream1 =  CompressFile.DelWatermark(memoryStream, watermark);
            byte[] result = FileOptExtend.StreamToBytes(stream1);
            return result;
        }

        public ResponseResult<FileDto> ModifyILFileName(UserContext userContext, UploadFileDto uploadFileDto)
        {
            string tempContent = null;
            RespWordEntity<WordEntity> respWordEntity = null;
            // => 提取文档里第一张图片
            byte[] byData =  SpirePdfHelper.GetPdfFirstPicture(uploadFileDto.TheStream,1);
            // 识别图中的文字
            if(byData!=null&& byData.Length > 0)
            {
                tempContent = PictureService.WebImage(byData);
                if (tempContent != null)
                {
                    respWordEntity = JsonConvert.DeserializeObject<RespWordEntity<WordEntity>>(tempContent);
                }
            }
            // 过滤文字
            if (respWordEntity!=null&&respWordEntity.Words_result.Count > 0)
            {
                
            }

            return ResponseResult<FileDto>.Success(null,"名称修改成功");
        }
    }
}
