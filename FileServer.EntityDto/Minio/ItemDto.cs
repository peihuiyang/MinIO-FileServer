using System;
using System.Collections.Generic;
using System.Text;

namespace FileServer.EntityDto.Minio
{
    /// <summary>
    /// 功能描述    ：文件详情实体
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-15 09:16:23 
    /// </summary>
    public class ItemDto
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 存储桶名
        /// </summary>
        public string BucketName { get; set; }
        /// <summary>
        /// 大小
        /// </summary>
        public ulong Size { get; set; }
        /// <summary>
        /// 是否文件夹
        /// </summary>
        public bool IsDir { get; set; }
        /// <summary>
        /// 最后修改时间
        /// </summary>
        public DateTime? LastModifiedDateTime { get; set; }
        /// <summary>
        /// 文件内容类型
        /// </summary>
        public string ContentType { get; set; }
    }
}
