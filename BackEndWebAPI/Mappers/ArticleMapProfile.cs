using AutoMapper;
using BackEndWebAPI.DTO;
using BackEndWebAPI.VO;
using Domain.Entities;

namespace BackEndWebAPI.Mappers
{
    public class ArticleMapProfile : Profile
    {


        public ArticleMapProfile()
        {

            //全部显式配置映射
            // 只映射部分属性,操作麻烦的在应用层赋值
            CreateMap<Article, ArticleDetailsVo>()
                .ForMember(dest => dest.Id, 
                opt => opt.MapFrom(src => src.Id.ToString()))
                 .ForMember(dest => dest.content, 
                 opt => opt.MapFrom(src => src.Content))
                 .ForMember(dest => dest.Summary, 
                 opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.thumbnail, 
                opt => opt.MapFrom(src => src.Thumbnail))
              /*  .ForMember(dest=>dest.Title,
                opt=>opt.MapFrom(src=>src.Title))*/
                .ForMember(dest => dest.categoryId, 
                opt => opt.MapFrom(src => src.Category.Id))
                .ForMember(dest => dest.categoryName, 
                opt => opt.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.viewCount, 
                opt => opt.MapFrom(src => src.ViewCount))
                .ForMember(dest => dest.tags, 
                opt => opt.MapFrom( src => src.Tags))
                 .ForMember(dest => dest.createTime,
                 opt => opt.MapFrom(src => src.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")));
            // 用于嵌套映射
            CreateMap<Tag, TagDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(TagDTO => TagDTO.name, opt => opt.MapFrom(src => src.TagName));


            CreateMap<Article, ArticleListVo>()
           .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.title, opt => opt.MapFrom(src => src.Title))
               .ForMember(dest => dest.summary, opt => opt.MapFrom(src => src.Description))
                 .ForMember(dest => dest.thumbnail, opt => opt.MapFrom(src => src.Thumbnail))
                   .ForMember(dest => dest.categoryName, opt => opt.MapFrom(src => src.Category.CategoryName)).MaxDepth(2)
                   .ForMember(dest => dest.viewCount, opt => opt.MapFrom(src => src.ViewCount))
                    .ForMember(dest => dest.createTime, opt => opt.MapFrom(src => src.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")));

           
            CreateMap<Article, HotArticleVo>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.title, opt => opt.MapFrom(src => src.Title))
              .ForMember(dest => dest.thumbnail, opt => opt.MapFrom(src => src.Thumbnail))
               .ForMember(dest => dest.viewCount, opt => opt.MapFrom(src => src.ViewCount))
               .ForMember(dest => dest.createTime, opt => opt.MapFrom(src => src.CreateTime.ToString("yyyy-MM-dd HH:mm:ss")));

            CreateMap<Article, ArticleEditDTO>()
              .ForMember(dest => dest.title, opt => opt.MapFrom(src => src.Title))
              .ForMember(dest => dest.content, opt => opt.MapFrom(src => src.Content))
                 .ForMember(dest => dest.summary, opt => opt.MapFrom(src => src.Description))
                 .ForMember(dest => dest.thumbnail, opt => opt.MapFrom(src => src.Thumbnail))
                 .ForMember(dest => dest.isDraft, opt => opt.MapFrom(src => src.IsDraft))
                 .ForMember(dest => dest.category, opt => opt.MapFrom(src => src.Category.CategoryName))
                 .ForMember(dest => dest.tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.TagName))).ReverseMap();







            //  CreateMap<Category,ArticleDTO>();

        }
    }
}
