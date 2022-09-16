using AutoMapper;
using QCUniversidad.Api.Shared.Dtos;
using QCUniversidad.WebClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.AutoMapperProfiles
{
    public class DisciplineDtoProfile : Profile
    {
        public DisciplineDtoProfile()
        {
            CreateMap<DisciplineDto, DisciplineModel>();
            CreateMap<DisciplineModel, DisciplineDto>();
        }
    }
}
