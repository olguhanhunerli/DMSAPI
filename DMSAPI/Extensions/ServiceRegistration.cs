using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Business.Repositories;
using DMSAPI.Entities.Models;
using DMSAPI.Services.IServices;
using DMSAPI.Services.Mapping;
using DMSAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.IdentityModel.JsonWebTokens;

public static class ServiceRegistration
{
	public static IServiceCollection AddApplicationService(
		this IServiceCollection services,
		IConfiguration configuration)
	{
		services.AddDbContext<DMSDbContext>(options =>
			options.UseMySql(
				configuration.GetConnectionString("DefaultConnection"),
				new MySqlServerVersion(new Version(8, 0, 32))
			)
		);

		services.AddAutoMapper(typeof(MappingProfile).Assembly);

		services.AddHttpContextAccessor();

		services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

		services.AddScoped<IUserRepository, UserRepository>();
		services.AddScoped<IRoleRepository, RoleRepository>();
		services.AddScoped<ICompanyRepository, CompanyRepository>();
		services.AddScoped<ICategoryRepository, CategoryRepository>();
		services.AddScoped<IDocumentRepository, DocumentRepository>();
		services.AddScoped<IDepartmentRepository, DepartmentRepository>();
		services.AddScoped<IPositionRepository, PositionRepository>();

        services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<IUserService, UserService>();
		services.AddScoped<IRoleService, RoleService>();
		services.AddScoped<ICompanyService, CompanyService>();
		services.AddScoped<ICategoryServices, CategoryServices>();
		services.AddScoped<IDocumentService, DocumentService>();
		services.AddScoped<IDepartmentService, DepartmentService>();
		services.AddScoped<IPositionService, PositionService>();

        services.AddScoped<IDocumentAttachmentRepository, DocumentAttachmentRepository>();
        services.AddScoped<IDocumentAttachmentService, DocumentAttachmentService>();

        services.AddScoped<IDocumentVersionRepository, DocumentVersionRepository>();
        services.AddScoped<IDocumentVersionService, DocumentVersionService>();

        services.AddScoped<IDocumentApprovalHistoryRepository, DocumentApprovalHistoryRepository>();
        services.AddScoped<IDocumentApprovalHistoryService, DocumentApprovalHistoryService>();

        services.AddScoped<IDocumentApprovalRepository, DocumentApprovalRepository>();
        services.AddScoped<IDocumentApprovalService, DocumentApprovalService>();

        services.AddScoped<IDocumentAccessLogRepository, DocumentAccessLogRepository>();
        services.AddScoped<IDocumentAccessLogService, DocumentAccessLogService>();

		services.AddScoped<IGlobalSearchRepository, GlobalSearchRepository>();
		services.AddScoped<IGlobalSearchService, GlobalSearchService>();

        services.AddSingleton<ITokenService, TokenService>();

		services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
		var jwt = configuration.GetSection("JwtSettings").Get<JwtSettings>();
		var key = Encoding.UTF8.GetBytes(jwt.Secret);

		services.AddAuthentication(options =>
		{
			options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
		})
		.AddJwtBearer(options =>
		{
			options.RequireHttpsMetadata = true;
			options.SaveToken = true;

			options.MapInboundClaims = false;

			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,

				ValidIssuer = jwt.Issuer,
				ValidAudience = jwt.Audience,
				IssuerSigningKey = new SymmetricSecurityKey(key),

				ClockSkew = TimeSpan.Zero,

				NameClaimType = JwtRegisteredClaimNames.Sub,
				RoleClaimType = "role"
			};
		});

		return services;
	}
}
