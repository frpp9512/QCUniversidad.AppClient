using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdServer.Data.Models
{
    public class StoredClientAllowedScope
    {
        public Guid Id { get; set; }
        public string AllowedScope { get; set; }
        public Guid ClientId { get; set; }
        public StoredClient Client { get; set; }
    }
}
