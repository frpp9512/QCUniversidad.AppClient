using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.TeachingPlan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.MappingProfiles
{
    public class TeachingPlanItemProfile : Profile
    {
        public TeachingPlanItemProfile()
        {
            CreateMap<TeachingPlanItemModel, TeachingPlanItemDto>().AfterMap((o, planItem) =>
            {
                planItem.TotalLoadCovered = planItem.LoadItems.Sum(i => i.HoursCovered);
                planItem.AllowLoad = planItem.TotalHoursPlanned > planItem.TotalLoadCovered;
            });
            CreateMap<TeachingPlanItemDto, TeachingPlanItemModel>();
            CreateMap<TeachingPlanItemModel, TeachingPlanItemSimpleDto>();
            CreateMap<TeachingPlanItemSimpleDto, TeachingPlanItemModel>();
            CreateMap<NewTeachingPlanItemDto, TeachingPlanItemModel>();
            CreateMap<EditTeachingPlanItemDto, TeachingPlanItemModel>();
        }
    }
}