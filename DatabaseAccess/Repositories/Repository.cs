using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DatabaseAccess.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public DbSet<TEntity> dbset;
        public DbContext _context;

        public Repository(DbContext context)
        {
            this._context = context;
            dbset = _context.Set<TEntity>();
        }

        public void AddEntity(TEntity entity)
        {
            dbset.Add(entity);
        }

        public void DeleteById(int id)
        {
            TEntity entity = GetById(id);
            dbset.Remove(entity);
        }

        public void DeleteEntity(TEntity entity)
        {
            dbset.Remove(entity);
        }

        public IList<TEntity> GetAll()
        {
            return dbset.ToList();
        }

        public IQueryable<TEntity> getDbSet()
        {
            return dbset;
        }

        public TEntity GetById(int id)
        {
            return dbset.Find(id);
        }

        public TEntity GetByFunc(Func<TEntity, bool> func)
        {
            return dbset.AsQueryable().Where(x => func(x)).FirstOrDefault();
        }

        public void UpdateEntity(TEntity entity)
        {
            dbset.Update(entity);
        }

        
    }
}
