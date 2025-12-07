using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Services.Common
{
    public interface ICurrentCompanyService
    {
		int CompanyId { get; }
		int UserId { get; }
	}
}
