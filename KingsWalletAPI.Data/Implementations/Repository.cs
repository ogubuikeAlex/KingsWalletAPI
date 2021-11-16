using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using KingsWalletAPI.Data.Interfaces;
using KingsWalletAPI.Model.Entites;
using Microsoft.EntityFrameworkCore;

namespace KingsWalletAPI.Data.Implementations
{
    public class Repository<T>: IRepository<T> where T : class
    {
        private readonly DbContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public Repository(DbContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<T>();
        }

        public T Add(T obj)
        {
            _dbContext.Add(obj);
            return obj;
        }
        public IEnumerable<T> AddRange(IEnumerable<T> obj)
        {
            _dbContext.AddRange(obj);
            return obj;
        }

        public void Insert(T entity)
        {
            _dbSet.Add(entity);
        }

        public IQueryable<T> GetAllWithInclude(string incPpt)
        {
            return _dbContext.Set<T>().Include(incPpt);
        }

        public async Task<T> AddAsync(T obj)
        {
            Add(obj);

            await _dbContext.SaveChangesAsync();

            return obj;
        }
        public IEnumerable<T> GetAll()
        {
            // return only none deleted items
            return _dbSet.ToList();
        }

        public async Task<IEnumerable<T>> GetAllWithoutDeletedEntitiesAsync()
        {

            return await _dbSet.ToListAsync();

        }

        public IEnumerable<T> GetByCondition(Expression<Func<T, bool>> predicate = null, Func<IQueryable, IOrderedQueryable> orderby = null, int? skip = null, int? take = null, params string[] includeProperties)
        {
            if (predicate is null)
                return _dbSet.ToList();

            return _dbSet
                .Where(predicate)
                .ToList();
        }

        public T GetById(object id)
        {

            var entity = _dbSet.Find(id);
            
            return entity;
        }

        public async Task<T> GetByIdAsync(object id)
        {
            var entity = await _dbSet.FindAsync(id);
           
            return entity;
        }

        public T GetSingleByCondition(Expression<Func<T, bool>> predicate = null, Func<IQueryable, IOrderedQueryable> orderby = null, params string[] includeProperties)
        {
            if (predicate is null)
                return _dbSet
                    .ToList()
                    .FirstOrDefault();

            return _dbSet
                .Where(predicate)
                .FirstOrDefault();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate is null)
                return await _dbSet
                    .AnyAsync();

            return await _dbSet
                .AnyAsync(predicate);
        }

        public bool Any(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate is null)
                return _dbSet.Any();

            return _dbSet
                .Any(predicate);
        }

       /* public ReturnModel SoftDelete(Guid Id)
        {
            var entity = _dbSet.Find(Id);

            if (entity is null)
                return new ReturnModel { Success = false, Message = "Entity Not Found" };

            if (entity.IsDeleted)
                return new ReturnModel { Success = false, Message = "Entity Already deleted" };

            entity.IsDeleted = true;

            _dbContext.Entry(entity).State = EntityState.Modified;

            return new ReturnModel { Success = true, Message = "Delete Successful" };
        }*/

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> obj)
        {
            _dbContext.AddRange(obj);

            await _dbContext.SaveChangesAsync();

            return obj;
        }

    }
}
