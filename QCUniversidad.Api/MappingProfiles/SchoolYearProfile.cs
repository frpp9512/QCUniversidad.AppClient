using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.SchoolYear;

namespace QCUniversidad.Api.MappingProfiles;

public class SchoolYearProfile : Profile
{
    public SchoolYearProfile()
    {
        _ = CreateMap<SchoolYearModel, SchoolYearDto>();
        _ = CreateMap<SchoolYearModel, SimpleSchoolYearDto>();
        _ = CreateMap<SchoolYearDto, SchoolYearModel>();
        _ = CreateMap<NewSchoolYearDto, SchoolYearModel>();
        _ = CreateMap<EditSchoolYearDto, SchoolYearModel>();
    }
}
