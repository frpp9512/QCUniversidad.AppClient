using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Career;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.MappingProfiles
{
    public class CareerProfile : Profile
    {
        public CareerProfile()
        {
            CreateMap<CareerModel, CareerDto>();
            CreateMap<CareerDto, CareerModel>();
            CreateMap<CareerModel, NewCareerDto>();
            CreateMap<NewCareerDto, CareerModel>();
            CreateMap<CareerModel, EditCareerDto>();
            CreateMap<EditCareerDto, CareerModel>();
        }
    }
}
