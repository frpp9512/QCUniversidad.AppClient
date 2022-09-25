using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.Subject;
using QCUniversidad.WebClient.Models.Subjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.AutoMapperProfiles;

public class SubjectDtoProfile : Profile
{
    public SubjectDtoProfile()
    {
        CreateMap<SubjectDto, SubjectModel>();
        CreateMap<SubjectModel, SubjectDto>();
        CreateMap<SubjectModel, NewSubjectDto>();
        CreateMap<NewSubjectDto, SubjectModel>();
        CreateMap<SubjectModel, EditSubjectDto>();
        CreateMap<EditSubjectDto, SubjectModel>();
        CreateMap<SubjectModel, CreateSubjectModel>();
        CreateMap<CreateSubjectModel, SubjectModel>();
        CreateMap<SubjectModel, EditSubjectModel>().ForMember(e => e.DisciplineName, opt => opt.MapFrom(s => s.Discipline.Name));
        CreateMap<EditSubjectModel, SubjectModel>();
    }
}
