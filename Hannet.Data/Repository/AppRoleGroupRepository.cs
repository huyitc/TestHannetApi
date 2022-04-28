using Hannet.Data.Infrastructure;
using Hannet.Model.MappingModels;
using Hannet.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Data.Repository
{
    public interface IAppRoleGroupRepository : IRepository<AppRole_Group>
    {
        IEnumerable<AppGroup_AppRole_Mapping> GetListByGroupId(int groupId);
    }
    public class AppRoleGroupRepository : RepositoryBase<AppRole_Group>, IAppRoleGroupRepository
    {
        private HannetDbContext DbContext;
        public AppRoleGroupRepository(HannetDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        public IEnumerable<AppGroup_AppRole_Mapping> GetListByGroupId(int groupId)
        {
            var query = from ag in DbContext.AppGroups
                        join agr in DbContext.AppRole_Groups on ag.GroupId equals agr.GroupId
                        join ar in DbContext.AppRoles on agr.RoleId equals ar.Id
                        where agr.GroupId == groupId
                        select new AppGroup_AppRole_Mapping
                        {
                            GroupId = ag.GroupId,
                            RoleId = ar.Id
                        };
            return query;
        }
    }
}
