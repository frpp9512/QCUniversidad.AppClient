using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Career;

namespace QCUniversidad.Api.MappingProfiles;

public class CareerProfile : Profile
{
    public CareerProfile()
    {
        _ = CreateMap<CareerModel, CareerDto>().ForMember(c => c.Departments, opt => opt.MapFrom(model => model.CareerDepartments.Select(cd => cd.DepartmentId)));
        _ = CreateMap<CareerDto, CareerModel>();
        _ = CreateMap<CareerModel, NewCareerDto>();
        _ = CreateMap<NewCareerDto, CareerModel>();
        _ = CreateMap<CareerModel, EditCareerDto>();
        _ = CreateMap<EditCareerDto, CareerModel>();
        _ = CreateMap<CareerModel, SimpleCareerDto>();
    }
}
