using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.MappingProfiles
{
    public class DisciplineProfile : Profile
    {
        public DisciplineProfile()
        {
            CreateMap<DisciplineModel, DisciplineDto>();
            CreateMap<DisciplineDto, DisciplineModel>();
            CreateMap<NewDisciplineDto, DisciplineModel>();
            CreateMap<DisciplineModel, NewDisciplineDto>();
            CreateMap<EditDisciplineDto, DisciplineModel>();
            CreateMap<DisciplineModel, EditDisciplineDto>();
        }
    }
}
