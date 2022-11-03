using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Subject;

namespace QCUniversidad.Api.MappingProfiles;

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
        CreateMap<PeriodSubjectModel, SimplePeriodSubjectDto>();
        CreateMap<PeriodSubjectModel, PeriodSubjectDto>();
        CreateMap<NewPeriodSubjectDto, PeriodSubjectModel>();
        CreateMap<EditPeriodSubjectDto, PeriodSubjectModel>();
    }
}
