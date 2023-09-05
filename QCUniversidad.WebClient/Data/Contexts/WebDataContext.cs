using Microsoft.EntityFrameworkCore;
using SmartB1t.Security.WebSecurity.Local;
using SmartB1t.Security.WebSecurity.Local.Models;

namespace QCUniversidad.WebClient.Data.Contexts;

public class WebDataContext : DbContext
{
    #region Account sets

    /// <summary>
    /// The database set storing <see cref="User"/>s.
    /// </summary>
    public DbSet<User> Users { get; set; }

    /// <summary>
    /// The database set storing <see cref="UserSecrets"/>.
    /// </summary>
    public DbSet<UserSecrets> Secrets { get; set; }

    /// <summary>
    /// The database set storing <see cref="Role"/>s.
    /// </summary>
    public DbSet<Role> Roles { get; set; }

    /// <summary>
    /// The database set storing <see cref="UserRole"/>s.
    /// </summary>
    public DbSet<UserRole> UserRoles { get; set; }

    /// <summary>
    /// The database set storing <see cref="ExtraClaim"/>
    /// </summary>
    public DbSet<ExtraClaim> ExtraClaims { get; set; }

    #endregion

    public WebDataContext(DbContextOptions<WebDataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        #region Account modeling

        _ = modelBuilder.Entity<User>().HasKey(u => u.Id);

        _ = modelBuilder.Entity<UserSecrets>()
                    .HasOne(us => us.User)
                    .WithOne(u => u.Secrets)
                    .HasForeignKey<UserSecrets>(us => us.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

        _ = modelBuilder.Entity<UserRole>()
                    .HasKey(ur => new { ur.UserId, ur.RoleId });

        _ = modelBuilder.Entity<UserRole>()
                    .HasOne(ur => ur.User)
                    .WithMany(u => u.Roles)
                    .HasForeignKey(ur => ur.UserId);

        _ = modelBuilder.Entity<UserRole>()
                    .HasOne(ur => ur.Role)
                    .WithMany(r => r.RoleUsers)
                    .HasForeignKey(ur => ur.RoleId);

        _ = modelBuilder.Entity<ExtraClaim>()
                    .HasOne(e => e.User)
                    .WithMany(u => u.ExtraClaims)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

        #endregion

        base.OnModelCreating(modelBuilder);
    }
}
