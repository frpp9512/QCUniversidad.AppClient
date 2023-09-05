using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdServer.Data.Models
{
    public record StoredIdentityResourceUserClaim
    {
        public Guid Id { get; set; }
        public string UserClaim { get; set; }
        public Guid StoredIdentityResourceId { get; set; }
        public StoredIdentityResource StoredIdentityResource { get; set; }
    }
}
