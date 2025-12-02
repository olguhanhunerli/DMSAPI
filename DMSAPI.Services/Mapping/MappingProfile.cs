using AutoMapper;
using DMSAPI.Entities.DTOs;
using DMSAPI.Entities.DTOs.CategoryDTOs;
using DMSAPI.Entities.DTOs.CompanyDTOs;
using DMSAPI.Entities.DTOs.DepartmentDTOs;
using DMSAPI.Entities.DTOs.DocumentDTOs;
using DMSAPI.Entities.DTOs.RoleDTOs;
using DMSAPI.Entities.DTOs.UserDTOs;
using DMSAPI.Entities.Models;

namespace DMSAPI.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>()
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
            .ForMember(dest => dest.CompanyId, opt => opt.MapFrom(src => src.CompanyId))
            .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
            .ForMember(dest => dest.DepartmentName,
                opt => opt.MapFrom(src =>
                    src.Department != null ? src.Department.Name : null
                ))
            .ForMember(dest => dest.ManagerName,
                opt => opt.MapFrom(src =>
                    src.Manager != null ? src.Manager.FirstName : null
                ));

            CreateMap<User, UserMiniDTO>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => src.FirstName + " " + src.LastName));
            CreateMap<User, AuthResponseDTO>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src));

            CreateMap<User, UserRegisterDTO>();
            CreateMap<Role, RoleDTO>();
            CreateMap<AddRoleDTO, Role>();
            CreateMap<UpdateRoleDTO, Role>();
            CreateMap<UpdateUserDTO, UserDTO>();
            CreateMap<UserActiveStatusDTO, UserDTO>();
            CreateMap<UserManagerDTO, UserDTO>();
            CreateMap<UserSearchDTO, UserDTO>();

			CreateMap<Company, CompanyDTO>();
			CreateMap<AddCompanyDTO, Company>();
			CreateMap<UpdateCompanyDTO, Company>();

            CreateMap<Category, CategoryDTO>();
            CreateMap<CreateCategoryDTO, Category>();

			CreateMap<Document, DocumentDTO>()
	        .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name))
	        .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
	        .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedByUser.FirstName))
	        .ForMember(dest => dest.UpdatedByName, opt => opt.MapFrom(src => src.UpdatedByUser.FirstName))
	        .ForMember(dest => dest.ApproverName, opt => opt.MapFrom(src => src.ApproverUser.FirstName))
	        .ForMember(dest => dest.ApprovedByName, opt => opt.MapFrom(src => src.ApprovedByUser.FirstName))
	        .ForMember(dest => dest.RejectedByName, opt => opt.MapFrom(src => src.RejectedByUser.FirstName))
	        .ForMember(dest => dest.DeletedByName, opt => opt.MapFrom(src => src.DeletedByUser.FirstName));

            CreateMap<Department, DepartmentDTO>()
    .       ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name));

            CreateMap<Department, DepartmentDetailDTO>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.Company.Name));

            CreateMap<CreateDepartmentDTO, Department>();
            CreateMap<UpdateDepartmentDTO, Department>();

        }
    }
}
