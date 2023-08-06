using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Data.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext dbContext;
        private readonly DbSet<T> DbSet;

        public Repository(DbContext dbContext)
        {
            this.dbContext = dbContext;
            DbSet = dbContext.Set<T>();
        }
        public void Add(T entity)
        {
            DbSet.Add(entity);
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }

        public int Count()
        {
            IQueryable<T> iQueryable = DbSet.AsQueryable();
            return iQueryable.Count();
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> iQueryable = DbSet.Where(predicate);
            return iQueryable.Count();
        }

        public void Delete(T entity)
        {
            DbSet.Remove(entity);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> iQueryable = DbSet.Where(predicate);
            return iQueryable.ToList().FirstOrDefault();
        }

        public IQueryable<T> GetAll()
        {
            IQueryable<T> iQueryable = DbSet.AsQueryable();
            return iQueryable;
        }

        public IQueryable<T> GetAllAsNoTracking(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> iQueryable = DbSet;
            iQueryable = iQueryable.AsNoTracking().Where(predicate);
            return iQueryable;
        }


        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> iQueryable = DbSet;
            iQueryable = iQueryable.Where(predicate);
            return iQueryable;
        }
        public void DeleteById(int id)
        {
            var entity = DbSet.Find(id);
            DbSet.Remove(entity);
        }

        public T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public void Update(T entity)
        {
            DbSet.Update(entity);
        }
    }

}
