using System;

namespace IdServer.Data.Models;

public record StoredApiResourceScope
{
    public Guid Id { get; set; }
    public string Scope { get; set; }
    public Guid StoredApiResourceId { get; set; }
    public StoredApiResource StoredApiResource { get; set; }
}
