using Hannet.Data.Infrastructure;
using Hannet.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Data.Repository
{
    public interface IAppUserGroupRepository: IRepository<AppUser_Group>
    {

    }
    public class AppUserGroupRepository :RepositoryBase<AppUser_Group>, IAppUserGroupRepository
    {
        private readonly HannetDbContext _dbContext;
        public AppUserGroupRepository(HannetDbContext dbFactory) : base(dbFactory)
        {
            _dbContext = dbFactory; 
        }
    }
}
