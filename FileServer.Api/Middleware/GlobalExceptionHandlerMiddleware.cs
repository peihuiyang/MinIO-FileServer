using FileServer.Api.Configuration.JsonOptions;
using log4net;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Peihui.Core.CustomException;
using Peihui.Core.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FileServer.Api.Middleware
{
    public class GlobalExceptionHandlerMiddleware
    {
        /// <summary>
        /// 请求代理
        /// </summary>
        private readonly RequestDelegate _next;
        private readonly ILog _log;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="next"></param>
        public GlobalExceptionHandlerMiddleware(RequestDelegate next)
        {
            this._next = next;
            _log = LogManager.GetLogger(typeof(GlobalExceptionHandlerMiddleware));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await this._next(httpContext);
            }
            catch (ExceptionHandle ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception ex)
        {
            if (ex == null) return;
            await WriteExceptionAsync(httpContext, ex).ConfigureAwait(false);
        }

        private async Task WriteExceptionAsync(HttpContext httpContext, Exception ex)
        {
            // 记录日志
            _log.Error(ex.ToString());
            // 待实现

            // 返回友好提示
            var response = httpContext.Response;

            // 状态码
            if (ex is UnauthorizedAccessException)
            {
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else if (ex is ExceptionHandle)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            // 返回内容格式化
            response.ContentType = httpContext.Request.Headers["Accept"];
            if (response.ContentType.ToLower() == "application/xml")
            {
                await response.WriteAsync(Object2XmlString(ResponseResult.Error(ex.GetBaseException().Message))).ConfigureAwait(false);
            }
            else
            {
                if (ex is ExceptionHandle)
                {
                    ExceptionHandle busException = (ExceptionHandle)ex;
                    await response.WriteAsync(JsonConvert.SerializeObject(
                            ResponseResult.Error(busException.ErrorMsg),
                            new JsonSerializerSettings
                            {
                                ContractResolver = new LowerContractResolver()
                            })
                          ).ConfigureAwait(false);
                }
                else
                {
                    await response.WriteAsync(JsonConvert.SerializeObject(
                            ResponseResult.Error(ex.GetBaseException().Message),
                            new JsonSerializerSettings
                            {
                                ContractResolver = new LowerContractResolver()
                            })
                          ).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// 对象转换为xml
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static string Object2XmlString(object o)
        {
            StringWriter sw = new StringWriter();
            try
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                serializer.Serialize(sw, o);
            }
            catch
            {
                //Handle Exception Code
            }
            finally
            {
                sw.Dispose();
            }
            return sw.ToString();
        }

    }
}
