using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Teacher;

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
            CreateMap<TeacherModel, SimpleTeacherDto>();
        }
    }
}
