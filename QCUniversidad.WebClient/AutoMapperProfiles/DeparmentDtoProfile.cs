using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.Department;
using QCUniversidad.WebClient.Models.Departments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.AutoMapperProfiles
{
    public class DeparmentDtoProfile : Profile
    {
        public DeparmentDtoProfile()
        {
            CreateMap<DepartmentDto, DepartmentModel>();
            CreateMap<DepartmentModel, DepartmentDto>();
            CreateMap<NewDepartmentDto, DepartmentModel>();
            CreateMap<DepartmentModel, NewDepartmentDto>();
            CreateMap<EditDepartmentModel, DepartmentModel>();
            CreateMap<DepartmentModel, EditDepartmentModel>().ForMember(d => d.FacultyName, opt => opt.MapFrom(d => d.Faculty.Name));
            CreateMap<EditDepartmentDto, DepartmentModel>();
            CreateMap<DepartmentModel, EditDepartmentDto>();
        }
    }
}
