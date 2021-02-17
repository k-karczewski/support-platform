using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SupportPlatform.Database
{
    public class SupportPlatformDbContext : IdentityDbContext<UserEntity, IdentityRole<int>, int>
    {
        public DbSet<ReportEntity> Reports { get; set; }
        public DbSet<ResponseEntity> Responses { get; set; }
        public DbSet<ModificationEntryEntity> ModificationEntries { get; set; }

        public SupportPlatformDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ReportEntity>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.HasOne(u => u.User).WithMany(r => r.Reports).HasForeignKey(k => k.UserId);

            });

            builder.Entity<ResponseEntity>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.HasOne(r => r.Report).WithMany(r => r.Responses).HasForeignKey(k => k.ReportId).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(u => u.User).WithMany(r => r.Responses).HasForeignKey(k => k.UserId);
            });

            builder.Entity<ModificationEntryEntity>(entity =>
            {
                entity.HasKey(k => k.Id);
                entity.HasOne(r => r.Report).WithMany(m => m.ModificationEntries).HasForeignKey(k => k.ReportId);
            });

            builder.Entity<UserEntity>().ToTable("Users");
        }
    }
}
