using System;
using System.Collections.Generic;

namespace IdServer.Data.Models;

public record StoredApiResource
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public List<StoredApiResourceUserClaim> UserClaims { get; set; }
    public List<StoredApiResourceScope> Scopes { get; set; }
}
