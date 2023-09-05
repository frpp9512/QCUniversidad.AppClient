using System;

namespace IdServer.Data.Models
{
    public class StoredClientPostLogoutRedirectUris
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public Guid StoredClientId { get; set; }
        public StoredClient StoredClient { get; set; }
    }
}