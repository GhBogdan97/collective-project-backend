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
		IRepository<Application> _ApplicationRepository;
		IRepository<Internship> _InternshipRepository;
		IRepository<Subscription> _SubscriptionRepository;
		
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

		public IRepository<Application> ApplicationRepository {
			get {
				if (_ApplicationRepository == null)
					_ApplicationRepository = new Repository<Application>(_context);
				return _ApplicationRepository;
			}
		}

		public IRepository<Internship> InternshipRepository {
			get {
				if (_InternshipRepository == null)
					_InternshipRepository = new Repository<Internship>(_context);
				return _InternshipRepository;
			}
		}

		public IRepository<Subscription> SubscriptionRepository {
			get {
				if (_SubscriptionRepository == null)
					_SubscriptionRepository = new Repository<Subscription>(_context);
				return _SubscriptionRepository;
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
