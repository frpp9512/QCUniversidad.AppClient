using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.LoadItem;

namespace QCUniversidad.Api.MappingProfiles;

public class LoadItemsProfile : Profile
{
    public LoadItemsProfile()
    {
        _ = CreateMap<LoadItemModel, LoadItemDto>();
        _ = CreateMap<LoadItemModel, SimpleLoadItemDto>();
    }
}
