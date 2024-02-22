using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.Department;
using QCUniversidad.WebClient.Models.Departments;

namespace QCUniversidad.WebClient.AutoMapperProfiles;

public class DeparmentDtoProfile : Profile
{
    public DeparmentDtoProfile()
    {
        _ = CreateMap<DepartmentDto, DepartmentModel>().ForMember(d => d.SelectedCareers, opt => opt.MapFrom(dto => dto.Careers.Select(c => c.Id)));
        _ = CreateMap<DepartmentModel, DepartmentDto>();
        _ = CreateMap<NewDepartmentDto, DepartmentModel>();
        _ = CreateMap<DepartmentModel, NewDepartmentDto>();
        _ = CreateMap<EditDepartmentModel, DepartmentModel>();
        _ = CreateMap<DepartmentModel, EditDepartmentModel>()
            .ForMember(d => d.FacultyName, opt => opt.MapFrom(d => d.Faculty.Name))
            .ForMember(d => d.SelectedCareers, opt => opt.MapFrom(model => model.Careers.Select(c => c.Id)));
        _ = CreateMap<EditDepartmentDto, DepartmentModel>();
        _ = CreateMap<DepartmentModel, EditDepartmentDto>();
    }
}