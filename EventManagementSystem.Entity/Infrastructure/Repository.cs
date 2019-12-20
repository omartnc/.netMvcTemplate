using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Entity.Context;
using EventManagementSystem.Entity.Model.Common;

namespace EventManagementSystem.Entity.Infrastructure
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private DbContext _dbContext;
        private DbSet<T> _dbSet;

        public Repository(ModelContext modelContext)
        {
            _dbContext = modelContext;
            _dbSet = modelContext.Set<T>();
        }
        public void ChangeContext(ModelContext modelContext)
        {
            _dbContext = modelContext;
            _dbSet = modelContext.Set<T>();
        }

        #region IRepository Members
        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate);
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).FirstOrDefault();
        }

        public void AddAndWaitForCommit(T entity)
        {
            if (entity.GetType().GetProperty("CreateDate") != null)
            {
                T _entity = entity;
                _entity.GetType().GetProperty("CreateDate").SetValue(_entity, DateTime.Now);
            }
            _dbSet.Add(entity);
        }

        public void Add(T entity)
        {
            AddAndWaitForCommit(entity);
            _dbContext.SaveChanges();
        }

        public void UpdateAndWaitForCommit(T entity)
        {
            if (entity.GetType().GetProperty("ModifyDate") != null)
            {
                T _entity = entity;
                _entity.GetType().GetProperty("ModifyDate").SetValue(_entity, DateTime.Now);
            }
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Update(T entity)
        {
            UpdateAndWaitForCommit(entity);
            _dbContext.SaveChanges();
        }




        public void DeleteAndWaitForCommit(T entity)
        {
            if (entity.GetType().GetProperty("IsDeleted") != null)
            {
                T _entity = entity;
                _entity.GetType().GetProperty("IsDeleted").SetValue(_entity, true);
                UpdateAndWaitForCommit(_entity);
            }
            else
            {
                DbEntityEntry dbEntityEntry = _dbContext.Entry(entity);
                if (dbEntityEntry.State != EntityState.Deleted)
                {
                    dbEntityEntry.State = EntityState.Deleted;
                }
                else
                {
                    _dbSet.Attach(entity);
                    _dbSet.Remove(entity);
                }
            }
        }

        public void DeleteReal(int id)
        {

            var entity = GetById(id);
            DbEntityEntry dbEntityEntry = _dbContext.Entry(entity);
            if (dbEntityEntry.State != EntityState.Deleted)
            {
                dbEntityEntry.State = EntityState.Deleted;
            }
            _dbSet.Attach(entity);
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();

        }


        public void DeleteAndWaitForCommit(int id)
        {
            var entity = GetById(id);
            if (entity == null) return;
            if (entity.GetType().GetProperty("IsDeleted") != null)
            {
                T _entity = entity;
                _entity.GetType().GetProperty("IsDeleted").SetValue(_entity, true);
                UpdateAndWaitForCommit(_entity);
            }
            else
            {
                DeleteAndWaitForCommit(entity);
            }
        }

        public void Delete(int id)
        {
            DeleteAndWaitForCommit(id);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}
