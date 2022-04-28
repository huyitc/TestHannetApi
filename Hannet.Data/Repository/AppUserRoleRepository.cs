using Hannet.Data.Infrastructure;
using Hannet.Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Data.Repository
{
    public interface IAppUserRoleRepository : IRepository<AppUser_Role>
    {
        List<string> GetListRoleCheck(string userName);
        Task<List<string>> GetListRole(string userName);
    }
    public class AppUserRoleRepository :RepositoryBase<AppUser_Role>, IAppUserRoleRepository
    {
        private HannetDbContext DbContext;

        public AppUserRoleRepository(HannetDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        public async Task<List<string>> GetListRole(string userName)
        {

            return await(from r in DbContext.UserRoles
                         join ApplicationRole in DbContext.AppRoles on r.RoleId equals ApplicationRole.Id
                         where r.UserId == userName
                         select ApplicationRole.Name).ToListAsync();
        }

        public List<string> GetListRoleCheck(string userName)
        {
            var query = from r in DbContext.Roles
                        join rg in DbContext.AppRole_Groups on r.Id equals rg.RoleId
                        join ug in DbContext.AppUser_Groups on rg.GroupId equals ug.GroupId
                        join u in DbContext.Users on ug.UserId equals u.Id
                        where u.UserName == userName
                        select r.Name;
            return query.ToList();
        }
    }
}
