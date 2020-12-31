using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileServer.Common.File
{
    /// <summary>
    /// 功能描述    ：
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-28 17:35:11 
    /// </summary>
    public class ITextSharpHelper
    {
        public static string GetPdfText(Stream stream, int p)
        {
            string content = null;
            PdfReader reader = new PdfReader(stream);
            try
            {
                // => 删除无法访问的对象
                reader.RemoveUnusedObjects();
                if (p > 0)
                {
                    byte[] bufferOfPageContent = reader.GetPageContent(p);
                    content = Encoding.ASCII.GetString(bufferOfPageContent);
                }
                else
                {
                    // 获取文档页数
                    int pageNum = reader.NumberOfPages;
                    for (int i = 1; i <= pageNum; i++)
                    {
                        byte[] bufferOfPageContent = reader.GetPageContent(p);
                        content += Encoding.ASCII.GetString(bufferOfPageContent);
                    }
                }
                return content;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return null;
            }
            finally
            {
                reader.Close();
            }
        }
    }
}
