using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileServer.EntityDto.Watermark
{
    /// <summary>
    /// 功能描述    ：
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-31 15:08:52 
    /// </summary>
    public class RemoveWatermarkDto
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string Watermark { get; set; }
        /// <summary>
        /// 文件流
        /// </summary>
        public Stream TheStream { get; set; }
    }
}
