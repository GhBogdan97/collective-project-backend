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

		public void AddSubscription(int companyId,int studentId)
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				if (uow.StudentRepository.GetById(studentId) == null)
				{
					throw new Exception("There is no student with id = " + studentId);
				}
				if (uow.CompanyRepository.GetById(companyId) == null)
				{
					throw new Exception("There is no company with id = " + companyId);
				}
                var subscription = new Subscription() { CompanyId = companyId, StudentId = studentId };
				uow.SubscriptionRepository.AddEntity(subscription);
				uow.Save();
			}
		}

		public void DeleteSubscription(int companyId, int studentId)
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				if (uow.StudentRepository.GetById(studentId) == null
					|| uow.CompanyRepository.GetById(companyId) == null)
				{
					throw new Exception("There is no subscription with student's id = " + studentId
                        + " and with company's id = " + companyId);
				}
                var subscription = new Subscription() { CompanyId = companyId, StudentId = studentId };
                uow.SubscriptionRepository.DeleteEntity(subscription);
				uow.Save();
			}
		}
	}
}
