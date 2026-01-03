using AutoMapper;
using DMSAPI.Entities.DTOs;
using DMSAPI.Entities.DTOs.CategoryDTOs;
using DMSAPI.Entities.DTOs.CompanyDTOs;
using DMSAPI.Entities.DTOs.DepartmentDTOs;
using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Entities.DTOs.PositionDTOs;
using DMSAPI.Entities.DTOs.RoleDTOs;
using DMSAPI.Entities.DTOs.UserDTOs;
using DMSAPI.Entities.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace DMSAPI.Services.Mapping
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<User, UserDTO>()
				.ForMember(d => d.RoleName,
					o => o.MapFrom(s => s.Role != null ? s.Role.Name : null))
				.ForMember(d => d.CompanyName,
					o => o.MapFrom(s => s.Company != null ? s.Company.Name : null))
				.ForMember(d => d.DepartmentName,
					o => o.MapFrom(s => s.Department != null ? s.Department.Name : null))
				.ForMember(d => d.ManagerName,
					o => o.MapFrom(s =>
						s.Manager != null
							? s.Manager.FirstName + " " + s.Manager.LastName
							: null));

			CreateMap<User, UserMiniDTO>()
				.ForMember(d => d.FullName,
					o => o.MapFrom(s => s.FirstName + " " + s.LastName));

			CreateMap<User, AuthResponseDTO>()
				.ForMember(d => d.User, o => o.MapFrom(s => s));

			CreateMap<UpdateUserDTO, User>()
				.ForMember(d => d.Position, o => o.Ignore());


			CreateMap<Role, RoleDTO>()
				.ForMember(d => d.CreatedByUser,
					o => o.MapFrom(s =>
						s.CreatedByUser != null
							? s.CreatedByUser.FirstName + " " + s.CreatedByUser.LastName
							: null))
				.ForMember(d => d.UploadedByUser,
					o => o.MapFrom(s =>
						s.UploadedByUser != null
							? s.UploadedByUser.FirstName + " " + s.UploadedByUser.LastName
							: null));

			CreateMap<AddRoleDTO, Role>();
			CreateMap<UpdateRoleDTO, Role>();


			CreateMap<Company, CompanyDTO>();
			CreateMap<AddCompanyDTO, Company>();
			CreateMap<UpdateCompanyDTO, Company>();


			CreateMap<Category, CategoryDTO>()
				.ForMember(d => d.CreatedByName,
					o => o.MapFrom(s =>
						s.CreatedByUser != null
							? s.CreatedByUser.FirstName + " " + s.CreatedByUser.LastName
							: null))
				.ForMember(d => d.UpdatedByName,
					o => o.MapFrom(s =>
						s.UpdatedByUser != null
							? s.UpdatedByUser.FirstName + " " + s.UpdatedByUser.LastName
							: null))
				.ForMember(d => d.CompanyName,
					o => o.MapFrom(s => s.Company != null ? s.Company.Name : null));

			CreateMap<CreateCategoryDTO, Category>();
			CreateMap<UpdateCategoryDTO, Category>();


			CreateMap<Department, DepartmentDTO>()
				.ForMember(d => d.CompanyName,
					o => o.MapFrom(s => s.Company != null ? s.Company.Name : null))
				.ForMember(d => d.ManagerName,
					o => o.MapFrom(s =>
						s.Manager != null
							? s.Manager.FirstName + " " + s.Manager.LastName
							: null))
				.ForMember(d => d.CreatedByName,
					o => o.MapFrom(s =>
						s.CreatedByUser != null
							? s.CreatedByUser.FirstName + " " + s.CreatedByUser.LastName
							: null))
				.ForMember(d => d.UploadedByName,
					o => o.MapFrom(s =>
						s.UploadedByUser != null
							? s.UploadedByUser.FirstName + " " + s.UploadedByUser.LastName
							: null));

			CreateMap<Department, DepartmentDetailDTO>()
				.ForMember(d => d.CompanyName,
					o => o.MapFrom(s => s.Company != null ? s.Company.Name : null));

			CreateMap<CreateDepartmentDTO, Department>();
			CreateMap<UpdateDepartmentDTO, Department>();


			CreateMap<Position, PositionDTO>()
				.ForMember(d => d.CompanyName,
					o => o.MapFrom(s => s.Company != null ? s.Company.Name : null))
				.ForMember(d => d.CreatedByName,
					o => o.MapFrom(s =>
						s.CreatedByUser != null
							? s.CreatedByUser.FirstName + " " + s.CreatedByUser.LastName
							: null))
				.ForMember(d => d.UploadedByName,
					o => o.MapFrom(s =>
						s.UploadedByUser != null
							? s.UploadedByUser.FirstName + " " + s.UploadedByUser.LastName
							: null));

			CreateMap<CreatePositionDTO, Position>();
			CreateMap<UpdatePositionDTO, Position>();


			CreateMap<DocumentFile, MainDocumentFileDTO>();
			CreateMap<DocumentAttachment, DocumentAttachmentDTO>();
			CreateMap<DocumentVersion, DocumentVersionDTO>();
			CreateMap<DocumentApprovalHistory, DocumentApprovalHistoryDTO>()
                .ForMember(d => d.ActionByName,
				o => o.MapFrom(s =>
					s.ActionByUser != null
						? s.ActionByUser.FirstName + " " + s.ActionByUser.LastName
						: "-"))

			.ForMember(d => d.ActionDisplayName,
				o => o.MapFrom(s => s.ActionType))

			.ForMember(d => d.ActionTimeText,
				o => o.MapFrom(s =>
					s.ActionAt.HasValue
						? s.ActionAt.Value.ToString("dd.MM.yyyy HH:mm")
						: "-"));
            CreateMap<DocumentAccessLog, DocumentAccessLogDTO>()
					 .ForMember(d => d.UserName,
						 o => o.MapFrom(x =>
							 x.User != null
								 ? x.User.FirstName + " " + x.User.LastName
								 : "-"))
					 .ForMember(d => d.AccessTimeText,
						 o => o.MapFrom(x =>
							 x.AccessAt.HasValue
								 ? x.AccessAt.Value.ToString("dd.MM.yyyy HH:mm")
								 : "-"));

            CreateMap<Document, DocumentCreateResponseDTO>()
				.ForMember(d => d.FileName, o => o.Ignore())
				.ForMember(d => d.OriginalFileName, o => o.Ignore())
				.ForMember(d => d.FileSize, o => o.Ignore())
				.ForMember(d => d.FileType, o => o.Ignore())
				.ForMember(d => d.CompanyName, o => o.MapFrom(s => string.Empty))
				.ForMember(d => d.CategoryName, o => o.MapFrom(s => string.Empty))
				.ForMember(d => d.Breadcrumb, o => o.MapFrom(s => new List<string>()))
				.ForMember(d => d.BreadcrumbPath, o => o.MapFrom(s => string.Empty))
				.ForMember(d => d.CreatedBy, o => o.MapFrom(s => s.CreatedByUserId))
				.ForMember(d => d.CreatedByName, o => o.MapFrom(s => string.Empty));


			CreateMap<Document, DocumentDTO>()
				.ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
				.ForMember(d => d.DocumentCode, o => o.MapFrom(s => s.DocumentCode))
				.ForMember(d => d.VersionNumber, o => o.MapFrom(s => s.VersionNumber))
				.ForMember(d => d.VersionNote, o => o.MapFrom(s => s.VersionNote))
				.ForMember(d => d.DocumentType, o => o.MapFrom(s => s.DocumentType))
				.ForMember(d => d.IsLatestVersion, o => o.MapFrom(s => s.IsLatestVersion))
                .ForMember(d => d.AllowedDepartments, opt => opt.Ignore())
				.ForMember(d => d.AllowedRoles, opt => opt.Ignore())
				.ForMember(d => d.AllowedUsers, opt => opt.Ignore())
				.ForMember(d => d.LockedByUserName, o => o.MapFrom(x => x.LockedByUser !=null ? x.LockedByUser.FirstName + " " +x.LockedByUser.LastName: null))
				.ForMember(d => d.IsLocked, opt => opt.MapFrom(x => x.IsLocked))
                .ForMember(d => d.RejectReason, o => o.MapFrom(s =>
								s.Approvals
									.Where(a => a.IsRejected)
									.Select(a => a.RejectReason)
									.FirstOrDefault()
							))

							.ForMember(d => d.RejectedBy, o => o.MapFrom(s =>
								s.Approvals
									.Where(a => a.IsRejected)
									.Select(a => (int?)a.UserId)
									.FirstOrDefault()
							))

							.ForMember(d => d.RejectedByName, o => o.MapFrom(s =>
								s.Approvals
									.Where(a => a.IsRejected && a.User != null)
									.Select(a => a.User.FirstName + " " + a.User.LastName)
									.FirstOrDefault()
							))

							.ForMember(d => d.RejectedAt, o => o.MapFrom(s =>
								s.Approvals
									.Where(a => a.IsRejected)
									.Select(a => a.ActionAt)
									.FirstOrDefault()
							))
				.ForMember(d => d.CategoryName,
					o => o.MapFrom(s => s.Category != null ? s.Category.Name : null))

				.ForMember(d => d.CompanyName,
					o => o.MapFrom(s => s.Company != null ? s.Company.Name : null))
				.ForMember(d => d.CompanyCode,
					o => o.MapFrom(s => s.Company != null ? s.Company.CompanyCode : null))

				.ForMember(d => d.CreatedByName,
					o => o.MapFrom(s =>
						s.CreatedByUser != null
							? s.CreatedByUser.FirstName + " " + s.CreatedByUser.LastName
							: null))

				.ForMember(d => d.UpdatedByName,
					o => o.MapFrom(s =>
						s.UpdatedByUser != null
							? s.UpdatedByUser.FirstName + " " + s.UpdatedByUser.LastName
							: null))

				.ForMember(d => d.DeletedByName,
					o => o.MapFrom(s =>
						s.DeletedByUser != null
							? s.DeletedByUser.FirstName + " " + s.DeletedByUser.LastName
							: null))
				.ForMember(d => d.Status, o => o.MapFrom(s =>
					s.StatusId == 1 ? "Bekliyor" :
					s.StatusId == 2 ? "Onaylandı" :
					s.StatusId == 3 ? "Reddedildi" :
					"Bilinmiyor"))

				.ForMember(d => d.CurrentApproverId, o => o.MapFrom(s =>
					s.Approvals
						.Where(a => !a.IsApproved && !a.IsRejected)
						.Select(a => (int?)a.UserId)
						.FirstOrDefault()))

				.ForMember(d => d.CurrentApproverName, o => o.MapFrom(s =>
					s.Approvals
						.Where(a => !a.IsApproved && !a.IsRejected && a.User != null)
						.Select(a => a.User.FirstName + " " + a.User.LastName)
						.FirstOrDefault()))

				.ForMember(d => d.ApprovedBy, o => o.MapFrom(s =>
					s.Approvals.Where(a => a.IsApproved)
							   .Select(a => (int?)a.UserId)
							   .FirstOrDefault()))

				.ForMember(d => d.ApprovedByName, o => o.MapFrom(s =>
					s.Approvals.Where(a => a.IsApproved && a.User != null)
							   .Select(a => a.User.FirstName + " " + a.User.LastName)
							   .FirstOrDefault()))

				.ForMember(d => d.RejectedBy, o => o.MapFrom(s =>
					s.Approvals.Where(a => a.IsRejected)
							   .Select(a => (int?)a.UserId)
							   .FirstOrDefault()))

				.ForMember(d => d.RejectedByName, o => o.MapFrom(s =>
					s.Approvals.Where(a => a.IsRejected && a.User != null)
							   .Select(a => a.User.FirstName + " " + a.User.LastName)
							   .FirstOrDefault()))

				.ForMember(d => d.MainFile, o => o.MapFrom(s =>
					s.Files.Select(f => new MainDocumentFileDTO
					{
						Id = f.Id,
						FileName = f.FileName,
						OriginalFileName = f.OriginalFileName,
						FileExtension = f.FileExtension,
						FileSize = f.FileSize,
						FilePath = f.FilePath,
						PdfFilePath = f.PdfFilePath
					}).FirstOrDefault()))

				.ForMember(d => d.Attachments, o => o.MapFrom(s => s.Attachments))
				.ForMember(d => d.Versions, o => o.MapFrom(s => s.Versions))
				.ForMember(d => d.ApprovalHistories, o => o.MapFrom(s => s.ApprovalHistories))
				.ForMember(d => d.AccessLogs, o => o.MapFrom(s => s.AccessLogs))

				.AfterMap((src, dest) =>
				{
					dest.AllowedRoleIds = SafeParseJson(src.AllowedRoles);
					dest.AllowedDepartmentIds = SafeParseJson(src.AllowedDepartments);
					dest.AllowedUserIds = SafeParseJson(src.AllowedUsers);
				});



			CreateMap<DocumentCreateDTO, Document>()
				.ForMember(x => x.Id, opt => opt.Ignore())
				.ForMember(x => x.DocumentCode, opt => opt.Ignore())
				.ForMember(x => x.CreatedAt, opt => opt.Ignore())
				.ForMember(x => x.CreatedByUserId, opt => opt.Ignore())
				.ForMember(x => x.UpdatedAt, opt => opt.Ignore())
				.ForMember(x => x.UpdatedByUserId, opt => opt.Ignore())
				.ForMember(x => x.DeletedAt, opt => opt.Ignore())
				.ForMember(x => x.DeletedByUserId, opt => opt.Ignore())
				.ForMember(x => x.IsDeleted, opt => opt.Ignore())
				.ForMember(x => x.IsArchived, opt => opt.Ignore())
				.ForMember(x => x.Files, opt => opt.Ignore())
				.ForMember(x => x.Attachments, opt => opt.Ignore());


			CreateMap<Document, MyPendingDTO>()
				.ForMember(d => d.DocumentCode, o => o.MapFrom(s => s.DocumentCode))
				.ForMember(d => d.Title, o => o.MapFrom(s => s.Title))
				.ForMember(d => d.StatusId, o => o.MapFrom(s => s.StatusId))
				.ForMember(d => d.StatusName, o => o.MapFrom(s =>
					s.StatusId == 1 ? "Bekliyor" :
					s.StatusId == 2 ? "Onaylandı" :
					s.StatusId == 3 ? "Reddedildi" :
					"Bilinmiyor"))
				.ForMember(d => d.CreatedAt, o => o.MapFrom(s => s.CreatedAt))
				.ForMember(d => d.CreatedById, o => o.MapFrom(s => s.CreatedByUserId))
				.ForMember(d => d.CreatedByName, o => o.MapFrom(s =>
					s.CreatedByUser != null
						? s.CreatedByUser.FirstName + " " + s.CreatedByUser.LastName
						: null));
				

		}
		private static List<int> SafeParseJson(string? json)
		{
			if (string.IsNullOrWhiteSpace(json))
				return new List<int>();

			try
			{
				return JsonSerializer.Deserialize<List<int>>(json) ?? new List<int>();
			}
			catch
			{
				return new List<int>();
			}
		}
	}

}
