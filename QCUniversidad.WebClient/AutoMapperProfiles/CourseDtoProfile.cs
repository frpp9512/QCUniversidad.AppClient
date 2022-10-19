using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.Course;
using QCUniversidad.WebClient.Models.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.AutoMapperProfiles
{
    public class CourseDtoProfile : Profile
    {
        public CourseDtoProfile()
        {
            CreateMap<CourseDto, CourseModel>();
            CreateMap<CourseModel, CourseDto>();
            CreateMap<CreateCourseModel, NewCourseDto>();
            CreateMap<NewCourseDto, CreateCourseModel>();
            CreateMap<CourseModel, EditCourseModel>();
            CreateMap<EditCourseModel, EditCourseDto>();
        }
    }
}
