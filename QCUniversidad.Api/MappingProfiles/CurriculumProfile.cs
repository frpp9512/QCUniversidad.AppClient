using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Curriculum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace QCUniversidad.Api.MappingProfiles
{
    public class CurriculumProfile : Profile
    {
        public CurriculumProfile()
        {
            CreateMap<CurriculumModel, CurriculumDto>().ForMember(dto => dto.CurriculumDisciplines, opt => opt.MapFrom(c => c.CurriculumDisciplines.Select(cs => cs.Discipline)));
            CreateMap<CurriculumDto, CurriculumModel>();
            CreateMap<NewCurriculumDto, CurriculumModel>().ForMember(c => c.CurriculumDisciplines, opt => opt.MapFrom(dto => dto.SelectedDisciplines.Select(sd => new CurriculumDiscipline { DisciplineId = sd })));
            CreateMap<CurriculumModel, NewCurriculumDto>();
            CreateMap<EditCurriculumDto, CurriculumModel>().ForMember(c => c.CurriculumDisciplines, opt => opt.MapFrom(dto => dto.SelectedDisciplines.Select(sd => new CurriculumDiscipline { DisciplineId = sd, CurriculumId = dto.Id })));
            CreateMap<CurriculumModel, EditCurriculumDto>();
        }
    }
}
