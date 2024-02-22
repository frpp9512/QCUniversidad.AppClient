using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.TeachingPlan;
using QCUniversidad.WebClient.Models.Planning;

namespace QCUniversidad.WebClient.AutoMapperProfiles;

public class TeachingPlanItemDtoProfile : Profile
{
    public TeachingPlanItemDtoProfile()
    {
        _ = CreateMap<TeachingPlanItemModel, TeachingPlanItemDto>();
        _ = CreateMap<TeachingPlanItemDto, TeachingPlanItemModel>();
        _ = CreateMap<TeachingPlanItemModel, TeachingPlanItemSimpleDto>();
        _ = CreateMap<TeachingPlanItemSimpleDto, TeachingPlanItemModel>();
        _ = CreateMap<TeachingPlanItemModel, NewTeachingPlanItemDto>();
        _ = CreateMap<TeachingPlanItemModel, CreateTeachingPlanItemModel>();
        _ = CreateMap<CreateTeachingPlanItemModel, TeachingPlanItemModel>();
        _ = CreateMap<TeachingPlanItemModel, EditTeachingPlanItemModel>();
        _ = CreateMap<EditTeachingPlanItemModel, TeachingPlanItemModel>();
        _ = CreateMap<TeachingPlanItemModel, EditTeachingPlanItemDto>();
    }
}
