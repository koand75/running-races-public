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

        CreateMap<RaceDto, Race>();
        CreateMap<Race, RaceDto>();
        CreateMap<PagedResult<Race>, PagedResult<RaceDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<RunnerDto, Runner>();
        CreateMap<Runner, RunnerDto>();
        CreateMap<PagedResult<Runner>, PagedResult<RunnerDto>>()
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));

        CreateMap<WayPoint, WayPointDto>();
        CreateMap<WayPointDto, WayPoint>();
        CreateMap<PagedResult<WayPoint>, PagedResult<WayPointDto>>();

        CreateMap<Section, SectionDto>();
        CreateMap<SectionDto, Section>();
        CreateMap<PagedResult<Section>, PagedResult<SectionDto>>();
    }
}
