using Hannet.Data.Infrastructure;
using Hannet.Model.Models;
using System.Collections.Generic;
using System.Linq;

namespace Hannet.Data.Repository
{
    public interface IAppGroupRepository: IRepository<AppGroup>
    {
        IEnumerable<AppGroup> GetListGroupByUserId(string userId);
        IEnumerable<AppUser> GetListUserByGroupId(int groupId);
    }
    public class AppGroupRepository: RepositoryBase<AppGroup>, IAppGroupRepository
    {
        private readonly HannetDbContext DbContext;
        public AppGroupRepository(HannetDbContext dbContext) : base(dbContext)
        {
            DbContext = dbContext;
        }

        public IEnumerable<AppGroup> GetListGroupByUserId(string userId)
        {
            var query = from g in DbContext.AppGroups
                        join ug in DbContext.AppUser_Groups
                        on g.GroupId equals ug.GroupId
                        where ug.UserId == userId
                        select g;
            return query;
        }

        public IEnumerable<AppUser> GetListUserByGroupId(int groupId)
        {
            var query = from g in DbContext.AppGroups
                        join ug in DbContext.AppUser_Groups
                        on g.GroupId equals ug.GroupId
                        join u in DbContext.Users
                        on ug.UserId equals u.Id
                        where ug.GroupId == groupId
                        select u;
            return query;
        }
    }
}
