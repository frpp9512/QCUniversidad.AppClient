using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdServer.Data.Context;
using IdServer.Data.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdServer.Services;

public class ResourceStore : IResourceStore
{
    private readonly IdServerDataContext _dataContext;

    public ResourceStore(IdServerDataContext dataContext) => _dataContext = dataContext;

    public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
    {
        var storedResources = new List<StoredApiResource>();
        foreach (var name in apiResourceNames)
        {
            var founded = await _dataContext.ApiResources.Include(x => x.UserClaims)
                                                         .Include(x => x.Scopes)
                                                         .FirstOrDefaultAsync(x => x.Name == name);
            if (founded is not null)
            {
                storedResources.Add(founded);
            }
        }

        return storedResources.Select(x => new ApiResource
        {
            Name = x.Name,
            DisplayName = x.DisplayName,
            UserClaims = x.UserClaims.Select(y => y.UserClaim).ToList(),
            Scopes = x.Scopes.Select(y => y.Scope).ToList()
        });
    }

    public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames) => await FindApiResourcesByNameAsync(scopeNames);//var storedResources = new List<StoredApiResource>();//foreach (var name in scopeNames)//{//    var founded = await _dataContext.ApiResources.Include(x => x.Scopes)//                                                 .Include(x => x.UserClaims)//                                                 .FirstOrDefaultAsync(x => x.Scopes.Any(y => y.Scope == name));//    if (founded is not null)//    {//        storedResources.Add(founded);//    }//}//return storedResources.Select(x => new ApiResource//{//    Name = x.Name,//    DisplayName = x.DisplayName,//    UserClaims = x.UserClaims.Select(y => y.UserClaim).ToList(),//    Scopes = x.Scopes.Select(y => y.Scope).ToList()//});

    public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
    {
        var storedApiScopes = new List<StoredApiScope>();
        foreach (var name in scopeNames)
        {
            var founded = await _dataContext.ApiScopes.Include(x => x.UserClaims)
                                                              .FirstOrDefaultAsync(x => x.Name == name);
            if (founded is not null)
            {
                storedApiScopes.Add(founded);
            }
        }

        return GetApiScopesFromStored(storedApiScopes);
    }

    private IEnumerable<ApiScope> GetApiScopesFromStored(List<StoredApiScope> storedApiScopes)
        => storedApiScopes.Select(x => new ApiScope
        {
            Name = x.Name,
            DisplayName = x.DisplayName,
            Emphasize = x.Emphasize,
            Required = x.Required,
            UserClaims = x.UserClaims.Select(y => y.UserClaim).ToList(),
        });

    public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
    {
        var storedIdResources = new List<StoredIdentityResource>();
        foreach (var name in scopeNames)
        {
            var founded = await _dataContext.IdentityResources.Include(x => x.UserClaims)
                                                              .FirstOrDefaultAsync(x => x.Name == name);
            if (founded is not null)
            {
                storedIdResources.Add(founded);
            }
        }

        return GetIdentityResourcesFromStored(storedIdResources);
    }

    private IEnumerable<IdentityResource> GetIdentityResourcesFromStored(IEnumerable<StoredIdentityResource> storedIdentities)
        => storedIdentities.Select(x => new IdentityResource
        {
            Name = x.Name,
            DisplayName = x.DisplayName,
            Description = x.Description,
            Emphasize = x.Emphasize,
            Required = x.Required,
            UserClaims = x.UserClaims.Select(x => x.UserClaim).ToList(),
        });

    private IEnumerable<ApiResource> GetApiResourcesFromStored(IEnumerable<StoredApiResource> storedApiResources)
        => storedApiResources.Select(x => new ApiResource
        {
            Name = x.Name,
            DisplayName = x.DisplayName,
            UserClaims = x.UserClaims.Select(y => y.UserClaim).ToList(),
            Scopes = x.Scopes.Select(y => y.Scope).ToList()
        });

    public async Task<Resources> GetAllResourcesAsync()
    {
        var apiresources = GetApiResourcesFromStored(await _dataContext.ApiResources.Include(x => x.Scopes)
                                                          .Include(x => x.UserClaims)
                                                          .ToListAsync());
        var identityresources = GetIdentityResourcesFromStored(await _dataContext.IdentityResources.Include(x => x.UserClaims)
                                                                    .ToListAsync());

        return new Resources(identityresources, apiresources, null);
    }
}
