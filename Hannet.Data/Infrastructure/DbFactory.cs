using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannet.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private HannetDbContext dbContext;

        public HannetDbContext Init()
        {
            return dbContext ?? (dbContext = new HannetDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
