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
using System.Text.Json;
using System.Collections.Generic;

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
                    o => o.MapFrom(s => s.Manager != null
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
                    o => o.MapFrom(s => s.CreatedByUser != null
                        ? s.CreatedByUser.FirstName + " " + s.CreatedByUser.LastName
                        : null))
                .ForMember(d => d.UpdatedByName,
                    o => o.MapFrom(s => s.UpdatedByUser != null
                        ? s.UpdatedByUser.FirstName + " " + s.UpdatedByUser.LastName
                        : null))
                .ForMember(d => d.CompanyName,
                    o => o.MapFrom(s => s.Company != null ? s.Company.Name : null));

            CreateMap<CreateCategoryDTO, Category>();
            CreateMap<UpdateCategoryDTO, Category>();

            CreateMap<Document, DocumentDTO>()
            .ForMember(d => d.CompanyName, o => o.Ignore())
            .ForMember(d => d.CompanyCode, o => o.Ignore())
            .ForMember(d => d.CategoryName, o => o.Ignore())

            .ForMember(d => d.CreatedByName, o => o.Ignore())
            .ForMember(d => d.UpdatedByName, o => o.Ignore())
            .ForMember(d => d.DeletedByName, o => o.Ignore())
            .ForMember(d => d.ApproverName, o => o.Ignore())
            .ForMember(d => d.ApprovedByName, o => o.Ignore())
            .ForMember(d => d.RejectedByName, o => o.Ignore())

            .ForMember(d => d.AllowedRoleIds, o => o.Ignore())
            .ForMember(d => d.AllowedDepartmentIds, o => o.Ignore())
            .ForMember(d => d.AllowedUserIds, o => o.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.AllowedRoleIds = string.IsNullOrWhiteSpace(src.AllowedRoles)
                    ? new List<int>()
                    : JsonSerializer.Deserialize<List<int>>(src.AllowedRoles);

                dest.AllowedDepartmentIds = string.IsNullOrWhiteSpace(src.AllowedDepartments)
                    ? new List<int>()
                    : JsonSerializer.Deserialize<List<int>>(src.AllowedDepartments);

                dest.AllowedUserIds = string.IsNullOrWhiteSpace(src.AllowedUsers)
                    ? new List<int>()
                    : JsonSerializer.Deserialize<List<int>>(src.AllowedUsers);
            })
                .ForMember(d => d.AllowedRoleIds, o => o.Ignore())
                .ForMember(d => d.AllowedDepartmentIds, o => o.Ignore())
                .ForMember(d => d.AllowedUserIds, o => o.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.AllowedRoleIds = string.IsNullOrWhiteSpace(src.AllowedRoles)
                        ? new List<int>()
                        : JsonSerializer.Deserialize<List<int>>(src.AllowedRoles);

                    dest.AllowedDepartmentIds = string.IsNullOrWhiteSpace(src.AllowedDepartments)
                        ? new List<int>()
                        : JsonSerializer.Deserialize<List<int>>(src.AllowedDepartments);

                    dest.AllowedUserIds = string.IsNullOrWhiteSpace(src.AllowedUsers)
                        ? new List<int>()
                        : JsonSerializer.Deserialize<List<int>>(src.AllowedUsers);
                });

            CreateMap<Department, DepartmentDTO>()
                .ForMember(d => d.CompanyName,
                    o => o.MapFrom(s => s.Company != null ? s.Company.Name : null))
                .ForMember(d => d.ManagerName,
                    o => o.MapFrom(s => s.Manager != null
                        ? s.Manager.FirstName + " " + s.Manager.LastName
                        : null))
                .ForMember(d => d.CreatedByName,
                    o => o.MapFrom(s => s.CreatedByUser != null
                        ? s.CreatedByUser.FirstName + " " + s.CreatedByUser.LastName
                        : null))
                .ForMember(d => d.UploadedByName,
                    o => o.MapFrom(s => s.UploadedByUser != null
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
                    o => o.MapFrom(s => s.CreatedByUser != null
                        ? s.CreatedByUser.FirstName + " " + s.CreatedByUser.LastName
                        : null))
                .ForMember(d => d.UploadedByName,
                    o => o.MapFrom(s => s.UploadedByUser != null
                        ? s.UploadedByUser.FirstName + " " + s.UploadedByUser.LastName
                        : null));

            CreateMap<CreatePositionDTO, Position>();
            CreateMap<UpdatePositionDTO, Position>();
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
             .ForMember(x => x.IsArchived, opt => opt.Ignore());
            CreateMap<Document, MyPendingDTO>()
            .ForMember(d => d.StatusName, o => o.MapFrom(s =>
                s.StatusId == 1 ? "Bekliyor" :
                s.StatusId == 2 ? "Onaylandı" :
                s.StatusId == 3 ? "Reddedildi" :
                "Bilinmiyor"
            ));

        }
    }
}
