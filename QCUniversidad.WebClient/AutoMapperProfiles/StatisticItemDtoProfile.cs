using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.Statistics;
using QCUniversidad.WebClient.Models.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.AutoMapperProfiles;

public class StatisticItemDtoProfile : Profile
{
	public StatisticItemDtoProfile()
	{
		CreateMap<StatisticItemDto, StatisticItemModel>();
	}
}
