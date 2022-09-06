using AutoMapper;
using QCUniversidad.Api.Shared.Dtos;
using QCUniversidad.AppClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.AutoMapperProfiles
{
    public class DeparmentDtoProfile : Profile
    {
        public DeparmentDtoProfile()
        {
            CreateMap<DepartmentDto, DepartmentModel>();
            CreateMap<DepartmentModel, DepartmentDto>();
        }
    }
}
