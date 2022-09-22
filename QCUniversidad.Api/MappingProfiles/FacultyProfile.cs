using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Faculty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.MappingProfiles
{
    public class FacultyProfile : Profile
    {
        public FacultyProfile()
        {
            CreateMap<FacultyModel, FacultyDto>();
            CreateMap<FacultyDto, FacultyModel>();
        }
    }
}
