using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IMANA.SIGELIBMA.DAL.Repository
{
    public interface IRepository<TEntity>
     where TEntity : class
    {
        void Add(TEntity entity);
        void Attach(TEntity entity);
        void Delete(TEntity entity);
        IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity FindSingle(Expression<Func<TEntity, bool>> predicate);
        IQueryable<TEntity> GetAll();
        TEntity GetById(object id);
        Task<TEntity> GetByIdAsync(object id);
        Type GetType();
        void Update(TEntity entity);
        bool ExistsLocal(TEntity entity);
        TEntity GetLocal(TEntity entity);
        IEnumerable<TEntity> FindLocal(Func<TEntity, bool> predicate);
    }
}