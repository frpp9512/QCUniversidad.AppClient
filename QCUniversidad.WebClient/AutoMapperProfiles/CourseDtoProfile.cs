using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.Course;
using QCUniversidad.WebClient.Models.Courses;
using QCUniversidad.WebClient.Models.Planning;

namespace QCUniversidad.WebClient.AutoMapperProfiles;

public class CourseDtoProfile : Profile
{
    public CourseDtoProfile()
    {
        _ = CreateMap<CourseDto, CourseModel>();
        _ = CreateMap<CourseModel, CourseDto>();
        _ = CreateMap<CreateCourseModel, NewCourseDto>();
        _ = CreateMap<NewCourseDto, CreateCourseModel>();
        _ = CreateMap<CourseModel, EditCourseModel>();
        _ = CreateMap<EditCourseModel, EditCourseDto>();
        _ = CreateMap<SimpleCourseDto, CourseModel>();
        _ = CreateMap<CoursePeriodPlanningInfoDto, CoursePeriodPlanningInfoModel>();
    }
}
