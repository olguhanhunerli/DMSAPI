using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Presentation.Controller
{
    [ApiController]
    public abstract class BaseApiController: ControllerBase
    {
		protected int UserId =>
			int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)!.Value);

		protected int CompanyId =>
			int.Parse(User.FindFirst("companyId")!.Value);

		protected bool IsGlobalAdmin =>
			User.FindFirst("role")?.Value == "GLOBAL_ADMIN";
        protected int RoleId =>
            int.Parse(User.FindFirst("roleId")!.Value);

        protected int DepartmentId =>
            int.Parse(User.FindFirst("departmentId")!.Value);
    }
}
