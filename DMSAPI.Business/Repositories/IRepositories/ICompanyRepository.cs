using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.Models;

namespace DMSAPI.Business.Repositories.IRepositories
{
	public interface ICompanyRepository : IGenericRepository<Company>
	{
		Task<IEnumerable<Company>> GetAllCompaniesAsync();
	}
}
