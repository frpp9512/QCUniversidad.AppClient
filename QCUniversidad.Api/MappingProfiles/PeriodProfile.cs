using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Period;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.MappingProfiles
{
    public class PeriodProfile : Profile
    {
        public PeriodProfile()
        {
            CreateMap<PeriodModel, PeriodDto>();
            CreateMap<PeriodDto, PeriodModel>();
            CreateMap<NewPeriodDto, PeriodModel>();
            CreateMap<EditPeriodDto, PeriodModel>();
            CreateMap<PeriodModel, EditPeriodDto>();
            CreateMap<PeriodModel, SimplePeriodDto>();
        }
    }
}
