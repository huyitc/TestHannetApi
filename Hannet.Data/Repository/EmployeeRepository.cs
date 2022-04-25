using Hannet.Data.Infrastructure;
using Hannet.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Data.Repository
{
    public interface IEmployeeRepository : IRepository<Employee>
    {

    }
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        private readonly HannetDbContext DbContext;

        public EmployeeRepository(HannetDbContext dbContext): base(dbContext)
        {
            DbContext = dbContext;
        }
    }
}
