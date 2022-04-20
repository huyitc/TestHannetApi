using System;

namespace Hannet.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        HannetDbContext Init();
    }
}
