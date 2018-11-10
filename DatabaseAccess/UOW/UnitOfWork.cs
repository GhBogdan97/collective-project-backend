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
        IRepository<Post> _PostRepository;
        IRepository<Internship> _InternshipRepository;
        IRepository<Application> _ApplicationsRepository;
        IRepository<Company> _CompanyRepository;

        public UnitOfWork()
        {
            this._context = new DbContextFactory().CreateDbContext(new string[] { });
        }

        public IRepository<Internship> InternshipRepository
        {
            get
            {
                if (_InternshipRepository == null)
                    _InternshipRepository = new Repository<Internship>(_context);
                return _InternshipRepository;
            }
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

        public IRepository<Post> PostRepository
        {
            get
            {
                if (_PostRepository == null)
                    _PostRepository = new Repository<Post>(_context);
                return _PostRepository;
            }
        }

        public IRepository<Application> ApplicationsRepository
        {
            get
            {
                if (_ApplicationsRepository == null)
                    _ApplicationsRepository = new Repository<Application>(_context);
                return _ApplicationsRepository;
            }
        }

        public IRepository<Company> CompanyRepository
        {
            get
            {
                if (_CompanyRepository == null)
                    _CompanyRepository = new Repository<Company>(_context);
                return _CompanyRepository;
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
