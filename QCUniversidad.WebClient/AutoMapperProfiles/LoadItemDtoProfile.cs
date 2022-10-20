using AutoMapper;
using QCUniversidad.Api.Shared.Dtos.LoadItem;
using QCUniversidad.WebClient.Models.LoadDistribution;
using QCUniversidad.WebClient.Models.LoadItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.AutoMapperProfiles;
public class LoadItemDtoProfile : Profile
{
	public LoadItemDtoProfile()
	{
		CreateMap<CreateLoadItemModel, NewLoadItemDto>();
		CreateMap<LoadItemDto, LoadItemModel>();
		CreateMap<SimpleLoadItemDto, LoadItemModel>();
	}
}
