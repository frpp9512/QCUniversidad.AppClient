using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.Faculty;
using QCUniversidad.WebClient.Models.Faculties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.AutoMapperProfiles
{
    public class FacultyDtoProfile : Profile
    {
        public FacultyDtoProfile()
        {
            CreateMap<FacultyDto, FacultyModel>();
            CreateMap<FacultyModel, FacultyDto>();
        }
    }
}