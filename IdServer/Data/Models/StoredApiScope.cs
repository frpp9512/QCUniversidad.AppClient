using System;
using System.Collections.Generic;

namespace IdServer.Data.Models;

public class StoredApiScope
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
    public bool Required { get; set; }
    public bool Emphasize { get; set; } = false;
    public IEnumerable<StoredApiScopeUserClaim> UserClaims { get; set; }
}
