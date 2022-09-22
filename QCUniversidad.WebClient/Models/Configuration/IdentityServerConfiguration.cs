using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.WebClient.Models.Configuration
{
    public class IdentityServerConfiguration
    {
        public string Address { get; set; }
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public string Scope { get; set; }
    }
}
