using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdServer.Data.Models
{
    public record StoredApiResourceScope
    {
        public Guid Id { get; set; }
        public string Scope { get; set; }
        public Guid StoredApiResourceId { get; set; }
        public StoredApiResource StoredApiResource { get; set; }
    }
}
