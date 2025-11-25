using DMSAPI.Business.Repositories.GenericRepository;
using DMSAPI.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMSAPI.Business.Repositories.IRepositories
{
    public interface IUserRepository: IGenericRepository<User>
    {
        Task<User> GetUserByEmailWithRoleAndPermissionsAsync(string email);
    }
}
