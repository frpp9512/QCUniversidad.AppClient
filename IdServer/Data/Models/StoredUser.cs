using System;
using System.Collections.Generic;

namespace IdServer.Data.Models;

public record StoredUser
{
    // Profile claims: ["website", "name", "birthdate", "profile", "family_name", "picture", "locale", "preferred_username", "middle_name", "gender", "updated_at", "given_name", "zoneinfo", "nickname"]

    public Guid Id { get; set; }

    public string Username { get; set; }

    public string DisplayName => $"{Name} {GivenName} {MiddleName} {FamilyName}";

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

    public bool IsActive { get; set; } = true;

    public StoredUserSecrets Secrets { get; set; }

    public List<StoredUserClaim> Claims { get; set; }

    public List<StoredUserClientRoles> StoredUserClientRoles { get; set; }
}