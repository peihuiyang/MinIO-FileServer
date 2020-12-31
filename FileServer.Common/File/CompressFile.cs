using iTextSharp.text.pdf;
using Spire.Pdf;
using Spire.Pdf.Exporting;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading;
using PdfDocument = Spire.Pdf.PdfDocument;

namespace FileServer.Common.File
{
    /// <summary>
    /// 功能描述    ：文件压缩类
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-16 19:18:24 
    /// </summary>
    public class CompressFile
    {
        /// <summary>
        /// 压缩图片文件流
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Stream CompressImage(Stream stream)
        {
            Bitmap bitmap = new Bitmap(stream);
            try
            {
                MemoryStream newstream = new MemoryStream();
                #region 设置EncoderParameters
                EncoderParameters encoder = new EncoderParameters();
                long[] quality = new long[1];
                quality[0] = 30;//设置压缩的比例1-100 
                EncoderParameter eParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
                encoder.Param[0] = eParam;
                #endregion

                #region 设置ImageCodecInfo
                ImageCodecInfo[] arrayICI = ImageCodecInfo.GetImageEncoders();
                ImageCodecInfo jpegICIinfo = null;
                for (int x = 0; x < arrayICI.Length; x++)
                {
                    if (arrayICI[x].FormatDescription.Equals("JPEG"))
                    {
                        jpegICIinfo = arrayICI[x];
                        break;
                    }
                }
                #endregion
                // 将图片保存到newstream
                bitmap.Save(newstream, jpegICIinfo, encoder);
                // 将数据写入流
                newstream.Seek(0, SeekOrigin.Begin);
                return newstream;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return null;
            }
            finally
            {
                bitmap.Dispose();
                stream.Dispose();
            }
        }
        /// <summary>
        /// 压缩PDF文件流
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Stream CompressPdf(Stream stream)
        {
            //加载PDF文档
            PdfDocument pdfDocument = new PdfDocument(stream);
            try
            {
                MemoryStream newstream = new MemoryStream();
                #region 压缩内容
                //禁用incremental update
                pdfDocument.FileInfo.IncrementalUpdate = false;

                //设置PDF文档的压缩级别
                pdfDocument.CompressionLevel = SetLevel(stream.Length);
                #endregion
                #region 压缩页面中的图片
                //遍历文档所有页面
                #region 降低图片质量
                //foreach (PdfPageBase page in pdfDocument.Pages)
                //{
                //    //提取页面中的图片
                //    Image[] images = page.ExtractImages();

                //    if (images != null && images.Length > 0)
                //    {
                //        //遍历所有图片
                //        for (int j = 0; j < images.Length; j++)
                //        {
                //            Image image = images[j];

                //            PdfBitmap bp = new PdfBitmap(image)
                //            {

                //                //降低图片的质量
                //                Quality = 40
                //            };

                //            //用压缩后的图片替换原文档中的图片
                //            page.ReplaceImage(j, bp);

                //        }
                //    }
                //}
                #endregion

                #region 直接压缩图片
                foreach (PdfPageBase page in pdfDocument.Pages)
                {
                    if (page != null)
                    {
                        if (page.ImagesInfo != null)
                        {
                            foreach (PdfImageInfo info in page.ImagesInfo)
                            {
                                //压缩图片
                                page.TryCompressImage(info.Index);
                            }
                        }
                    }
                }
                #endregion
                #endregion
                // 保存流
                pdfDocument.SaveToStream(newstream, 0);
                // 将数据写入流
                newstream.Seek(0, SeekOrigin.Begin);

                // => 去水印
                // 水印
                string watermarkText = "Evaluation Warning : The document was created with Spire.PDF for .NET.";
                newstream = DelWatermark(newstream, watermarkText);
                return newstream;
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                return null;
            }
            finally
            {
                pdfDocument.Dispose();
                stream.Dispose();
            }
        }

        public static MemoryStream DelWatermark(MemoryStream newstream, string watermarkText)
        {
            //临时存储内存流
            MemoryStream streamTemp = new MemoryStream();
            PrStream stream;
            string content;
            PdfArray contentarray;
            PdfReader reader = new PdfReader(newstream);
            try
            {
                // => 删除无法访问的对象
                reader.RemoveUnusedObjects();
                // 获取文档页数
                int pageNum = reader.NumberOfPages;
                for (int i = 1; i <= pageNum; i++)
                {
                    PdfDictionary page = reader.GetPageN(i);//获取当前页
                    contentarray = page.GetAsArray(PdfName.Contents);
                    if (contentarray != null)
                    {
                        for (int j = 0; j < contentarray.Size; j++)
                        {
                            stream = (PrStream)contentarray.GetAsStream(j);
                            content = Encoding.ASCII.GetString(PdfReader.GetStreamBytes(stream));//获取pdf页内的文字内容
                            if (content.IndexOf("/OC") >= 0 || content.IndexOf(watermarkText) >= 0)//如果pdf内容包含水印文字
                            {
                                content = content.Replace(watermarkText, "");//替换水印文字为空
                                byte[] byteArray = Encoding.Default.GetBytes(content);//转换为byte[]
                                stream.Put(PdfName.LENGTH, new PdfNumber(byteArray.Length));//重新指定大小

                                stream.SetData(byteArray);//重新赋值
                            }
                        }
                    }
                }
                PdfStamper pdfStamper = new PdfStamper(reader, streamTemp);
                if (pdfStamper != null)
                {
                    pdfStamper.Close();
                }
                streamTemp.Position = 0;
                return streamTemp;
            }
            catch(Exception ee)
            {
                Console.WriteLine(ee.Message);
                return null;
            }
            finally
            {
                newstream.Dispose();
                reader.Close();
            }
        }

        /// <summary>
        /// 设置压缩的级别
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        private static PdfCompressionLevel SetLevel(long length)
        {
            if (length <= 500 * 1024)
                return PdfCompressionLevel.Normal;
            else if(length > 500 * 1024 && length <= 3*1024*1024)
                return PdfCompressionLevel.BelowNormal;
            else
                return PdfCompressionLevel.BestSpeed;
        }
    }
}
