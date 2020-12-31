using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileServer.EntityDto.Minio
{
    /// <summary>
    /// 功能描述    ：上传文件传输实体
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-15 10:41:05 
    /// </summary>
    public class UploadFileDto
    {
        /// <summary>
        /// 包名
        /// </summary>
        public string BucketName { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件流
        /// </summary>
        public Stream TheStream { get; set; }
    }
}
