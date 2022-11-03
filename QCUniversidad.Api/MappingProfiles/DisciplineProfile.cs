using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Discipline;

namespace QCUniversidad.Api.MappingProfiles;

public class DisciplineProfile : Profile
{
    public DisciplineProfile()
    {
        _ = CreateMap<DisciplineModel, PopulatedDisciplineDto>();
        _ = CreateMap<PopulatedDisciplineDto, DisciplineModel>();
        _ = CreateMap<NewDisciplineDto, DisciplineModel>();
        _ = CreateMap<DisciplineModel, NewDisciplineDto>();
        _ = CreateMap<EditDisciplineDto, DisciplineModel>();
        _ = CreateMap<DisciplineModel, EditDisciplineDto>();
        _ = CreateMap<PopulatedDisciplineDto, TeacherDiscipline>().ForPath(td => td.DisciplineId, opt => opt.MapFrom(d => d.Id));
        _ = CreateMap<SimpleDisciplineDto, TeacherDiscipline>().ForPath(td => td.DisciplineId, opt => opt.MapFrom(d => d.Id));
        _ = CreateMap<SimpleDisciplineDto, DisciplineModel>();
        _ = CreateMap<DisciplineModel, SimpleDisciplineDto>();
    }
}
