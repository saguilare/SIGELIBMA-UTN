using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace IMANA.SIGELIBMA.DAL.Repository
{
    public interface IUnitOfWork
    {
        DbContext DbContext { get; }
        void Dispose();
        IRepository<T> Repository<T>() where T : class;
        void Save();
        void SaveAsync();
    }
}