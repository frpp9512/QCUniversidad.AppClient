using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.Subject;
using QCUniversidad.WebClient.Models.Subjects;

namespace QCUniversidad.WebClient.AutoMapperProfiles;

public class SubjectDtoProfile : Profile
{
    public SubjectDtoProfile()
    {
        _ = CreateMap<SubjectDto, SubjectModel>();
        _ = CreateMap<SubjectModel, SubjectDto>();
        _ = CreateMap<SubjectModel, NewSubjectDto>();
        _ = CreateMap<NewSubjectDto, SubjectModel>();
        _ = CreateMap<SubjectModel, EditSubjectDto>();
        _ = CreateMap<EditSubjectDto, SubjectModel>();
        _ = CreateMap<SubjectModel, CreateSubjectModel>();
        _ = CreateMap<CreateSubjectModel, SubjectModel>();
        _ = CreateMap<SubjectModel, EditSubjectModel>().ForMember(e => e.DisciplineName, opt => opt.MapFrom(s => s.Discipline.Name));
        _ = CreateMap<EditSubjectModel, SubjectModel>();
        _ = CreateMap<CreatePeriodSubjectModel, PeriodSubjectModel>();
        _ = CreateMap<PeriodSubjectModel, NewPeriodSubjectDto>();
        _ = CreateMap<PeriodSubjectModel, EditPeriodSubjectDto>();
        _ = CreateMap<PeriodSubjectDto, PeriodSubjectModel>();
    }
}
