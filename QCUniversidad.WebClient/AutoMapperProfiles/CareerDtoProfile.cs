using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.Career;
using QCUniversidad.WebClient.Models.Careers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.AutoMapperProfiles
{
    public class CareerDtoProfile : Profile
    {
        public CareerDtoProfile()
        {
            CreateMap<CareerDto, CareerModel>();
            CreateMap<CareerModel, CareerDto>();
            CreateMap<CareerModel, NewCareerDto>();
            CreateMap<NewCareerDto, CareerModel>();
            CreateMap<CareerModel, EditCareerDto>();
            CreateMap<EditCareerDto, CareerModel>();
            CreateMap<CareerModel, EditCareerModel>().ForMember(c => c.FacultyName, opt => opt.MapFrom(e => e.Faculty.Name));
            CreateMap<EditCareerModel, CareerModel>();
        }
    }
}
