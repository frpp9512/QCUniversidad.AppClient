using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Discipline;
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
            CreateMap<DisciplineModel, PopulatedDisciplineDto>();
            CreateMap<PopulatedDisciplineDto, DisciplineModel>();
            CreateMap<NewDisciplineDto, DisciplineModel>();
            CreateMap<DisciplineModel, NewDisciplineDto>();
            CreateMap<EditDisciplineDto, DisciplineModel>();
            CreateMap<DisciplineModel, EditDisciplineDto>();
            CreateMap<PopulatedDisciplineDto, TeacherDiscipline>().ForPath(td => td.DisciplineId, opt => opt.MapFrom(d => d.Id));
            CreateMap<SimpleDisciplineDto, TeacherDiscipline>().ForPath(td => td.DisciplineId, opt => opt.MapFrom(d => d.Id));
            CreateMap<SimpleDisciplineDto, DisciplineModel>();
            CreateMap<DisciplineModel, SimpleDisciplineDto>();
        }
    }
}
