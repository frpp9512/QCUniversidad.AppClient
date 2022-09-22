using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Teacher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.MappingProfiles
{
    public class TeacherProfile : Profile
    {
        public TeacherProfile()
        {
            CreateMap<TeacherModel, TeacherDto>();
            CreateMap<TeacherDto, TeacherModel>();
            CreateMap<NewTeacherDto, TeacherModel>();
            CreateMap<TeacherModel, NewTeacherDto>();
            CreateMap<EditTeacherDto, TeacherModel>();
            CreateMap<TeacherModel, EditTeacherDto>();
        }
    }
}
