using Mapster;

using RunningRacesApi.Models;
using RunningRacesApi.Models.DTOs;

namespace RunningRacesApi.Mappings;

public static class MappingConfig
{
    public static void Configure()
    {
        TypeAdapterConfig<Section, SectionExportDto>.NewConfig()
            .Map(dest => dest.StartWayPointName, src => src.StartWayPoint != null ? src.StartWayPoint.Name : null)
            .Map(dest => dest.StartLat, src => src.StartWayPoint != null ? src.StartWayPoint.Lat : null)
            .Map(dest => dest.StartLng, src => src.StartWayPoint != null ? src.StartWayPoint.Lng : null)
            .Map(dest => dest.EndWayPointName, src => src.EndWayPoint != null ? src.EndWayPoint.Name : null)
            .Map(dest => dest.EndLat, src => src.EndWayPoint != null ? src.EndWayPoint.Lat : null)
            .Map(dest => dest.EndLng, src => src.EndWayPoint != null ? src.EndWayPoint.Lng : null);
    }
}