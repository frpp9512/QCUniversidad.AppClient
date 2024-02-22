using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.Discipline;
using QCUniversidad.WebClient.Models.Disciplines;

namespace QCUniversidad.WebClient.AutoMapperProfiles;

public class DisciplineDtoProfile : Profile
{
    public DisciplineDtoProfile()
    {
        _ = CreateMap<PopulatedDisciplineDto, DisciplineModel>();
        _ = CreateMap<DisciplineModel, PopulatedDisciplineDto>();
        _ = CreateMap<NewDisciplineDto, DisciplineModel>();
        _ = CreateMap<DisciplineModel, NewDisciplineDto>();
        _ = CreateMap<EditDisciplineModel, DisciplineModel>();
        _ = CreateMap<DisciplineModel, EditDisciplineModel>();
        _ = CreateMap<EditDisciplineDto, DisciplineModel>();
        _ = CreateMap<DisciplineModel, EditDisciplineDto>();
        _ = CreateMap<DisciplineModel, SimpleDisciplineDto>();
        _ = CreateMap<SimpleDisciplineDto, DisciplineModel>();
    }
}
