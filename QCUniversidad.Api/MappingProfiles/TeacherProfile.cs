using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Teacher;

namespace QCUniversidad.Api.MappingProfiles;

public class TeacherProfile : Profile
{
    public TeacherProfile()
    {
        _ = CreateMap<TeacherModel, TeacherDto>();
        _ = CreateMap<TeacherDto, TeacherModel>();
        _ = CreateMap<NewTeacherDto, TeacherModel>();
        _ = CreateMap<TeacherModel, NewTeacherDto>();
        _ = CreateMap<EditTeacherDto, TeacherModel>();
        _ = CreateMap<TeacherModel, EditTeacherDto>();
        _ = CreateMap<TeacherModel, SimpleTeacherDto>();
        _ = CreateMap<TeacherModel, BirthdayTeacherDto>().AfterMap((model, dto) => dto.IsBirthdayToday = model.Birthday == DateTime.Today);
    }
}