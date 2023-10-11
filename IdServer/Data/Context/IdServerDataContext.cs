using IdServer.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace IdServer.Data.Context;

public class IdServerDataContext : DbContext
{
    public DbSet<StoredClient> Clients { get; set; }
    public DbSet<StoredClientSecret> ClientSecrets { get; set; }
    public DbSet<StoredClientGrantType> ClientGrantTypes { get; set; }
    public DbSet<StoredClientAllowedScope> ClientAllowedScopes { get; set; }

    public DbSet<StoredApiResource> ApiResources { get; set; }
    public DbSet<StoredApiResourceScope> ApiResourceScopes { get; set; }
    public DbSet<StoredApiResourceUserClaim> apiResourceUserClaims { get; set; }

    public DbSet<StoredIdentityResource> IdentityResources { get; set; }
    public DbSet<StoredIdentityResourceUserClaim> IdentityResourceUserClaims { get; set; }

    public DbSet<StoredApiScope> ApiScopes { get; set; }
    public DbSet<StoredApiScopeUserClaim> storedApiScopeUserClaims { get; set; }

    public DbSet<StoredUser> StoredUsers { get; set; }
    public DbSet<StoredUserClaim> StoredUserClaims { get; set; }
    public DbSet<StoredUserSecrets> StoredUserSecrets { get; set; }
    public DbSet<StoredUserClientRoles> StoredUserClientRoles { get; set; }

    public IdServerDataContext(DbContextOptions options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.Entity<StoredClient>()
                    .HasMany(x => x.Secrets)
                    .WithOne(x => x.Client)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<StoredClient>()
                    .HasMany(x => x.AllowedScopes)
                    .WithOne(x => x.Client)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<StoredClient>()
                    .HasMany(x => x.GrantTypes)
                    .WithOne(x => x.Client)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<StoredClient>()
                    .HasMany(x => x.PostLogoutRedirectUris)
                    .WithOne(x => x.StoredClient)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<StoredApiResource>()
                    .HasMany(x => x.UserClaims)
                    .WithOne(x => x.StoredApiResource)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<StoredApiResource>()
                    .HasMany(x => x.Scopes)
                    .WithOne(x => x.StoredApiResource)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<StoredIdentityResource>()
                    .HasMany(x => x.UserClaims)
                    .WithOne(x => x.StoredIdentityResource)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<StoredApiScope>()
                    .HasMany(x => x.UserClaims)
                    .WithOne(x => x.StoredApiScope)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<StoredUser>()
                    .HasMany(x => x.Claims)
                    .WithOne(x => x.StoredUser)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<StoredUser>()
                    .HasOne(x => x.Secrets)
                    .WithOne(x => x.StoredUser)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<StoredUserClientRoles>()
                    .HasOne<StoredUser>(x => x.StoredUser)
                    .WithMany(x => x.StoredUserClientRoles)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<StoredUserClientRoles>()
                    .HasOne<StoredClient>(x => x.StoredClient)
                    .WithMany(x => x.StoredUserClientRoles)
                    .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}