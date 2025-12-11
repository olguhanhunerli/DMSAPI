using DMSAPI.Entities.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Context
{
    public class DMSDbContext : DbContext
    {
        public DMSDbContext(DbContextOptions<DMSDbContext> options) : base(options)
        { }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Position> Positions { get; set; }

        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentAttachment> DocumentAttachments { get; set; }
        public DbSet<DocumentVersion> DocumentVersions { get; set; }
        public DbSet<DocumentApprovalHistory> DocumentApprovalHistories { get; set; }
        public DbSet<DocumentAccessLog> DocumentAccessLogs { get; set; }
        public DbSet<DocumentApproval> DocumentApprovals { get; set; }
        public DbSet<DocumentFile> DocumentFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
         .HasOne(u => u.Manager)
         .WithMany()
         .HasForeignKey(u => u.ManagerId)
         .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<User>()
                .HasMany(u => u.RefreshTokens)
                .WithOne(r => r.User)
                .HasForeignKey(r => r.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.PermissionsList)
                .WithOne(p => p.User)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Position)
                .WithMany(p => p.Users)
                .HasForeignKey(u => u.PositionId);


            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Parent)
                      .WithMany(e => e.Children)
                      .HasForeignKey(e => e.ParentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Company)
                      .WithMany()
                      .HasForeignKey(e => e.CompanyId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.CompanyId);
                entity.HasIndex(e => e.ParentId);
                entity.HasIndex(e => e.IsActive);
                entity.HasIndex(e => e.Slug).IsUnique(false);
            });

            modelBuilder.Entity<Category>()
                .HasOne(x => x.CreatedByUser)
                .WithMany()
                .HasForeignKey(x => x.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasOne(x => x.UpdatedByUser)
                .WithMany()
                .HasForeignKey(x => x.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Document>(entity =>
            {
                entity.HasKey(d => d.Id);

                entity.Property(d => d.Title).IsRequired();
                entity.Property(d => d.DocumentCode).IsRequired();
               
             
                entity.Property(d => d.DocumentType).IsRequired();

                entity.HasIndex(d => d.CompanyId);
                entity.HasIndex(d => d.CategoryId);
                entity.HasIndex(d => d.StatusId);
                entity.HasIndex(d => d.IsDeleted);
                entity.HasIndex(d => d.IsArchived);
            });

            modelBuilder.Entity<Document>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Document>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(d => d.UpdatedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Document>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(d => d.DeletedByUserId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Department>()
                .HasOne(d => d.Manager)
                .WithMany()
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Department>()
                .HasOne(d => d.CreatedByUser)
                .WithMany()
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Department>()
                .HasOne(d => d.UploadedByUser)
                .WithMany()
                .HasForeignKey(d => d.UploadedBy)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Role>()
                .HasOne(r => r.CreatedByUser)
                .WithMany()
                .HasForeignKey(r => r.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>()
                .HasOne(r => r.UploadedByUser)
                .WithMany()
                .HasForeignKey(r => r.UploadedBy)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Position>()
                .HasOne(p => p.CreatedByUser)
                .WithMany()
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Position>()
                .HasOne(p => p.UploadedByUser)
                .WithMany()
                .HasForeignKey(p => p.UploadedBy)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<DocumentApproval>(entity =>
            {
                entity.HasIndex(x => new { x.DocumentId, x.UserId, x.ApprovalLevel })
                      .IsUnique();

                entity.HasOne(x => x.Document)
                      .WithMany(d => d.Approvals)
                      .HasForeignKey(x => x.DocumentId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.User)
                      .WithMany()
                      .HasForeignKey(x => x.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
