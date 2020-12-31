using Newtonsoft.Json;
using Peihui.Core.CustomException;
using Peihui.Core.EnDecrypt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;

namespace FileServer.Common.BaiDuAI
{
    /// <summary>
    /// 功能描述    ：调用百度AI
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-29 11:10:26 
    /// </summary>
    public class PictureService
    {
		// 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务
		private static readonly string ClientId = AesHelper.Decrypt("qXdspvWFhBoRtvx0+GvGSsC8xvFv3GAcJ47KnGz2Ga0=");
		// 百度云中开通对应服务应用的 Secret Key
		private static readonly string ClientSecret = AesHelper.Decrypt("P74LjjbqOghbZ8FY3PoVQJyYCGNDKR/eHfiCgfm05M7G0/xuJSbZZBekfsrJHDcK");
        /// <summary>
        /// 百度鉴权
        /// </summary>
        /// <returns></returns>
		public static string GetAccessToken()
		{
            string result;
            string authHost = "https://aip.baidubce.com/oauth/2.0/token";
			HttpClient client = new HttpClient();
            List<KeyValuePair<string, string>> paraList = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", ClientId),
                new KeyValuePair<string, string>("client_secret", ClientSecret)
            };

            HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                RespSuccessEntity respSuccessEntity = JsonConvert.DeserializeObject<RespSuccessEntity>(response.Content.ReadAsStringAsync().Result);
                result = respSuccessEntity.Access_token;
            }
            else
            {
                throw new ExceptionHandle(new ExceptionEntity(400, "百度AI接口鉴权失败"));
            }
			return result;
		}
        /// <summary>
        /// 获取图片上的文字
        /// </summary>
        /// <param name="pictureData"></param>
        /// <returns></returns>
        public static string WebImage(byte[] pictureData)
        {
            string token = GetAccessToken();
            string host = "https://aip.baidubce.com/rest/2.0/ocr/v1/webimage?access_token=" + token;
            Encoding encoding = Encoding.Default;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
            request.Method = "POST";
            request.KeepAlive = true;
            // 图片的base64编码
            string base64 = GetFileBase64(pictureData);
            string str = "image=" + HttpUtility.UrlEncode(base64);
            byte[] buffer = encoding.GetBytes(str);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
            string result = reader.ReadToEnd();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return result;
            }
            else
                return null;
        }

        public static string GetFileBase64(byte[] pictureData)
        {
            string baser64 = Convert.ToBase64String(pictureData);
            return baser64;
        }
    }
}
