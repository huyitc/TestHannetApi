using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hannet.Data.Infrastructure
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        private HannetDbContext dbContext;

        protected RepositoryBase(HannetDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<T> AddASync(T entity)
        {
            dbContext.Set<T>().Add(entity);
            dbContext.Entry(entity).State = EntityState.Added;
            await dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> CheckContainsAsync(Expression<Func<T, bool>> predicate)
        {
            return await dbContext.Set<T>().CountAsync(predicate) > 0;
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> where)
        {
            return await dbContext.Set<T>().CountAsync(where);
        }

        public async Task<T> DeleteAsync(int id)
        {
            var entity = await dbContext.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            dbContext.Set<T>().Remove(entity);
            dbContext.Entry(entity).State = EntityState.Deleted;
            await dbContext.SaveChangesAsync();

            return entity;
        }
        public async Task<T> DeleteAsync(string id)
        {
            var entity = await dbContext.Set<T>().FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            dbContext.Set<T>().Remove(entity);
            dbContext.Entry(entity).State = EntityState.Deleted;
            await dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteMulti(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbContext.Set<T>().Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbContext.Remove(obj);
            await dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(string[] includes = null)
        {
            return await dbContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = dbContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                {
                    query = query.Include(include);
                }
                return await query.Where(predicate).AsQueryable().ToListAsync();
            }
            return await dbContext.Set<T>().Where(predicate).AsQueryable().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetByIdAsync(string id)
        {
            return await dbContext.Set<T>().FindAsync(id);
        }

        public async Task<T> GetSingleByConditionAsync(Expression<Func<T, bool>> expression, string[] includes = null)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = dbContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                    query = query.Include(include);
                return await query.FirstOrDefaultAsync(expression);
            }
            return await dbContext.Set<T>().FirstOrDefaultAsync(expression);
        }

        public async Task<T> UpdateASync(T entity)
        {
            dbContext.Set<T>().Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
