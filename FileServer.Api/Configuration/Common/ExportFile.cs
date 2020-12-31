using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FileServer.Api.Configuration.Common
{
    public class ExportFile
    {
        /// <summary>
        /// 生成文件
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static Stream CreateFile(byte[] buffer)
        {
            var memoryStream = new MemoryStream(buffer);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }
        /// <summary>
        /// 生成文件名
        /// </summary>
        /// <param name="extend">后缀</param>
        /// <returns></returns>
        public static string CreateFileName(string extend)
        {
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + extend;
            return fileName;
        }
    }
}
