using System;

namespace IdServer.Data.Models;

public class StoredApiScopeUserClaim
{
    public Guid Id { get; set; }
    public string UserClaim { get; set; }
    public Guid StoredApiScopeId { get; set; }
    public StoredApiScope StoredApiScope { get; set; }
}