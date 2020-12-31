using log4net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Peihui.Core.CustomException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace FileServer.Api.Filters
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// 日志
        /// </summary>
        private readonly ILog _log;

        /// <summary>
        /// host主机
        /// </summary>
        private readonly IWebHostEnvironment _env;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="env"></param>
        public HttpGlobalExceptionFilter(IWebHostEnvironment env)
        {
            _env = env;
            _log = LogManager.GetLogger(typeof(HttpGlobalExceptionFilter));
        }

        /// <summary>
        /// exception
        /// </summary>
        /// <param name="context">异常上下文</param>
        public void OnException(ExceptionContext context)
        {
            // =>初始化Log4net日志
            //ILog log = LogManager.GetLogger(Startup.Repository.Name, controller.ToString());

            // =>开发环境
            if (!_env.IsDevelopment())
            {
                _log.Error($"\r\n全局异常处理程序捕获到异常：\r\n错误消息：{context.Exception.Message}\r\n错误堆栈：{context.Exception.StackTrace}");

                _log.Error(context.Exception.ToString());
                //var JsonMessage = new ErrorResponse("未知错误,请重试");
                //JsonMessage.DeveloperMessage = context.Exception;
                //context.Result = new ApplicationErrorResult(JsonMessage);
                //context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                //context.ExceptionHandled = true;
            }
            // =>非开发环境
            else
            {
                if (context.Exception is ExceptionHandle)
                {
                    var ex = (ExceptionHandle)context.Exception;
                    var errMsg = new ErrorResponse(ex.ErrorMsg);
                    context.Result = new ObjectResult(errMsg) { StatusCode = ex.StatusCode };
                }
                // =>无权访问异常
                else if (context.Exception is UnauthorizedAccessException)
                {
                    context.Result = new ObjectResult(new ErrorResponse("无权访问")) { StatusCode = (int)HttpStatusCode.Unauthorized };
                }
                else
                {
                    //var json = new ErrorResponse("未知错误,请联系管理员");
                    var ex = context.Exception;
                    //if (_env.IsDevelopment())
                    //    json.DeveloperMessage = context.Exception;
                    context.Result = new ObjectResult(new ErrorResponse(ex.Message)) { StatusCode = (int)HttpStatusCode.BadRequest };
                    _log.Error(ex.ToString());
                }
                //context.Result = new ObjectResult(json) { StatusCode = context.HttpContext.Response.StatusCode};
                context.ExceptionHandled = true;
            }
        }

        /// <summary>
        /// 错误结果类
        /// </summary>
        public class ApplicationErrorResult : ObjectResult
        {
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="value"></param>
            public ApplicationErrorResult(object value) : base(value)
            {
                StatusCode = (int)HttpStatusCode.InternalServerError;
            }
        }

        /// <summary>
        /// 错误Response
        /// </summary>
        public class ErrorResponse
        {
            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="msg"></param>
            public ErrorResponse(string msg)
            {
                Message = msg;
            }

            /// <summary>
            /// 消息内容
            /// </summary>
            public string Message { get; set; }
        }
    }
}
