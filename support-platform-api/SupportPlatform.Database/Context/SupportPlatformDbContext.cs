using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SupportPlatform.Database
{
    public class SupportPlatformDbContext : IdentityDbContext<UserEntity, RoleEntity, int, IdentityUserClaim<int>, UserRoleEntity, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DbSet<ReportEntity> Reports { get; set; }
        public DbSet<ResponseEntity> Responses { get; set; }
        public DbSet<ModificationEntryEntity> ModificationEntries { get; set; }
        public DbSet<AttachmentEntity> Attachments { get; set; }

        public SupportPlatformDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ReportEntity>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.HasOne(u => u.User).WithMany(r => r.Reports).HasForeignKey(k => k.UserId);
                entity.HasMany(r => r.Responses).WithOne(r => r.Report).HasForeignKey(k => k.ReportId);
            });

            builder.Entity<ResponseEntity>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.HasOne(u => u.User).WithMany(r => r.Responses).HasForeignKey(k => k.UserId).OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<ModificationEntryEntity>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.HasOne(r => r.Report).WithMany(m => m.ModificationEntries).HasForeignKey(k => k.ReportId);
            });

            builder.Entity<UserRoleEntity>(entity =>
            {
                entity.HasOne(u => u.User).WithMany(ur => ur.UserRoles).HasForeignKey(k => k.UserId);
                entity.HasOne(r => r.Role).WithMany(ur => ur.UserRoles).HasForeignKey(k => k.RoleId);
            });

            builder.Entity<UserEntity>().ToTable("Users");
            builder.Entity<RoleEntity>().ToTable("Roles");
        }
    }
}
