using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdServer.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdServer.Services;

public class ClientStore : IClientStore
{
    private readonly IdServerDataContext _dataContext;

    public ClientStore(IdServerDataContext dataContext) => _dataContext = dataContext;

    public async Task<Client> FindClientByIdAsync(string clientId)
    {
        var storedClient = await _dataContext.Clients.Where(x => x.ClientId == clientId)
                                                .Include(x => x.RedirectUris)
                                                .Include(x => x.GrantTypes)
                                                .Include(x => x.AllowedScopes)
                                                .Include(x => x.Secrets)
                                                .Include(x => x.PostLogoutRedirectUris)
                                                .FirstOrDefaultAsync();

        var result = storedClient is not null
            ? new Client
            {
                ClientId = storedClient.ClientId,
                RedirectUris = storedClient.RedirectUris?.Select(x => x.Url).ToArray(),
                AllowedGrantTypes = storedClient.GrantTypes?.Select(x => x.GrantType).ToArray(),
                AllowedScopes = storedClient.AllowedScopes?.Select(x => x.AllowedScope).ToArray(),
                ClientSecrets = storedClient.Secrets?.Select(x => new Secret(x.Value, x.Description, x.Expiration)).ToList(),
                AlwaysIncludeUserClaimsInIdToken = storedClient.AlwaysIncludeUserClaimsInIdToken,
                RequireConsent = storedClient.RequireConsent,
                PostLogoutRedirectUris = storedClient.PostLogoutRedirectUris?.Select(x => x.Url).ToArray(),
                Claims = new List<ClientClaim> { new ClientClaim { Type = "stored_id", Value = storedClient.Id.ToString() } }
            }
            : throw new Exception();

        return result;
    }
}