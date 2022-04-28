using Hannet.Data.Infrastructure;
using Hannet.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Data.Repository
{
    public interface IAppRoleRepository : IRepository<AppRole>
    {
        IEnumerable<AppRole> GetListRoleByGroupId(int groupId);
    }
    public class AppRoleRepository : RepositoryBase<AppRole>, IAppRoleRepository
    {

        private HannetDbContext DbContext;
        public AppRoleRepository(HannetDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        public IEnumerable<AppRole> GetListRoleByGroupId(int groupId)
        {
            var query = from g in DbContext.AppRoles
                        join ug in DbContext.AppRole_Groups
                        on g.Id equals ug.RoleId
                        where ug.GroupId == groupId
                        select g;
            return query;
        }
    }
}
