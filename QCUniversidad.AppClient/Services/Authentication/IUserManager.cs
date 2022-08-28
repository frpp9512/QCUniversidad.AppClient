using QCUniversidad.AppClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.Services.Authentication
{
    public interface IUserManager
    {
        event UserAuthenticationEventHandler AuthenticationEvent;
        bool IsAuthenticated { get; }
        LoggedUser LoggedUser { get; }
        Task LoginAsync(Action successfullLoginCallback, Action<string> failedLoginCallback);
        Task LogoutAsync(Action successfullLogoutCallback, Action<string> failedLogoutCallback);
    }
}
