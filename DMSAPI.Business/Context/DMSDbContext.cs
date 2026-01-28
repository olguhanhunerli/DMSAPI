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
        public DbSet<DocumentApprovalHistory> DocumentApprovalHistory { get; set; }
        public DbSet<DocumentAccessLog> DocumentAccessLogs { get; set; }
        public DbSet<DocumentApproval> DocumentApprovals { get; set; }
        public DbSet<DocumentFile> DocumentFiles { get; set; }
		public DbSet<DocumentCodeReservation> DocumentCodeReservations { get; set; }
		public DbSet<DocumentRevision> DocumentRevisions { get; set; }
		public DbSet<Instrument> Instruments { get; set; }
		public DbSet<InstrumentCalibration> InstrumentCalibrations { get; set; }
		public DbSet<InstrumentCalibrationFile> InstrumentCalibrationFiles { get; set; }
		public DbSet<Customer> Customers { get; set; }
		public DbSet<Complaint> Complaints { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.Entity<Company>()
					.HasKey(c => c.Id);

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

			modelBuilder.Entity<Permission>()
				.HasOne(p => p.User)
				.WithMany(u => u.PermissionsList)
				.HasForeignKey(p => p.UserId)
				.OnDelete(DeleteBehavior.Cascade);

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
				.HasOne(u => u.Position)
				.WithMany(p => p.Users)
				.HasForeignKey(u => u.PositionId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<User>()
				.HasOne(u => u.Department)
				.WithMany(d => d.Users)
				.HasForeignKey(u => u.DepartmentId)
				.OnDelete(DeleteBehavior.SetNull);

			modelBuilder.Entity<User>()
				.HasOne(u => u.Role)
				.WithMany(r => r.Users)
				.HasForeignKey(u => u.RoleId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<RefreshToken>()
				.HasKey(r => r.Id);

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

				entity.HasOne(d => d.CreatedByUser)
					.WithMany()
					.HasForeignKey(d => d.CreatedByUserId)
					.OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(d => d.UpdatedByUser)
					.WithMany()
					.HasForeignKey(d => d.UpdatedByUserId)
					.OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(d => d.DeletedByUser)
					.WithMany()
					.HasForeignKey(d => d.DeletedByUserId)
					.OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(d => d.Category)
					.WithMany(c => c.Documents)
					.HasForeignKey(d => d.CategoryId)
					.OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(d => d.Company)
					.WithMany()
					.HasForeignKey(d => d.CompanyId)
					.OnDelete(DeleteBehavior.Restrict);
			});
			modelBuilder.Entity<DocumentAttachment>()
				.HasOne(x => x.Document)
				.WithMany(d => d.Attachments)
				.HasForeignKey(x => x.DocumentId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<DocumentVersion>()
				.HasOne(x => x.Document)
				.WithMany(d => d.Versions)
				.HasForeignKey(x => x.DocumentId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<DocumentFile>()
				.HasOne(x => x.Document)
				.WithMany(d => d.Files)
				.HasForeignKey(x => x.DocumentId)
				.OnDelete(DeleteBehavior.Cascade);

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

			modelBuilder.Entity<DocumentApprovalHistory>()
				.HasOne(x => x.Document)
				.WithMany(d => d.ApprovalHistories)
				.HasForeignKey(x => x.DocumentId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<DocumentAccessLog>()
				.HasOne(x => x.Document)
				.WithMany(d => d.AccessLogs)
				.HasForeignKey(x => x.DocumentId)
				.OnDelete(DeleteBehavior.Cascade);
			modelBuilder.Entity<DocumentAccessLog>()
				.HasOne(x => x.User)
				.WithMany()
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.NoAction);
			modelBuilder.Entity<DocumentCodeReservation>()
				.HasIndex(x => new { x.CompanyId, x.RootCategoryId, x.SequenceNumber })
				.IsUnique();

			modelBuilder.Entity<DocumentCodeReservation>()
				.HasIndex(x => x.DocumentCode)
				.IsUnique();
			modelBuilder.Entity<Instrument>(entity =>
			{
				entity.HasIndex(e => e.Asset_Code).IsUnique();
				entity.HasIndex(e => e.Serial_No).IsUnique();
				entity.HasIndex(e => e.Location);

				entity.HasOne(i => i.Company)
					  .WithMany()
					  .HasForeignKey(i => i.CompanyId)
					  .OnDelete(DeleteBehavior.Restrict);
			});
			modelBuilder.Entity<Instrument>(entity =>
			{
				entity.HasOne(i => i.CreatedByName)
					  .WithMany()
					  .HasForeignKey(i => i.CreatedBy)
					  .OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(i => i.UpdatedByName)
					  .WithMany()
					  .HasForeignKey(i => i.UpdatedBy)
					  .OnDelete(DeleteBehavior.Restrict);

			});
			modelBuilder.Entity<Instrument>(entity =>
			{
				entity.HasOne(i => i.DeletedByUser)
					  .WithMany()
					  .HasForeignKey(i => i.DeletedBy)
					  .OnDelete(DeleteBehavior.Restrict);
			});
			modelBuilder.Entity<InstrumentCalibration>(entity =>
			{
				entity.HasKey(x => x.CalibrationId);

				entity.HasOne(x => x.InstrumentName)
					  .WithMany()
					  .HasForeignKey(x => x.InstrumentId)
					  .OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(x => x.CompanyName)
					  .WithMany()
					  .HasForeignKey(x => x.CompanyId)
					  .OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(x => x.CreatedByName)
					  .WithMany()
					  .HasForeignKey(x => x.CreatedBy)
					  .OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(x => x.UpdatedByName)
					  .WithMany()
					  .HasForeignKey(x => x.UpdatedBy)
					  .OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(x => x.DeletedByUser)
					  .WithMany()
					  .HasForeignKey(x => x.DeletedBy)
					  .OnDelete(DeleteBehavior.Restrict);

				entity.HasMany(x => x.Files)
					  .WithOne(f => f.Calibration)
					  .HasForeignKey(f => f.CalibrationId)
					  .OnDelete(DeleteBehavior.Restrict);

				entity.HasIndex(x => x.CompanyId);
				entity.HasIndex(x => x.InstrumentId);
				entity.HasIndex(x => x.IsDeleted);
			});
			modelBuilder.Entity<InstrumentCalibrationFile>(entity =>
			{
				entity.HasKey(x => x.FileId);

				entity.HasOne(x => x.Calibration)
					  .WithMany(c => c.Files)
					  .HasForeignKey(x => x.CalibrationId)
					  .OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(x => x.CreatedByName)
					  .WithMany()
					  .HasForeignKey(x => x.CreatedBy)
					  .OnDelete(DeleteBehavior.Restrict);

				entity.HasOne(x => x.UpdatedByName)
					  .WithMany()
					  .HasForeignKey(x => x.UpdatedBy)
					  .OnDelete(DeleteBehavior.Restrict);
				entity.HasIndex(x => x.CompanyId);
				entity.HasIndex(x => x.CalibrationId);
				entity.HasIndex(x => x.IsDeleted);
			});
			modelBuilder.Entity<Customer>(entity =>
			{
				entity.HasKey(c => c.Id);
				entity.HasOne(c => c.Company)
						.WithMany()
						.HasForeignKey(c => c.CompanyId)
						.OnDelete(DeleteBehavior.Restrict);
				entity.HasOne(x => x.DeletedByUser)
				.WithMany()
				.HasForeignKey(x => x.DeletedBy)
				.OnDelete(DeleteBehavior.Restrict);
			});
			modelBuilder.Entity<Complaint>(entity =>
			{ 
				entity.HasKey(c => c.Id);
				entity.HasIndex(c => c.ComplaintNo).IsUnique();
				entity.HasOne(c => c.Company)
						.WithMany()
						.HasForeignKey(c => c.CompanyId)
						.OnDelete(DeleteBehavior.Restrict);
				entity.HasOne(c => c.Customer)
						.WithMany()
						.HasForeignKey(c => c.CustomerId)
						.OnDelete(DeleteBehavior.Restrict);
				entity.HasOne(c => c.CreatedByUser)
						.WithMany()
						.HasForeignKey(c => c.CreatedBy)
						.OnDelete(DeleteBehavior.Restrict);
				entity.HasOne(c => c.AssignedToUser)
						.WithMany()
						.HasForeignKey(c => c.AssignedTo)
						.OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(c => c.UpdateByUser)
                        .WithMany()
                        .HasForeignKey(c => c.UpdateBy)
                        .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(c => c.DeleteByUser)
                        .WithMany()
                        .HasForeignKey(c => c.DeletedBy)
                        .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(c => c.ClosedByUser)
                      .WithMany()
                      .HasForeignKey(c => c.ClosedBy)
                      .OnDelete(DeleteBehavior.Restrict);

            });

        }
	}
}
