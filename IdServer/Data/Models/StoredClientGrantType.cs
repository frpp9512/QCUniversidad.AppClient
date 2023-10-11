using System;

namespace IdServer.Data.Models;

public class StoredClientGrantType
{
    public Guid Id { get; set; }
    public string GrantType { get; set; }
    public Guid ClientId { get; set; }
    public StoredClient Client { get; set; }
}
