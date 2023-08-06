using Digital.Data.Context;
using Digital.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Digital.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DigitalEfDbContext dbContext;
        private bool disposed = false;

        public UnitOfWork(DigitalEfDbContext dbContext)
        {
            this.dbContext = dbContext;

        }
        public IRepository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(dbContext);
        }

        public int SaveChanges()
        {
            try
            {
                using (TransactionScope tScope = new TransactionScope())
                {
                    int result = dbContext.SaveChanges();
                    tScope.Complete();
                    return result;
                }
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
