using AutoMapper;
using QCUniversidad.WebClient.Models.Accounts;
using SmartB1t.Security.WebSecurity.Local.Models;

namespace QCUniversidad.WebClient.AutoMapperProfiles;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        _ = CreateMap<User, UserViewModel>();
    }
}
