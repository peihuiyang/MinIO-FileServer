using System;
using System.Collections.Generic;
using System.Text;

namespace FileServer.EntityDto.Minio
{
    /// <summary>
    /// 功能描述    ：存储桶传输实体
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-14 14:30:26 
    /// </summary>
    public class BucketDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        public string CreationDate { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationDateDateTime { get; set; }
    }
}
