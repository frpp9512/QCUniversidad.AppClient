using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.Period;

namespace QCUniversidad.Api.MappingProfiles;

public class PeriodProfile : Profile
{
    public PeriodProfile()
    {
        _ = CreateMap<PeriodModel, PeriodDto>();
        _ = CreateMap<PeriodDto, PeriodModel>();
        _ = CreateMap<NewPeriodDto, PeriodModel>();
        _ = CreateMap<EditPeriodDto, PeriodModel>();
        _ = CreateMap<PeriodModel, EditPeriodDto>();
        _ = CreateMap<PeriodModel, SimplePeriodDto>();
    }
}
