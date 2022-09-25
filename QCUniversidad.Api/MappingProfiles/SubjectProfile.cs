using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.MappingProfiles
{
    public class SubjectProfile : Profile
    {
        public SubjectProfile()
        {
            CreateMap<SubjectModel, SubjectDto>();
            CreateMap<SubjectDto, SubjectModel>();
            CreateMap<SubjectModel, NewSubjectDto>();
            CreateMap<NewSubjectDto, SubjectModel>();
            CreateMap<SubjectModel, EditSubjectDto>();
            CreateMap<EditSubjectDto, SubjectModel>();
        }
    }
}
