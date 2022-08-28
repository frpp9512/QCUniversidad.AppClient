using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace QCUniversidad.AppClient.Models
{
    public class LoggedUser
    {
        private ClaimsPrincipal UserPrincipal { get; set; }

        public Guid Id { get; set; }

        public string Username { get; set; }

        public string DisplayName => $"{GivenName} {MiddleName} {FamilyName}";

        /// <summary>
        /// Claim type: name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Claim type: given_name
        /// </summary>
        public string GivenName { get; set; }

        /// <summary>
        /// Claim type: middle_name
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Claim type: family_name
        /// </summary>
        public string FamilyName { get; set; }

        /// <summary>
        /// Claim type: birthdate
        /// </summary>
        public DateTimeOffset BirthDate { get; set; }

        /// <summary>
        /// Claim type: profile
        /// </summary>
        public string ProfileUrl { get; set; }

        /// <summary>
        /// Claim type: picture
        /// </summary>
        public string PictureUrl { get; set; }

        /// <summary>
        /// Claim type: locale
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// Claim type: gender
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Claim type: updated_at
        /// </summary>
        public string UpdatedAt { get; set; }

        /// <summary>
        /// Claim type: nickname
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Claim type: zoneinfo
        /// </summary>
        public string ZoneInfo { get; set; }

        /// <summary>
        /// Claim type: preferred_username
        /// </summary>
        public string PreferredUsername { get; set; }

        /// <summary>
        /// Claim type: website
        /// </summary>
        public string Website { get; set; }

        public LoggedUser(ClaimsPrincipal userPrincipal)
        {
            UserPrincipal = userPrincipal;
            SetUserInfo();
        }

        private void SetUserInfo()
        {
            Id = new Guid(UserPrincipal.Claims.FirstOrDefault(c => c.Type == "sid")?.Value);
            Name = UserPrincipal.Claims.FirstOrDefault(c => c.Type == "name")?.Value;
            GivenName = UserPrincipal.Claims.FirstOrDefault(c => c.Type == "given_name")?.Value;
            MiddleName = UserPrincipal.Claims.FirstOrDefault(c => c.Type == "middle_name")?.Value;
            FamilyName = UserPrincipal.Claims.FirstOrDefault(c => c.Type == "family_name")?.Value;
            DateTimeOffset.TryParse(UserPrincipal.Claims.FirstOrDefault(c => c.Type == "birthdate")?.Value, out DateTimeOffset result);
            BirthDate = result;
            ProfileUrl = UserPrincipal.Claims.FirstOrDefault(c => c.Type == "profile")?.Value;
            PictureUrl = UserPrincipal.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;
            Locale = UserPrincipal.Claims.FirstOrDefault(c => c.Type == "locale")?.Value;
            Gender = UserPrincipal.Claims.FirstOrDefault(c => c.Type == "gender")?.Value;
            UpdatedAt = UserPrincipal.Claims.FirstOrDefault(c => c.Type == "updated_at")?.Value;
            Nickname = UserPrincipal.Claims.FirstOrDefault(c => c.Type == "nickname")?.Value;
            ZoneInfo = UserPrincipal.Claims.FirstOrDefault(c => c.Type == "zoneinfo")?.Value;
            PreferredUsername = UserPrincipal.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
            Website = UserPrincipal.Claims.FirstOrDefault(c => c.Type == "website")?.Value;
        }
    }
}