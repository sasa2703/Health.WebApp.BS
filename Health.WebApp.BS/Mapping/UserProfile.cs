using AutoMapper;
using HealthManager.WebApp.BS.Entities.Models;
using HealthManager.WebApp.BS.Shared.DataTransferObjects.User;

namespace HealthManager.WebApp.BS.API.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserForUpdateDto, User>()
                 .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                 .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                 .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                 .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.CountryCode))
                 .ForMember(dest => dest.TimeZone,
                            opt => { opt.Condition(src => src.TimeZone != null); opt.MapFrom(src => src.TimeZone); })
                 .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleID))
                 .ForMember(dest => dest.Deleted, opt => opt.MapFrom(src => src.Deleted))          
                 .ReverseMap();


            CreateMap<InvitedUserDto, User>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.SubscriptionId, opt => opt.MapFrom(src => src.SubscriptionId))
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.CountryCode))
                .ForMember(dest => dest.TimeZone, opt => opt.MapFrom(src => src.TimeZone))
                .ReverseMap();

            CreateMap<UserDto, InvitedUserDto>()
                .ReverseMap();

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.SubscriptionId, opt => opt.MapFrom(src => src.Subscription.Id))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.UserCategoryNavigation))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Phone))
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(src => src.CountryCode))
                .ForMember(dest => dest.TimeZone, opt => opt.MapFrom(src => src.TimeZone))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.DtCreated))
                .ForMember(dest => dest.LastUpdate, opt => opt.MapFrom(src => src.DtLastUpdate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(u => u.Status.StatusName))
                .ForMember(dest => dest.UserID, opt => opt.MapFrom(u => u.Id))
                .ReverseMap();

            CreateMap<UserCategory, UserCategoryDto>()
                .ForMember(dest => dest.UserCategoryId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserCategoryName, opt => opt.MapFrom(src => src.UserCategoryName))
                .ReverseMap();

        }
    }
}
