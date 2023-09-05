using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdServer.Data.Models
{
    public class StoredUserSecrets
    {
        public Guid Id { get; set; }
        public string Password { get; set; }
        public Guid StoredUserId { get; set; }
        public StoredUser StoredUser { get; set; }
    }
}
