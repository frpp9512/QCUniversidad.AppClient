using System;
using System.Collections.Generic;

namespace IdServer.Data.Models;

public record StoredIdentityResource
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public bool Required { get; set; }
    public bool Emphasize { get; set; } = false;
    public bool ShowInDiscoveryDocument { get; set; }
    public IEnumerable<StoredIdentityResourceUserClaim> UserClaims { get; set; }
}
