using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdServer.Data.Models
{
    public class StoredUserClientRoles
    {
        public Guid Id { get; set; }
        public string Role { get; set; }
        public Guid StoredClientId { get; set; }
        public StoredClient StoredClient { get; set; }
        public Guid StoredUserId { get; set; }
        public StoredUser StoredUser { get; set; }
    }
}
