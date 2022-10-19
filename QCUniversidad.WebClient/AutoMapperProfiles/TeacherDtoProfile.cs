using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.Teacher;
using QCUniversidad.WebClient.Models.Teachers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.AutoMapperProfiles
{
    public class TeacherDtoProfile : Profile
    {
        public TeacherDtoProfile()
        {
            CreateMap<TeacherModel, TeacherDto>();
            CreateMap<TeacherDto, TeacherModel>();
            CreateMap<NewTeacherDto, TeacherModel>();
            CreateMap<TeacherModel, NewTeacherDto>();
            CreateMap<EditTeacherDto, TeacherModel>();
            CreateMap<TeacherModel, EditTeacherDto>();
            CreateMap<TeacherLoadDto, TeacherLoadModel>();
        }
    }
}
