using DatabaseAccess.Data;
using DatabaseAccess.Models;
using DatabaseAccess.Repositories;
using System;

namespace DatabaseAccess.UOW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        IRepository<Student> _StudentRepository;


        public UnitOfWork()
        {
            this._context = new DbContextFactory().CreateDbContext(new string[] { });
        }

        public IRepository<Student> StudentRepository
        {
            get
            {
                if (_StudentRepository == null)
                    _StudentRepository = new Repository<Student>(_context);
                return _StudentRepository;
            }
        }










        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
