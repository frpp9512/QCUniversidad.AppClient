using QCUniversidad.AppClient.Services.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.Attributes
{
    public class AuthRequired : Attribute
    {
        public AuthRequired(IUserManager userManager)
        {
            UserManager = userManager;
        }

        public IUserManager UserManager { get; }
    }
}
