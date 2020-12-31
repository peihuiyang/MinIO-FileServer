using FileServer.EntityDto.Minio;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileServer.DomainServer.Minio.MinioObject
{
    /// <summary>
    /// 功能描述    ：实体转换
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-16 10:18:21 
    /// </summary>
    public class MinioObjectConvert
    {
        /// <summary>
        /// 将替换实体转化为查询实体
        /// </summary>
        /// <param name="replaceFileDto"></param>
        /// <returns></returns>
        internal static FindFileDto ReplaceToFindFile(ReplaceFileDto replaceFileDto)
        {
            FindFileDto findFileDto = new FindFileDto
            {
                BucketName = replaceFileDto.OldBucket,
                FileName = replaceFileDto.OldFileName
            };
            return findFileDto;
        }
        /// <summary>
        /// 替换实体转化为上传实体
        /// </summary>
        /// <param name="replaceFileDto"></param>
        /// <returns></returns>
        internal static UploadFileDto UploadFileToFindFile(ReplaceFileDto replaceFileDto)
        {
            UploadFileDto uploadFileDto = new UploadFileDto
            {
                BucketName = replaceFileDto.NewBucket,
                FileName = replaceFileDto.NewFileName,
                TheStream = replaceFileDto.TheStream
            };
            return uploadFileDto;
        }
    }
}
