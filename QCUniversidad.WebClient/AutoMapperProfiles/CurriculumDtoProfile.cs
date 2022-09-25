using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.Curriculum;
using QCUniversidad.WebClient.Models.Curriculums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.AutoMapperProfiles
{
    public class CurriculumDtoProfile : Profile
    {
        public CurriculumDtoProfile()
        {
            CreateMap<CurriculumDto, CurriculumModel>();
            CreateMap<CurriculumModel, CurriculumDto>();
            CreateMap<NewCurriculumDto, CurriculumModel>();
            CreateMap<CurriculumModel, NewCurriculumDto>();
            CreateMap<EditCurriculumDto, CurriculumModel>();
            CreateMap<CurriculumModel, EditCurriculumDto>();
        }
    }
}
