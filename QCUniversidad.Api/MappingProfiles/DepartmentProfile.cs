using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Department;

namespace QCUniversidad.Api.MappingProfiles;

public class DepartmentProfile : Profile
{
    public DepartmentProfile()
    {
        _ = CreateMap<DepartmentModel, DepartmentDto>().ForMember(d => d.Careers, opt => opt.MapFrom(model => model.DepartmentCareers.Select(dc => dc.Career)));
        _ = CreateMap<DepartmentDto, DepartmentModel>();
        _ = CreateMap<NewDepartmentDto, DepartmentModel>().ForMember(d => d.DepartmentCareers, opt => opt.MapFrom(dto => dto.SelectedCareers.Select(sc => new DepartmentCareer { CareerId = sc })));
        _ = CreateMap<DepartmentModel, NewDepartmentDto>();
        _ = CreateMap<EditDepartmentDto, DepartmentModel>().ForMember(d => d.DepartmentCareers, opt => opt.MapFrom(dto => dto.SelectedCareers.Select(sc => new DepartmentCareer { DepartmentId = dto.Id, CareerId = sc })));
        _ = CreateMap<DepartmentModel, EditDepartmentDto>();
        _ = CreateMap<DepartmentModel, SimpleDepartmentDto>();
    }
}
