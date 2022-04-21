﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Hannet.Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        Task<T> AddASync(T entity);

        Task<T> UpdateASync(T entity);

        Task<T> GetByIdAsync(int id);

        Task<T> GetByIdAsync(string id);

        Task<T> DeleteAsync(int id);

        Task<T> DeleteAsync(string id);

        Task<T> GetSingleByConditionAsync(Expression<Func<T, bool>> expression, string[] includes = null);

        Task<IEnumerable<T>> GetAllAsync(string[] includes = null);

        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate, string[] includes = null);

        Task<int> CountAsync(Expression<Func<T, bool>> where);

        Task<bool> CheckContainsAsync(Expression<Func<T, bool>> predicate);

        Task DeleteMulti(Expression<Func<T, bool>> where);
    }
}
