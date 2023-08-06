using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Digital.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetAllAsNoTracking(Expression<Func<T, bool>> predicate);


        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);

        int Count();

        int Count(Expression<Func<T, bool>> predicate);

        T Get(Expression<Func<T, bool>> predicate);


        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);
        void DeleteById(int id);
        T GetById(int id);
        bool Any(Expression<Func<T, bool>> predicate);

    }

}
