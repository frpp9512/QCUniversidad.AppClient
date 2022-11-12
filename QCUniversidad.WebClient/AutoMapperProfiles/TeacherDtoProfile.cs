using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.Teacher;
using QCUniversidad.WebClient.Models.Teachers;

namespace QCUniversidad.WebClient.AutoMapperProfiles;

public class TeacherDtoProfile : Profile
{
    public TeacherDtoProfile()
    {
        _ = CreateMap<TeacherModel, TeacherDto>();
        _ = CreateMap<TeacherDto, TeacherModel>();
        _ = CreateMap<NewTeacherDto, TeacherModel>();
        _ = CreateMap<TeacherModel, NewTeacherDto>();
        _ = CreateMap<EditTeacherDto, TeacherModel>();
        _ = CreateMap<TeacherModel, EditTeacherDto>();
        _ = CreateMap<TeacherLoadDto, TeacherLoadModel>();
        _ = CreateMap<BirthdayTeacherDto, BirthdayTeacherModel>();
    }
}
