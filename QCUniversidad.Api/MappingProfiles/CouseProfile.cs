using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Course;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.MappingProfiles
{
    public class CouseProfile : Profile
    {
        public CouseProfile()
        {
            CreateMap<CourseModel, CourseDto>();
            CreateMap<CourseDto, CourseModel>();
            CreateMap<CourseModel, NewCourseDto>();
            CreateMap<NewCourseDto, CourseModel>();
            CreateMap<EditCourseDto, CourseModel>();
            CreateMap<CourseModel, SimpleCourseDto>();
        }
    }
}
