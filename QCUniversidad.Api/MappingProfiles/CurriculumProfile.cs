using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Curriculum;

namespace QCUniversidad.Api.MappingProfiles;

public class CurriculumProfile : Profile
{
    public CurriculumProfile()
    {
        _ = CreateMap<CurriculumModel, CurriculumDto>().ForMember(dto => dto.CurriculumDisciplines, opt => opt.MapFrom(c => c.CurriculumDisciplines.Select(cs => cs.Discipline)));
        _ = CreateMap<CurriculumDto, CurriculumModel>();
        _ = CreateMap<NewCurriculumDto, CurriculumModel>().ForMember(c => c.CurriculumDisciplines, opt => opt.MapFrom(dto => dto.SelectedDisciplines.Select(sd => new CurriculumDiscipline { DisciplineId = sd })));
        _ = CreateMap<CurriculumModel, NewCurriculumDto>();
        _ = CreateMap<EditCurriculumDto, CurriculumModel>().ForMember(c => c.CurriculumDisciplines, opt => opt.MapFrom(dto => dto.SelectedDisciplines.Select(sd => new CurriculumDiscipline { DisciplineId = sd, CurriculumId = dto.Id })));
        _ = CreateMap<CurriculumModel, EditCurriculumDto>();
    }
}
