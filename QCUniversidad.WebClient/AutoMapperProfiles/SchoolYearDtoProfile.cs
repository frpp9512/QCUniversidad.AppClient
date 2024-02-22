using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.SchoolYear;
using QCUniversidad.WebClient.Models.SchoolYears;

namespace QCUniversidad.WebClient.AutoMapperProfiles;

public class SchoolYearDtoProfile : Profile
{
    public SchoolYearDtoProfile()
    {
        _ = CreateMap<SchoolYearDto, SchoolYearModel>();
        _ = CreateMap<SimpleSchoolYearDto, SchoolYearModel>();
        _ = CreateMap<SchoolYearModel, NewSchoolYearDto>();
        _ = CreateMap<SchoolYearModel, EditSchoolYearDto>();
    }
}
