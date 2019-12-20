using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Entity.Context;
using EventManagementSystem.Entity.Model.Common;

namespace EventManagementSystem.Entity.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ModelContext _dbContext;

        public UnitOfWork(ModelContext dbContext)
        {
            Database.SetInitializer<ModelContext>(null);
            _dbContext = dbContext;
        }

        #region IUnitOfWork Members
        public IRepository<T> Repository<T>() where T : BaseEntity
        {
            return new Repository<T>(_dbContext);
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
        #endregion

        #region IDisposable Members
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }

            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
