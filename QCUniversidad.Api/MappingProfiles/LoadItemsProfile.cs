using AutoMapper;
using QCUniversidad.Api.Data.Models;
using QCUniversidad.Api.Shared.Dtos.LoadItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.Api.MappingProfiles;

public class LoadItemsProfile : Profile
{
	public LoadItemsProfile()
	{
		CreateMap<LoadItemModel, LoadItemDto>();
	}
}
