using DatabaseAccess.Models;
using DatabaseAccess.UOW;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Services
{
    public class SubscriptionService
    {
        public IList<Subscription> GetAllSubscriptions()
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                return uow.SubscriptionRepository.GetAll();
            }
		}

		public IList<int> GetStudentIdsByCompanyId(int companyId) {
			using (UnitOfWork uow = new UnitOfWork()) {
				return uow.SubscriptionRepository.GetAll()
					.AsQueryable()
					.Where(x => x.CompanyId == companyId)
					.Select(x => x.StudentId)
					.ToList();
			}
		}
	}
}
