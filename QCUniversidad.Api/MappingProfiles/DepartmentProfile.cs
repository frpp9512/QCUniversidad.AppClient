using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.MappingProfiles
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<DepartmentModel, DepartmentDto>();
            CreateMap<DepartmentDto, DepartmentModel>();
            CreateMap<NewDepartmentDto, DepartmentModel>();
            CreateMap<DepartmentModel, NewDepartmentDto>();
            CreateMap<EditDepartmentDto, DepartmentModel>();
            CreateMap<DepartmentModel, EditDepartmentDto>();
        }
    }
}
