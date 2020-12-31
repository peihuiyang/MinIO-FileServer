using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using FileServer.Api.Configuration.AutoMappers;
using FileServer.Api.Configuration.Consul;
using FileServer.Api.Configuration.Swagger;
using FileServer.Api.Filters;
using FileServer.Api.Middleware;
using FileServer.Api.RequestBody;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;


namespace FileServer.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// 程序集列表
        /// </summary>
        private static readonly List<string> _Assemblies = new List<string>()
        {
            "Peihui.Common.Security",
            "FileServer.Common",
            "FileServer.ApplicationServer",
            "FileServer.DomainServer"
        };
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="container"></param>
        public void ConfigureContainer(ContainerBuilder container)
        {
            var assemblys = _Assemblies.Select(x => Assembly.Load(x)).ToList();
            List<Type> allTypes = new List<Type>();
            assemblys.ForEach(aAssembly =>
            {
                allTypes.AddRange(aAssembly.GetTypes());
            });

            // 通过Autofac自动完成依赖注入
            container.RegisterTypes(allTypes.ToArray())
                .AsImplementedInterfaces()
                .PropertiesAutowired()
                .InstancePerDependency();

            // 注册Controller
            container.RegisterAssemblyTypes(typeof(Startup).GetTypeInfo().Assembly)
                .Where(t => typeof(Controller).IsAssignableFrom(t) && t.Name.EndsWith("Controller", StringComparison.Ordinal))
                .PropertiesAutowired();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region 全局异常捕获
            //services.AddMvc(options =>
            //{
            //    options.Filters.Add<HttpGlobalExceptionFilter>();
            //});
            #endregion

            services.AddControllers(o => o.InputFormatters.Insert(0, new RawRequestBodyFormatter()))
               .AddNewtonsoftJson(options =>
               {
                   //修改属性名称的序列化方式，首字母小写
                   options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                   //修改时间的序列化方式
                   //options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
                   //options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                   options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
               });
            //注册http请求服务
            services.AddHttpClient();

            #region Swagger生成器
            services.AddSwaggerGen(c =>
            {
                // => 分组v1
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "基于MinIO文件服务管理后台API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "杨培辉",
                        Email = "2019070053@sanhepile.com",
                        Url = new Uri("https://github.com/peihuiyang")
                    }
                });
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Token登录验证,格式：Bearer {token}(注意两者之间是一个空格)",
                    Name = "Authorization",
                    //这两个参数均有修改
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                     {
                          new OpenApiSecurityScheme
                          {
                                Reference = new OpenApiReference
                                {
                                      Type = ReferenceType.SecurityScheme,
                                      Id = "Bearer"
                                }
                          },
                          new string[] { }
                     }
                });
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                //var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                var xmlPath = Path.Combine(basePath, "FileServer.Api.xml");
                c.IncludeXmlComments(xmlPath);//第二个参数true表示用控制器的XML注释。默认false
                var entityDtoXmlPath = Path.Combine(basePath, "FileServer.EntityDto.xml");
                c.IncludeXmlComments(entityDtoXmlPath);
                //添加对控制器的标签(描述)
                c.DocumentFilter<SwaggerDocTag>();
                //c.OperationFilter<SwaggerFileUploadFilter>();
            });
            #endregion

            #region 使用AutoMapper
            services.AddAutoMapper(typeof(MapperProfiles));
            #endregion

            #region 设置上传文件大小上限
            services.Configure<KestrelServerOptions>(options =>
            {
                // Set the limit to 256 MB
                options.Limits.MaxRequestBodySize = 268435456;
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            #region 注册服务到ConsulServer
            this.Configuration.ConsulRegister();
            #endregion

            #region 全局异常处理
            app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
            #endregion

            #region Swagger中间件服务
            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "三和管桩文件服务管理后台API V1");
                c.RoutePrefix = string.Empty;
            });
            #endregion           

            //配置支持nginx
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
