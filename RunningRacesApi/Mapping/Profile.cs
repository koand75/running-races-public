using AutoMapper;

using RunningRacesApi.Models;
using RunningRacesApi.Models.DTOs;

namespace RunningRacesApi.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<SectionImportDto, Section>();

        CreateMap<Section, SectionExportDto>()
            .ForMember(dest => dest.StartWayPointName, opt => opt.MapFrom(src => src.StartWayPoint != null ? src.StartWayPoint.Name : null))
            .ForMember(dest => dest.StartLat, opt => opt.MapFrom(src => src.StartWayPoint != null ? src.StartWayPoint.Lat : null))
            .ForMember(dest => dest.StartLng, opt => opt.MapFrom(src => src.StartWayPoint != null ? src.StartWayPoint.Lng : null))
            .ForMember(dest => dest.EndWayPointName, opt => opt.MapFrom(src => src.EndWayPoint != null ? src.EndWayPoint.Name : null))
            .ForMember(dest => dest.EndLat, opt => opt.MapFrom(src => src.EndWayPoint != null ? src.EndWayPoint.Lat : null))
            .ForMember(dest => dest.EndLng, opt => opt.MapFrom(src => src.EndWayPoint != null ? src.EndWayPoint.Lng : null));
    }
}
