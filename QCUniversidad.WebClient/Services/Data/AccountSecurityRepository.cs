using Microsoft.EntityFrameworkCore;
using QCUniversidad.WebClient.Data.Contexts;
using SmartB1t.Security;
using SmartB1t.Security.WebSecurity.Local.Interfaces;
using SmartB1t.Security.WebSecurity.Local.Models;

namespace QCUniversidad.WebClient.Services.Data;

public class UserNotFoundException : Exception { }

public class AccountSecurityRepository : IAccountSecurityRepository
{
    private readonly WebDataContext _dataContext;

    public AccountSecurityRepository(WebDataContext dataContext) => _dataContext = dataContext;

    public async Task AssignRoleToUserAsync(User user, Role role)
    {
        if (await CheckIfUserHasRole(user, role))
        {
            return;
        }

        var userRole = new UserRole { RoleId = role.Id, UserId = user.Id };
        _ = await _dataContext.AddAsync(userRole);
        _ = await _dataContext.SaveChangesAsync();
    }

    public async Task<bool> AnyUserAsync() => await _dataContext.Users.AnyAsync();

    public async Task<bool> AuthenticateUser(User user, string password)
    {
        if (!user.Active || user.PermanentDeactivation)
        {
            return false;
        }

        var userSecret = await _dataContext.Secrets
                    .Where(us => us.UserId == user.Id)
                    .FirstOrDefaultAsync();
        return AuthUtil.TryAuth(user.Email, ref password, SecurityUtil.SecureString(userSecret.Password));
    }

    public async Task<bool> CheckIfUserHasRole(User user, Role role)
        => await _dataContext.UserRoles
                             .Where(ur => ur.UserId == user.Id && ur.RoleId == role.Id)
                             .AnyAsync();

    public async Task CreateRoleAsync(Role role)
    {
        if (role is null)
        {
            return;
        }

        _ = await _dataContext.AddAsync(role);
        _ = await _dataContext.SaveChangesAsync();
    }

    public async Task CreateUserAsync(User user, string password)
    {
        if (user is null)
        {
            return;
        }

        var secrets = new UserSecrets
        {
            Password = SecurityUtil.B64HashEncrypt(user.Email, password)
        };
        user.Secrets = secrets;
        _ = await _dataContext.AddAsync(user);
        _ = await _dataContext.SaveChangesAsync();
    }

    public async Task<Role> GetRoleAsync(Guid id)
        => await _dataContext.Roles.FindAsync(id);

    public async Task<Role> GetRoleAsync(string name)
        => await _dataContext.Roles.FirstOrDefaultAsync(r => r.Name == name);

    public async Task<IEnumerable<Role>> GetRolesAsync()
        => await _dataContext.Roles.Where(r => r.Active).ToListAsync();

    public async Task<User> GetUserAsync(Guid id, bool loadUserRoles = false, bool includePermanentDeactivated = false)
    {
        var userQuery = _dataContext.Users.Where(u => u.Id == id);
        if (!includePermanentDeactivated)
        {
            _ = userQuery.Where(u => !u.PermanentDeactivation);
        }

        userQuery = userQuery.Include(u => u.ExtraClaims);
        if (loadUserRoles)
        {
            userQuery = userQuery.Include(u => u.Roles).ThenInclude(r => r.Role);
        }

        return await userQuery.FirstOrDefaultAsync() ?? throw new UserNotFoundException();
    }

    public async Task<User> GetUserAsync(string email, bool loadUserRoles = false, bool includePermanentDeactivated = false)
    {
        var userQuery = _dataContext.Users.Where(u => u.Email == email);
        if (!includePermanentDeactivated)
        {
            _ = userQuery.Where(u => !u.PermanentDeactivation);
        }

        userQuery = userQuery.Include(u => u.ExtraClaims);
        if (loadUserRoles)
        {
            userQuery = userQuery.Include(u => u.Roles).ThenInclude(r => r.Role);
        }

        return await userQuery.FirstOrDefaultAsync() ?? throw new UserNotFoundException();
    }

    public async Task<IEnumerable<User>> GetUsersAsync(bool loadUserRoles = false)
    {
        var usersQuery = _dataContext.Users.Where(u => !u.PermanentDeactivation);

        usersQuery = usersQuery.Include(u => u.ExtraClaims);
        if (loadUserRoles)
        {
            usersQuery = usersQuery.Include(u => u.Roles).ThenInclude(r => r.Role);
        }

        usersQuery = usersQuery.OrderByDescending(u => u.Roles.Count());
        return await usersQuery.ToListAsync();
    }

    public Task<IEnumerable<User>> GetUsersAsync(int startIndex, int count, bool loadUserRoles = false)
    {
        var usersQuery = _dataContext.Users.Where(u => !u.PermanentDeactivation)
                                           .Skip(startIndex)
                                           .Take(count);
        usersQuery = usersQuery.Include(u => u.ExtraClaims);
        if (loadUserRoles)
        {
            usersQuery = usersQuery.Include(u => u.Roles).ThenInclude(r => r.Role);
        }

        usersQuery = usersQuery.OrderByDescending(u => u.Roles.Count());
        return Task.FromResult(usersQuery.AsEnumerable());
    }

    public async Task RemoveRoleAsync(Role role)
    {
        if (role is null)
        {
            return;
        }

        _ = _dataContext.Remove(role);
        _ = await _dataContext.SaveChangesAsync();
    }

    public async Task RemoveRoleFromUserAsync(User user, Role role)
    {
        if (user is null || role is null)
        {
            return;
        }

        var userRole = await _dataContext.UserRoles.Where(ur => ur.UserId == user.Id && ur.RoleId == role.Id)
                                                   .FirstOrDefaultAsync();

        if (userRole is not null)
        {
            _ = _dataContext.Remove(userRole);
            _ = await _dataContext.SaveChangesAsync();
        }
    }

    public async Task RemoveUserAsync(User user)
    {
        if (user is null)
        {
            return;
        }

        _ = _dataContext.Remove(user);
        _ = await _dataContext.SaveChangesAsync();
    }

    public async Task RemoveExtraClaimAsync(ExtraClaim extraClaim)
    {
        _ = _dataContext.ExtraClaims.Remove(extraClaim);
        _ = await _dataContext.SaveChangesAsync();
    }

    public async Task SetUserPasswordAsync(User user, string newPassword)
    {
        var secrets = await _dataContext.Secrets.FirstOrDefaultAsync(s => s.UserId == user.Id);
        if (secrets is not null)
        {
            secrets.Password = newPassword;
            _ = _dataContext.Update(secrets);
            return;
        }

        secrets = new UserSecrets
        {
            Password = SecurityUtil.B64HashEncrypt(user.Email, newPassword),
            User = user
        };
        _ = _dataContext.Add(secrets);
    }

    public async Task UpdateRoleAsync(Role role)
    {
        if (role is null)
        {
            return;
        }

        _ = _dataContext.Update(role);
        _ = await _dataContext.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        if (user is null)
        {
            return;
        }

        _ = _dataContext.Update(user);
        _ = await _dataContext.SaveChangesAsync();
    }

    public async Task<int> GetUsersCount(bool includeInactive = true)
        => await _dataContext.Users.CountAsync(u => !u.PermanentDeactivation && (u.Active || includeInactive));
}
