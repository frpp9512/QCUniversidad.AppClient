using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using SmartB1t.Security.WebSecurity.Local;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SmartB1t.Security.Extensions.AspNetCore
{
    public static class UserAuthExtensions
    {
        public static async Task SignInAsync(this User user, HttpContext context, string scheme, bool rememberSession) 
            => await context.SignInAsync(scheme,
                                         GenerateClaimsPrincipal(user, scheme),
                                         new AuthenticationProperties { IsPersistent = rememberSession });

        private static ClaimsPrincipal GenerateClaimsPrincipal(User user, string scheme)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Fullname),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };
            if (user.Roles?.Any() == true)
            {
                claims.AddRange(user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Role.Name)));
            }
            if (user.ExtraClaims?.Any() == true)
            {
                claims.AddRange(user.ExtraClaims.Select(c => new Claim(c.Type, c.Value)));
            }
            var claimsIdentity = new ClaimsIdentity(claims, scheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return claimsPrincipal;
        }

        public static Guid GetId(this ClaimsPrincipal user)
            => new(user.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
    }
}