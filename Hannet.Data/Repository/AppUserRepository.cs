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
    public interface IAppUserRepository: IRepository<AppUser>
    {
        AppUser_Mapping GetUserName(string userName);

        IEnumerable<AppUser_Employee_Accout_Mapping> GetAllByMapping(string keyword);
    }
    public class AppUserRepository : RepositoryBase<AppUser>, IAppUserRepository
    {
        private HannetDbContext DbContext;

        public AppUserRepository(HannetDbContext dbFactory) : base(dbFactory)
        {
            DbContext = dbFactory;
        }

        public IEnumerable<AppUser_Employee_Accout_Mapping> GetAllByMapping(string keyword)
        {
            var query = from u in DbContext.Users
                        join e in DbContext.Employees on u.EmployeeId equals e.EmployeeId
                        select new AppUser_Employee_Accout_Mapping
                        {
                            Email = u.Email,
                            Image = e.Image,
                            EmName = e.EmployeeName,
                            UserName = u.UserName,
                            Id = u.Id,
                            EmployeeId = e.EmployeeId
                        };
            if (!string.IsNullOrEmpty(keyword))
                query = query.Where(x => x.EmName.Contains(keyword) || x.UserName.Contains(keyword));
            return query;
        }

        public AppUser_Mapping GetUserName(string userName)
        {
            var query = from a in DbContext.Users
                        where a.UserName == userName
                        select new AppUser_Mapping
                        {
                            UserName = a.UserName,
                            Id = a.Id
                        };
            return query.FirstOrDefault();
        }
    }
}
