using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Subject;

namespace QCUniversidad.Api.MappingProfiles;

public class SubjectProfile : Profile
{
    public SubjectProfile()
    {
        _ = CreateMap<SubjectModel, SubjectDto>();
        _ = CreateMap<SubjectDto, SubjectModel>();
        _ = CreateMap<SubjectModel, NewSubjectDto>();
        _ = CreateMap<NewSubjectDto, SubjectModel>();
        _ = CreateMap<SubjectModel, EditSubjectDto>();
        _ = CreateMap<EditSubjectDto, SubjectModel>();
        _ = CreateMap<PeriodSubjectModel, SimplePeriodSubjectDto>();
        _ = CreateMap<PeriodSubjectModel, PeriodSubjectDto>();
        _ = CreateMap<NewPeriodSubjectDto, PeriodSubjectModel>();
        _ = CreateMap<EditPeriodSubjectDto, PeriodSubjectModel>();
    }
}
