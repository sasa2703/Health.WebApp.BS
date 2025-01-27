using AutoMapper;
using HealthManager.WebApp.BS.Entities.Models;
using HealthManager.WebApp.BS.Shared.DataTransferObjects.ApiAccessToken;

namespace HealthManager.WebApp.BS.API.Mapping
{
    public class ApiAccessTokenProfile : Profile
    {
        public ApiAccessTokenProfile()
        {

            CreateMap<ApiAccessToken, ApiAccessTokenDto>()
               .ForMember(dest => dest.ApiAccessTokenId, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.LoginId, opt => opt.MapFrom(src => src.LoginId))
               .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.DtCreated))
               .ForMember(dest => dest.ExpireDate, opt => opt.MapFrom(src => src.DtExpireDate))
               .ForMember(dest => dest.KeyVaultSecretId, opt => opt.MapFrom(src => src.KeyVaultSecretId));


            CreateMap<ApiAccessTokenForCreationDto, ApiAccessToken>()
                .ForMember(dest => dest.LoginId, opt => opt.MapFrom(src => src.LoginId))
                .ForMember(dest => dest.DtExpireDate, opt => opt.MapFrom(src => src.ExpireDate));
                  
        }
    }
}
