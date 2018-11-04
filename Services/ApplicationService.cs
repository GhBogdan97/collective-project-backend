using DatabaseAccess.Models;
using DatabaseAccess.UOW;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Services
{
	public class ApplicationService {
		public IList<Application> GetAllApplications() {
			using (UnitOfWork uow = new UnitOfWork()) {
				return uow.ApplicationRepository.GetAll();
			}
		}

		public IList<int> GetStudentIdsByInternshipId(int internshipId) {
			using (UnitOfWork uow = new UnitOfWork()) {
				return uow.ApplicationRepository.GetAll()
					.AsQueryable()
					.Where(x => x.InternshipId==internshipId)
					.Select(x=> x.StudentId)
					.ToList();
			}
		}

		public void AddApplication(Application application)
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				uow.ApplicationRepository.AddEntity(application);
			}
		}

		public void UpdateApplication(Application application)
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				uow.ApplicationRepository.UpdateEntity(application);
			}
		}
	}
}
