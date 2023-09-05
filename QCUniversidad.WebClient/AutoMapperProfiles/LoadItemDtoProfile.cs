using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.LoadItem;
using QCUniversidad.WebClient.Models.LoadDistribution;
using QCUniversidad.WebClient.Models.LoadItem;

namespace QCUniversidad.WebClient.AutoMapperProfiles;

public class LoadItemDtoProfile : Profile
{
    public LoadItemDtoProfile()
    {
        _ = CreateMap<CreateLoadItemModel, NewLoadItemDto>();
        _ = CreateMap<LoadItemDto, LoadItemModel>();
        _ = CreateMap<SimpleLoadItemDto, LoadItemModel>();
        _ = CreateMap<LoadViewItemDto, LoadViewItemModel>();
        _ = CreateMap<SetNonTeachingLoadModel, SetNonTeachingLoadDto>();
    }
}
