using System;
using System.Collections.Generic;
using System.Text;

namespace FileServer.EntityDto.Minio
{
    /// <summary>
    /// 功能描述    ：查询文件实体
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-15 11:24:10 
    /// </summary>
    public class FindFileDto
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
        /// 前缀
        /// </summary>
        public string Prefix { get; set; }
    }
}
