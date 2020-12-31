using AutoMapper;
using FileServer.Common.Minio;
using FileServer.EntityDto.Minio;
using FileServer.IDomainServer.Minio;
using log4net;
using Minio.DataModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileServer.DomainServer.Minio.MinioBucket
{
    /// <summary>
    /// 功能描述    ：存储桶业务操作
    /// 创 建 者    ：Yang Peihui
    /// 创建日期    ：2020-12-14 15:03:02 
    /// </summary>
    public partial class MinioBucketDomain : IMinioBucketDomain
    {
        private readonly IMinioHelper _minioHelper;
        private readonly ILog _log;
        private readonly IMapper _mapper;

        public MinioBucketDomain(IMinioHelper minioHelper, IMapper mapper)
        {
            _minioHelper = minioHelper;
            _mapper = mapper;
            _log = LogManager.GetLogger(typeof(MinioBucketDomain));
        }

        private List<BucketDto> ChangeEntityDto(List<Bucket> result)
        {
            List<BucketDto> bucketDtos = new List<BucketDto>();
            foreach (var item in result)
            {
                BucketDto bucketDto = new BucketDto
                {
                    Name = item.Name,
                    CreationDate = item.CreationDate,
                    CreationDateDateTime = item.CreationDateDateTime
                };
                bucketDtos.Add(bucketDto);
            }
            return bucketDtos;
        }
    }
}
