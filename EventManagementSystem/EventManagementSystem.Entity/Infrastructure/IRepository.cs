using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using EventManagementSystem.Entity.Context;

namespace EventManagementSystem.Entity.Infrastructure
{
    public interface IRepository<T> where T : IEntity
    {
        void ChangeContext(ModelContext modelContext);
        IQueryable<T> GetAll();
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);
        T GetById(int id);
        T Get(Expression<Func<T, bool>> predicate);
        void Add(T entity);
        void AddAndWaitForCommit(T entity);
        void Update(T entity);
        void UpdateAndWaitForCommit(T entity);
        void DeleteAndWaitForCommit(T entity);
        void DeleteAndWaitForCommit(int id);
        void Delete(int id);
        void DeleteReal(int id);
    }
}
