using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdServer.Data.Models
{
    public class StoredClientRedirectUris
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public Guid StoredClientId { get; set; }
        public StoredClient StoredClient { get; set; }
    }
}
