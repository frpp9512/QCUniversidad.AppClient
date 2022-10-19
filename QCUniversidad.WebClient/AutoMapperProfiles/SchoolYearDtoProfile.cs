using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.SchoolYear;
using QCUniversidad.WebClient.Models.SchoolYears;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.AutoMapperProfiles
{
    public class SchoolYearDtoProfile : Profile
    {
        public SchoolYearDtoProfile()
        {
            CreateMap<SchoolYearDto, SchoolYearModel>();
            CreateMap<SimpleSchoolYearDto, SchoolYearModel>();
            CreateMap<SchoolYearModel, NewSchoolYearDto>();
            CreateMap<SchoolYearModel, EditSchoolYearDto>();
        }
    }
}
