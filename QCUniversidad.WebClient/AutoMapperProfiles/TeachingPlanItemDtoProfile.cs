using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.TeachingPlan;
using QCUniversidad.WebClient.Models.Planning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.AutoMapperProfiles
{
    public class TeachingPlanItemDtoProfile : Profile
    {
        public TeachingPlanItemDtoProfile()
        {
            CreateMap<TeachingPlanItemModel, TeachingPlanItemDto>();
            CreateMap<TeachingPlanItemDto, TeachingPlanItemModel>();
            CreateMap<TeachingPlanItemModel, TeachingPlanItemSimpleDto>();
            CreateMap<TeachingPlanItemSimpleDto, TeachingPlanItemModel>();
            CreateMap<TeachingPlanItemModel, NewTeachingPlanItemDto>();
            CreateMap<TeachingPlanItemModel, CreateTeachingPlanItemModel>();
            CreateMap<CreateTeachingPlanItemModel, TeachingPlanItemModel>();
            CreateMap<TeachingPlanItemModel, EditTeachingPlanItemModel>();
            CreateMap<EditTeachingPlanItemModel, TeachingPlanItemModel>();
            CreateMap<TeachingPlanItemModel, EditTeachingPlanItemDto>();
        }
    }
}
