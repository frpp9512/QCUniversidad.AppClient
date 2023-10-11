using System;
using System.Collections.Generic;

namespace IdServer.Data.Models;

public class StoredClient
{
    public Guid Id { get; set; }
    public string ClientId { get; set; }
    public List<StoredClientRedirectUris> RedirectUris { get; set; }
    public List<StoredClientPostLogoutRedirectUris> PostLogoutRedirectUris { get; set; }
    public List<StoredClientSecret> Secrets { get; set; }
    public List<StoredClientGrantType> GrantTypes { get; set; }
    public List<StoredClientAllowedScope> AllowedScopes { get; set; }
    public bool AlwaysIncludeUserClaimsInIdToken { get; set; }
    public bool RequireConsent { get; set; }
    public List<StoredUserClientRoles> StoredUserClientRoles { get; set; }
}
