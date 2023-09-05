using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdServer.Data.Models
{
    public record StoredUserClaim
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; } = ClaimValueTypes.String;
        public string Issuer { get; set; }
        public Guid UserId { get; set; }
        public StoredUser StoredUser { get; set; }
    }
}
