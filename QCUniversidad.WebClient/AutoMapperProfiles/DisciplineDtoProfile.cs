using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.Discipline;
using QCUniversidad.WebClient.Models.Disciplines;
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
            CreateMap<PopulatedDisciplineDto, DisciplineModel>();
            CreateMap<DisciplineModel, PopulatedDisciplineDto>();
            CreateMap<NewDisciplineDto, DisciplineModel>();
            CreateMap<DisciplineModel, NewDisciplineDto>();
            CreateMap<EditDisciplineModel, DisciplineModel>();
            CreateMap<DisciplineModel, EditDisciplineModel>();
            CreateMap<EditDisciplineDto, DisciplineModel>();
            CreateMap<DisciplineModel, EditDisciplineDto>();
            CreateMap<DisciplineModel, SimpleDisciplineDto>();
            CreateMap<SimpleDisciplineDto, DisciplineModel>();
        }
    }
}
