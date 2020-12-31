using AutoMapper;
using FileServer.EntityDto.Minio;
using Minio.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileServer.DomainServer.Mapper
{
    /// <summary>
    /// 功能描述    ：
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-14 17:32:52 
    /// </summary>
    public class MinioMapperProfile : Profile
    {
        public MinioMapperProfile()
        {
            CreateMap<Bucket, BucketDto>(MemberList.Destination).ReverseMap();    // ReverseMap双向映射
        }
    }
}
