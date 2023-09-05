using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.Curriculum;
using QCUniversidad.WebClient.Models.Curriculums;

namespace QCUniversidad.WebClient.AutoMapperProfiles;

public class CurriculumDtoProfile : Profile
{
    public CurriculumDtoProfile()
    {
        _ = CreateMap<CurriculumDto, CurriculumModel>();
        _ = CreateMap<CurriculumModel, CurriculumDto>();
        _ = CreateMap<NewCurriculumDto, CurriculumModel>();
        _ = CreateMap<CurriculumModel, NewCurriculumDto>();
        _ = CreateMap<EditCurriculumDto, CurriculumModel>();
        _ = CreateMap<CurriculumModel, EditCurriculumDto>();
    }
}
