using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Faculty;

namespace QCUniversidad.Api.MappingProfiles;

public class FacultyProfile : Profile
{
    public FacultyProfile()
    {
        _ = CreateMap<FacultyModel, FacultyDto>();
        _ = CreateMap<FacultyDto, FacultyModel>();
    }
}
