using AutoMapper;
using BackEndWebAPI.DTO;
using BackEndWebAPI.VO;
using Domain.Entities;

namespace BackEndWebAPI.Mappers
{
    public class TagMapProfile : Profile
    {
        public TagMapProfile()
        {
            /*SourceMemberNamingConvention =new PascalCaseNamingConvention();
            DestinationMemberNamingConvention = new LowerUnderscoreNamingConvention();//解决命名问题*/
            CreateMap<Tag, TagCountVO>().ForMember(dest => dest.name, opt => opt.MapFrom(src => src.TagName))
                .ForMember(dest => dest.count, opt => opt.MapFrom(src => src.GetArticleCount()))//匹配到方法
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id.ToString()));

            CreateMap<Tag, TagDTO>().ForMember(dest => dest.name, opt => opt.MapFrom(src => src.TagName))
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id));

        }
    }
}
