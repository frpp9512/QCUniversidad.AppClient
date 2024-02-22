using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.Statistics;
using QCUniversidad.WebClient.Models.Statistics;

namespace QCUniversidad.WebClient.AutoMapperProfiles;

public class StatisticItemDtoProfile : Profile
{
    public StatisticItemDtoProfile()
    {
        _ = CreateMap<StatisticItemDto, StatisticItemModel>();
    }
}
