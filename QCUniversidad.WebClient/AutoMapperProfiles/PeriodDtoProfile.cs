using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.Period;
using QCUniversidad.WebClient.Models.Periods;

namespace QCUniversidad.WebClient.AutoMapperProfiles;

public class PeriodDtoProfile : Profile
{
    public PeriodDtoProfile()
    {
        _ = CreateMap<PeriodDto, PeriodModel>();
        _ = CreateMap<PeriodModel, PeriodDto>();
        _ = CreateMap<PeriodModel, NewPeriodDto>();
        _ = CreateMap<CreatePeriodModel, PeriodModel>();
        _ = CreateMap<EditPeriodDto, PeriodModel>();
        _ = CreateMap<PeriodModel, EditPeriodDto>();
        _ = CreateMap<SimplePeriodDto, PeriodModel>();
    }
}
