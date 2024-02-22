using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.Career;
using QCUniversidad.WebClient.Models.Careers;

namespace QCUniversidad.WebClient.AutoMapperProfiles;

public class CareerDtoProfile : Profile
{
    public CareerDtoProfile()
    {
        _ = CreateMap<CareerDto, CareerModel>();
        _ = CreateMap<CareerModel, CareerDto>();
        _ = CreateMap<CareerModel, NewCareerDto>();
        _ = CreateMap<NewCareerDto, CareerModel>();
        _ = CreateMap<CareerModel, EditCareerDto>();
        _ = CreateMap<EditCareerDto, CareerModel>();
        _ = CreateMap<CareerModel, EditCareerModel>().ForMember(c => c.FacultyName, opt => opt.MapFrom(e => e.Faculty.Name));
        _ = CreateMap<EditCareerModel, CareerModel>();
    }
}
