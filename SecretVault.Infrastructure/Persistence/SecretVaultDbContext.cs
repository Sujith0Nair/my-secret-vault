using SecretVault.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SecretVault.Infrastructure.Persistence;

public class SecretVaultDbContext(DbContextOptions<SecretVaultDbContext> options)
    : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<Secret> Secrets { get; set; }
    public DbSet<SharePermission> SharePermissions { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Secret>(entity =>
        {
            entity.ToTable("Secrets");
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Title).IsRequired().HasMaxLength(200);
            entity.Property(s => s.EncryptedContent).IsRequired();
            
            entity.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(e => e.OwnerUserId)
                .OnDelete(DeleteBehavior.Cascade);
            
            entity.HasMany(s => s.SharePermissions)
                .WithOne()
                .HasForeignKey(sharePermission => sharePermission.SecretId)
                .OnDelete(DeleteBehavior.Cascade);
        });
        
        builder.Entity<SharePermission>(entity =>
        {
            entity.ToTable("SharePermissions");
            entity.HasKey(s => s.Id);
            
            entity.HasOne<ApplicationUser>()
                .WithMany()
                .HasForeignKey(e => e.SharedWithUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}