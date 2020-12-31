using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileServer.EntityDto.Minio
{
    /// <summary>
    /// 功能描述    ：文件信息（带数据）
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-11 17:30:26 
    /// </summary>
    public class FileDto
    {
        /// <summary>
        /// Bucket名
        /// </summary>
        public string Bucket { get; set; }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FileUrl { get; set; }
        /// <summary>
        /// Base64
        /// </summary>
        public string Base64String { get; set; }
        /// <summary>
        /// 文件流
        /// </summary>
        public byte[] TheStream { get; set; }
    }
}
