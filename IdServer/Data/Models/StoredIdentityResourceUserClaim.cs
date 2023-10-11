using System;

namespace IdServer.Data.Models;

public record StoredIdentityResourceUserClaim
{
    public Guid Id { get; set; }
    public string UserClaim { get; set; }
    public Guid StoredIdentityResourceId { get; set; }
    public StoredIdentityResource StoredIdentityResource { get; set; }
}
