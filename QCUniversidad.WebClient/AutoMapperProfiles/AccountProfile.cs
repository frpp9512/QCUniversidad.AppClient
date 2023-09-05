using AutoMapper;
using QCUniversidad.WebClient.Models.Accounts;
using SmartB1t.Security.WebSecurity.Local;

namespace QCUniversidad.WebClient.AutoMapperProfiles;

public class AccountProfile : Profile
{
    public AccountProfile() => CreateMap<User, UserViewModel>();
}
