using DMSAPI.Business.Context;
using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Business.Repositories.IRepositories;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories
{
    public class CompanyRepository: GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(DMSDbContext context) : base(context) { }

        public async Task<IEnumerable<Company>> GetAllRolesAsync() => await GetAllAsync();

    }
}
