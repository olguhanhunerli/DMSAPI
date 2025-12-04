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
    public class PositionRepository: GenericRepository<Position>, IPositionRepository
    {
        public PositionRepository(DMSDbContext context) : base(context) { }

    }
}
