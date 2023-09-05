using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdServer.Data.Models
{
    public record StoredApiResourceUserClaim
    {
        public Guid Id { get; set; }
        public string UserClaim { get; set; }
        public Guid ApiResourceId { get; set; }
        public StoredApiResource StoredApiResource { get; set; }
    }
}
