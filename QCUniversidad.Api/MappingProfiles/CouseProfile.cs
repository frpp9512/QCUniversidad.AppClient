using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Course;

namespace QCUniversidad.Api.MappingProfiles;

public class CouseProfile : Profile
{
    public CouseProfile()
    {
        _ = CreateMap<CourseModel, CourseDto>();
        _ = CreateMap<CourseDto, CourseModel>();
        _ = CreateMap<CourseModel, NewCourseDto>();
        _ = CreateMap<NewCourseDto, CourseModel>();
        _ = CreateMap<EditCourseDto, CourseModel>();
        _ = CreateMap<CourseModel, SimpleCourseDto>();
    }
}
