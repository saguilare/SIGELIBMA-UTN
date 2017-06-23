using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IMANA.SIGELIBMA.DAL.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly DbContext dbContext;
        private DbSet<TEntity> dbSet = null;

        public Repository(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext", "An instance of DbContext is required to use this repository.");
            }
            this.dbContext = dbContext;
            this.dbContext.Configuration.LazyLoadingEnabled = true;
            this.dbSet = this.dbContext.Set<TEntity>();
        }

        public new Type GetType()
        {
            return typeof(TEntity);
        }

        public virtual TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }

        public virtual bool ExistsLocal(TEntity entity)
        {
            return dbSet.Local.Any(e => e == entity);
        }

        public virtual IEnumerable<TEntity> FindLocal(Func<TEntity, bool> predicate)
        {
            if (null == predicate)
            {
                throw new ArgumentNullException("predicate", "Predicate value must be passed to FindLocal.");
            }

            IEnumerable<TEntity> query = dbSet.Local.Where(predicate);
            return query;
        }

        public virtual TEntity GetLocal(TEntity entity)
        {
            return dbSet.Local.Where(e => e == entity).FirstOrDefault();
        }

        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            return await dbSet.FindAsync(id);
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            return this.dbSet;
        }

        public virtual IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            if (null == predicate)
            {
                throw new ArgumentNullException("predicate", "Predicate value must be passed to FindAll<TEntity>.");
            }

            IQueryable<TEntity> query = dbSet.Where(predicate);
            return query;
        }

        public virtual TEntity FindSingle(Expression<Func<TEntity, bool>> predicate)
        {
            if (null == predicate)
            {
                throw new ArgumentNullException("predicate", "Predicate value must be passed to FindSingle<TEntity>.");
            }

            return FindAll(predicate).SingleOrDefault();
        }

        public virtual async Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (null == predicate)
            {
                throw new ArgumentNullException("predicate", "Predicate value must be passed to FindSingle<TEntity>.");
            }

            return await FindAll(predicate).SingleOrDefaultAsync();
        }

        public virtual void Add(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Entity value must be passed to Add method.");
            }

            dbSet.Add(entity);
        }

        public virtual void Attach(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Entity value must be passed to Attach method.");
            }

            dbSet.Attach(entity);
        }

        public virtual void Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Entity value must be passed to Add method.");
            }

            dbSet.Attach(entity);
            dbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "Entity value must be passed to Add method.");
            }

            if (dbContext.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }

            dbSet.Remove(entity);
        }

    }
}

