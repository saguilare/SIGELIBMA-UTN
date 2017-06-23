using System;
using System.Collections;
using System.Data.Entity;

namespace IMANA.SIGELIBMA.DAL.Repository
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private bool disposed = false;
        private readonly DbContext dbContext;
        private Hashtable repositories;

        public UnitOfWork(DbContext dbContext)
        {
            if (dbContext == null)
            {
                throw new ArgumentNullException("dbContext", "An instance of DbContext is required to use this Unit Of Work.");
            }

            this.dbContext = dbContext;
            this.dbContext.Configuration.LazyLoadingEnabled = true;
        }

        public DbContext DbContext
        {
            get
            {
                return this.dbContext;
            }
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public void SaveAsync()
        {
            dbContext.SaveChangesAsync();
        }

        public IRepository<T> Repository<T>() where T : class
        {
            if (repositories == null)
                repositories = new Hashtable();

            var type = typeof(T).Name;

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(Repository<>);

                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), dbContext);

                repositories.Add(type, repositoryInstance);
            }

            return (IRepository<T>)repositories[type];
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

