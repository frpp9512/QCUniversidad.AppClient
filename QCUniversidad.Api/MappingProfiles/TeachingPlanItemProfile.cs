using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.TeachingPlan;

namespace QCUniversidad.Api.MappingProfiles;

public class TeachingPlanItemProfile : Profile
{
    public TeachingPlanItemProfile()
    {
        _ = CreateMap<TeachingPlanItemModel, TeachingPlanItemDto>().AfterMap((o, planItem) =>
        {
            planItem.TotalLoadCovered = planItem.LoadItems.Sum(i => i.HoursCovered);
            planItem.AllowLoad = planItem.TotalHoursPlanned > planItem.TotalLoadCovered;
        });
        _ = CreateMap<TeachingPlanItemDto, TeachingPlanItemModel>();
        _ = CreateMap<TeachingPlanItemModel, TeachingPlanItemSimpleDto>();
        _ = CreateMap<TeachingPlanItemSimpleDto, TeachingPlanItemModel>();
        _ = CreateMap<NewTeachingPlanItemDto, TeachingPlanItemModel>();
        _ = CreateMap<EditTeachingPlanItemDto, TeachingPlanItemModel>();
    }
}