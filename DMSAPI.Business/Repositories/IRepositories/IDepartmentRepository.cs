using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories.IRepositories
{
    public interface IDepartmentRepository: IGenericRepository<Department>
    {
        Task<IEnumerable<Department>> GetDepartmentsByCompanyAsync(int companyId);
        Task<IEnumerable<Department>> GetAllDepartmentsAsync();
        Task<Department> GetDepartmentDetailAsync(int id);

        Task<bool> IsNameExistAsync(string name, int companyId);
        Task<bool> IsCodeExistAsync(string code, int companyId);
    }
}
