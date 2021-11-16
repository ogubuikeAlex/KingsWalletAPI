using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace KingsWalletAPI.Data.Interfaces
{
    public interface IRepository<T>
    {
        T Add(T obj);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> obj);
        IEnumerable<T> AddRange(IEnumerable<T> obj);
        void Insert(T entity);
        Task<T> AddAsync(T obj);
                    
        T GetById(object id);
        Task<T> GetByIdAsync(object id);
        T GetSingleByCondition(Expression<Func<T, bool>> predicate = null, Func<IQueryable, IOrderedQueryable> orderby = null, params string[] includeProperties);
        IEnumerable<T> GetByCondition(Expression<Func<T, bool>> predicate = null, Func<IQueryable, IOrderedQueryable> orderby = null, int? skip = null, int? take = null, params string[] includeProperties);
        IEnumerable<T> GetAll();

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null);
        bool Any(Expression<Func<T, bool>> predicate = null);
        //ReturnModel SoftDelete(Guid Id);
    }
}
