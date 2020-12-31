using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace FileServer.Common.File
{
    /// <summary>
    /// 功能描述    ：
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-28 19:53:26 
    /// </summary>
    public class SpirePdfHelper
    {
        /// <summary>
        /// 获取PDF的文字
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static string GetPdfText(Stream stream,int p)
        {
            //实例化一个PdfDocument对象
            PdfDocument doc = new PdfDocument(stream);
            //实例化一个StringBuilder 对象
            StringBuilder content = new StringBuilder();
            if (p > 0)
            {
                content.Append(doc.Pages[p-1].ExtractText());
            }
            else
            {
                foreach (PdfPageBase page in doc.Pages)
                {
                    content.Append(page.ExtractText());
                }
            }
            return content.ToString();
        }

        public static byte[] GetPdfFirstPicture(Stream theStream, int p)
        {
            //实例化一个PdfDocument对象
            PdfDocument doc = new PdfDocument(theStream);
            // 获取 Spire.Pdf.PdfPageBase类对象
            PdfPageBase page = doc.Pages[p - 1];
            // 提取图片
            Image[] images = page.ExtractImages();
            if (images != null && images.Length > 0)
            {
                Image image = images[0];
                return PhotoImageInsert(image);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取图片并保存
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="p"></param>
        public static void SavePdfFirstPicture(Stream stream, int p)
        {
            // 实例化一个PdfDocument对象
            PdfDocument doc = new PdfDocument(stream);
            // 获取 Spire.Pdf.PdfPageBase类对象
            PdfPageBase page = doc.Pages[p-1];
            // 提取图片
            Image[] images = page.ExtractImages();
            if (images != null && images.Length > 0)
            {
                Image image = images[0];
                image.Save("image.png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }
        public static byte[] PhotoImageInsert(Image imgPhoto)
        {
            MemoryStream mstream = new MemoryStream();
            imgPhoto.Save(mstream, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] byData = new byte[mstream.Length];
            mstream.Position = 0;
            mstream.Read(byData, 0, byData.Length); mstream.Close();
            return byData;
        }
    }
}
