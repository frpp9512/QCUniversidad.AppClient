using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.SchoolYear;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.MappingProfiles
{
    public class SchoolYearProfile : Profile
    {
        public SchoolYearProfile()
        {
            CreateMap<SchoolYearModel, SchoolYearDto>();
            CreateMap<SchoolYearModel, SimpleSchoolYearDto>();
            CreateMap<SchoolYearDto, SchoolYearModel>();
            CreateMap<NewSchoolYearDto, SchoolYearModel>();
            CreateMap<EditSchoolYearDto, SchoolYearModel>();
        }
    }
}
