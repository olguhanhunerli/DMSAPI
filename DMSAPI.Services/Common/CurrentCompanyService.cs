using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.Common
{
    public class CurrentCompanyService: ICurrentCompanyService
    {
		private readonly IHttpContextAccessor _httpContextAccessor;

		public CurrentCompanyService(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public int CompanyId
		{
			get
			{
				var value = _httpContextAccessor.HttpContext?
					.User.FindFirst("companyId")?.Value;

				return string.IsNullOrWhiteSpace(value) ? 0 : int.Parse(value);
			}
		}

		public int UserId
		{
			get
			{
				var value = _httpContextAccessor.HttpContext?
					.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

				return string.IsNullOrWhiteSpace(value) ? 0 : int.Parse(value);
			}
		}
	}
}
