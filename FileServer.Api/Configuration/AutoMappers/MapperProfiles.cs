using AutoMapper;
using FileServer.DomainServer.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FileServer.Api.Configuration.AutoMappers
{
    /// <summary>
    /// 实体自动映射配置
    /// </summary>
    public static class MapperProfiles
    {
        static MapperProfiles()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // => 添加配置的类
                // Minio层
                cfg.AddProfile<MinioMapperProfile>();
            });
        }
    }
}
