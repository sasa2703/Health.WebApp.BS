using AutoMapper;
using HealthManager.WebApp.BS.Entities.Models;
using HealthManager.WebApp.BS.Shared.DataTransferObjects.Health;
using HealthManager.WebApp.BS.Shared.DataTransferObjects.Product;

namespace FiscalCloud.WebApp.BS.API.Mapping
{
    public class ClinicalTrialProfile : Profile
    {
        public ClinicalTrialProfile()
        {

            CreateMap<ClinicalTrialMetadata, ClinicalTrialDto>()
               .ForMember(dest => dest.TrialId, opt => opt.MapFrom(src => src.TrialId))
               .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
               .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
               .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
               .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants))
               .ForMember(dest => dest.Duration, opt => opt.MapFrom(src => src.Duration));


            CreateMap<EditClinicalTrialDto, ClinicalTrialMetadata>()
               .ForMember(dest => dest.TrialId, opt => opt.MapFrom(src => src.TrialId))
               .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
               .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
               .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
               .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
               .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants));


            CreateMap<ClinicalTrialForCreationDto, ClinicalTrialMetadata>()
                .ForMember(dest => dest.TrialId, opt => opt.MapFrom(src => src.TrialId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate))
                .ForMember(dest => dest.Participants, opt => opt.MapFrom(src => src.Participants))
                .ReverseMap();

        }
    }
}
