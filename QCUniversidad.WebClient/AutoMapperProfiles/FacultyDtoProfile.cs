using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.Faculty;
using QCUniversidad.WebClient.Models.Faculties;

namespace QCUniversidad.WebClient.AutoMapperProfiles;

public class FacultyDtoProfile : Profile
{
    public FacultyDtoProfile()
    {
        _ = CreateMap<FacultyDto, FacultyModel>();
        _ = CreateMap<FacultyModel, FacultyDto>();
    }
}