using AutoMapper;
using DMSAPI.Entities.DTOs;
using DMSAPI.Entities.DTOs.CategoryDTOs;
using DMSAPI.Entities.DTOs.CompanyDTOs;
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
		}
    }
}
