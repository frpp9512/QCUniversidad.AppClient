using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.Period;
using QCUniversidad.WebClient.Models.Periods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.AutoMapperProfiles
{
    public class PeriodDtoProfile : Profile
    {
        public PeriodDtoProfile()
        {
            CreateMap<PeriodDto, PeriodModel>();
            CreateMap<PeriodModel, PeriodDto>();
            CreateMap<PeriodModel, NewPeriodDto>();
            CreateMap<CreatePeriodModel, PeriodModel>();
            CreateMap<EditPeriodDto, PeriodModel>();
            CreateMap<PeriodModel, EditPeriodDto>();
        }
    }
}
