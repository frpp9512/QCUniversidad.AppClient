using System;

namespace IdServer.Data.Models;

public record StoredClientSecret
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public string Value { get; set; }
    public DateTime? Expiration { get; set; }
    public Guid ClientId { get; set; }
    public StoredClient Client { get; set; }
}