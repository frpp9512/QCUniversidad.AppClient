using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using SmartB1t.Security.WebSecurity.Local.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SmartB1t.Security.Extensions.AspNetCore;

public static class UserAuthExtensions
{
    public static async Task SignInAsync(this User user, HttpContext context, string scheme, bool rememberSession)
    {
        await context.SignInAsync(scheme,
                                         GenerateClaimsPrincipal(user, scheme),
                                         new AuthenticationProperties { IsPersistent = rememberSession });
    }

    private static ClaimsPrincipal GenerateClaimsPrincipal(User user, string scheme)
    {
        List<Claim> claims =
        [
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Name, user.Fullname),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        ];
        if (user.Roles?.Any() == true)
        {
            claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Role.Name)));
        }

        if (user.ExtraClaims?.Any() == true)
        {
            claims.AddRange(user.ExtraClaims.Select(c => new Claim(c.Type, c.Value)));
        }

        ClaimsIdentity claimsIdentity = new(claims, scheme);
        ClaimsPrincipal claimsPrincipal = new(claimsIdentity);
        return claimsPrincipal;
    }

    public static Guid GetId(this ClaimsPrincipal user)
    {
        return new(user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}