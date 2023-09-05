using IdentityModel;
using IdServer.Data.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdServer.Extensions
{
    public static class UserExtensions
    {
        public static IEnumerable<Claim> GetProfileClaims(this StoredUser user, List<Claim> additionalClaims = null)
        {
            var claims = new List<Claim>
            {
                new Claim("website", user.Website ?? ""),
                new Claim("name", user.Name ?? ""),
                new Claim("birthdate", JsonConvert.SerializeObject(user.BirthDate), ClaimValueTypes.Date),
                new Claim("family_name", user.FamilyName ?? ""),
                new Claim("picture", user.PictureUrl ?? ""),
                new Claim("locale", user.Locale ?? ""),
                new Claim("preferred_username", user.PreferredUsername ?? ""),
                new Claim("middle_name", user.MiddleName ?? ""),
                new Claim("gender", user.Gender ?? ""),
                new Claim("updated_at", user.UpdatedAt ?? ""),
                new Claim("given_name", user.GivenName ?? ""),
                new Claim("zoneinfo", user.ZoneInfo ?? ""),
                new Claim("nickname", user.Nickname ?? ""),
            };
            if (additionalClaims is not null)
            {
                claims.AddRange(additionalClaims);
            }
            return claims;
        }

        public static Claim GetRoleClaim(this StoredUserClientRoles role)
            => new Claim(JwtClaimTypes.Role, role.Role);
    }
}
