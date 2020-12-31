using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileServer.Api.Configuration.Swagger
{
    /// <summary>
    /// 接口文档注释
    /// </summary>
    public class SwaggerDocTag : IDocumentFilter
    {
        /// <summary>
        /// 添加控制器注释
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = new List<OpenApiTag>
            {
                //添加控制器类描述
                new OpenApiTag{Name = "Health",Description = "健康检查"},
                new OpenApiTag{Name = "MinioBucket",Description = "存储桶操作"},
                new OpenApiTag{Name = "MinioObject",Description = "文件操作"},
                new OpenApiTag{Name = "FileExtend",Description = "文件拓展操作"},
            };
        }
    }
}
