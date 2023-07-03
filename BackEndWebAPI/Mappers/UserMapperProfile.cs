using AutoMapper;
using Domain.Entities;
using BackEndWebAPI.VO;
using BackEndWebAPI.DTO;

namespace BackEndWebAPI.Mappers
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile() 
        {
            CreateMap<User, UserInfoVo>().
                ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.userName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.nickName, opt => opt.MapFrom(src => src.NickName))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.avatar, opt => opt.MapFrom(src => src.avatar))
                .ForMember(dest => dest.isAdmin, opt => opt.MapFrom(src => src.UserName == "Admin"))//系统目前就一个管理员先这样写吧
                .ForMember(dest => dest.sex, opt => opt.MapFrom(src => src.sex))
               .ForMember(dest => dest.phoneNumber, opt => opt
               .MapFrom(src => src.PhoneNumber)) ;

            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id.ToString()))
               .ForMember(dest => dest.userName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.nickName, opt => opt.MapFrom(src => src.NickName))
                .ForMember(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.avatar, opt => opt.MapFrom(src => src.avatar))
                .ForMember(dest => dest.sex, opt => opt.MapFrom(src => src.sex.ToString()))
               .ForMember(dest => dest.phonenumber, opt => opt
               .MapFrom(src => src.PhoneNumber))
               .ForMember(dest => dest.signature, opt => opt.MapFrom(src => src.signature)).ReverseMap();

            //映射用户到管理员信息
            CreateMap<User, UserShownInfoVO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.nickName, opt => opt.MapFrom(src => src.NickName))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Avatar, opt => opt.MapFrom(src => src.avatar))
                .ForMember(dest => dest.signature, opt => opt.MapFrom(src => src.signature))
                .ForMember(dest => dest.sex, opt => opt.MapFrom(src => src.sex));
           
        }
    }
}
