using IdServer.Data.Context;
using IdServer.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdServer.Services
{
    public class UserStore : IUserStore
    {
        private readonly IdServerDataContext _dataContext;

        public UserStore(IdServerDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public StoredUser FindByUsername(string username)
        {
            var user = _dataContext.StoredUsers.Include(x => x.Claims).FirstOrDefault(x => x.Username == username);
            if (user is not null)
            {
                return user;
            }
            return null;
        }

        public List<StoredUserClientRoles> GetRoles(Guid userId, Guid storedClientId)
        {
            var roles = _dataContext.StoredUserClientRoles.Where(x => x.StoredUserId == userId && x.StoredClientId == storedClientId).ToList();
            return roles;
        }

        public bool ValidateCredentials(string username, string password)
        {
            var user = _dataContext.StoredUsers.Include(x => x.Secrets).FirstOrDefault(x => x.Username == username);
            return user is not null ? user.Secrets.Password == password : false;
        }
    }
}