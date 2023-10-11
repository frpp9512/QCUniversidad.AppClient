using System;

namespace IdServer.Data.Models;

public class StoredClientAllowedScope
{
    public Guid Id { get; set; }
    public string AllowedScope { get; set; }
    public Guid ClientId { get; set; }
    public StoredClient Client { get; set; }
}
