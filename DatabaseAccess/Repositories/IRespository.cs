using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseAccess.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IList<TEntity> GetAll();

        TEntity GetById(int id);

        TEntity GetByFunc(Func<TEntity, bool> func);

        void AddEntity(TEntity entity);

        void DeleteById(int id);

        void DeleteEntity(TEntity entity);

        void UpdateEntity(TEntity entity);
    }
}
