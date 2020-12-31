using AutoMapper;
using FileServer.Common.File;
using FileServer.Common.Minio;
using FileServer.EntityDto.Minio;
using FileServer.IDomainServer.Minio;
using log4net;
using Minio.DataModel;
using Peihui.Core.CustomException;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileServer.DomainServer.Minio.MinioObject
{
    /// <summary>
    /// 功能描述    ：
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-11 16:56:20 
    /// </summary>
    public partial class MinioObjectDomain : IMinioObjectDomain
    {
        private readonly IMinioHelper _minioHelper;
        private readonly IMapper _mapper;
        private readonly ILog _log;
        public MinioObjectDomain(IMinioHelper minioHelper,IMapper mapper)
        {
            _minioHelper = minioHelper;
            _mapper = mapper;
            _log = LogManager.GetLogger(typeof(MinioObjectDomain));
        }

        private List<ItemDto> ChangeEntityDto(List<Item> result, string bucketName)
        {
            List<ItemDto> itemDtos = new List<ItemDto>();
            foreach (var item in result)
            {
                ItemDto itemDto = new ItemDto
                {
                    Name = item.Key,
                    BucketName = bucketName,
                    IsDir = item.IsDir,
                    LastModifiedDateTime = item.LastModifiedDateTime,
                    Size = item.Size,
                };
                itemDtos.Add(itemDto);
            }
            return itemDtos;
        }
        /// <summary>
        /// 修改文件名
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string ChangeFileName(string fileName)
        {
            if(string.IsNullOrWhiteSpace(fileName))
                throw new ExceptionHandle(new ExceptionEntity(400, fileName + "文件名不合法"));
            // 获取文件名后缀
            string extension = Path.GetExtension(fileName);
            // => 将文件名规范为guid+后缀
            string newName = Guid.NewGuid().ToString("N").ToLower() + extension;
            return newName;
        }
        /// <summary>
        /// 压缩文件
        /// </summary>
        /// <param name="uploadFileDto"></param>
        /// <returns></returns>
        private UploadFileDto CustCompressFile(UploadFileDto uploadFileDto)
        {
            // 获取文件名后缀
            string extension = Path.GetExtension(uploadFileDto.FileName).Replace(".", "").ToLower();
            // 识别是否为图片,图片大小大于100KB
            if(uploadFileDto.TheStream.Length > 100 * 1024 && Enum.IsDefined(typeof(ImageEnum), extension))
            {
                uploadFileDto.TheStream = CompressFile.CompressImage(uploadFileDto.TheStream);
                uploadFileDto.TheStream.Position = 0;
            }
            else if(uploadFileDto.TheStream.Length > 100 * 1024 && extension == "pdf")
            {
                uploadFileDto.TheStream = CompressFile.CompressPdf(uploadFileDto.TheStream);
            }
            return uploadFileDto;
        }

        /// <summary>
        /// 图片文件后缀
        /// </summary>
        enum ImageEnum
        {
            bmp, jpg, png, tif, gif, pcx, tga, exif, fpx, svg, psd, cdr, pcd, dxf, ufo, eps, ai, raw, WMF, webp, avif
        }
    }
}
