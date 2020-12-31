using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileServer.EntityDto.Minio
{
    /// <summary>
    /// 功能描述    ：替换文件实体
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-16 10:07:47 
    /// </summary>
    public class ReplaceFileDto
    {
        /// <summary>
        /// Bucket名
        /// </summary>
        public string OldBucket { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string OldFileName { get; set; }

        /// <summary>
        /// Bucket名
        /// </summary>
        public string NewBucket { get; set; }

        /// <summary>
        /// 文件名（可不传）
        /// </summary>
        public string NewFileName { get; set; }

        /// <summary>
        /// 文件流
        /// </summary>
        public Stream TheStream { get; set; }
    }
}
