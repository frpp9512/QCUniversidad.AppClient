using IdServer.Data.Models;
using System;
using System.Collections.Generic;

namespace IdServer.Services;

public interface IUserStore
{
    StoredUser FindByUsername(string username);
    bool ValidateCredentials(string username, string password);
    List<StoredUserClientRoles> GetRoles(Guid userId, Guid storedClientId);
}