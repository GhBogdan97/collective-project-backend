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

		public void AddSubscription(Subscription subscription)
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				if (uow.StudentRepository.GetById(subscription.StudentId) == null)
				{
					throw new Exception("There is no student with id = " + subscription.StudentId);
				}
				if (uow.CompanyRepository.GetById(subscription.CompanyId) == null)
				{
					throw new Exception("There is no company with id = " + subscription.CompanyId);
				}
				uow.SubscriptionRepository.AddEntity(subscription);
				uow.Save();
			}
		}

		public void UpdateSubscription(Subscription subscription)
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				if (uow.StudentRepository.GetById(subscription.StudentId) == null
					|| uow.CompanyRepository.GetById(subscription.CompanyId) == null)
				{
					throw new Exception("There is no subscription with student's id = " + subscription.StudentId
						+ " and with company's id = " + subscription.CompanyId);
				}
				uow.SubscriptionRepository.UpdateEntity(subscription);
				uow.Save();
			}
		}
	}
}
