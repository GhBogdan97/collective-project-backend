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
				if (uow.StudentRepository.GetById(application.StudentId) == null)
				{
					throw new Exception("There is no student with id = " + application.StudentId);
				}
				if (uow.InternshipRepository.GetById(application.InternshipId) == null)
				{
					throw new Exception("There is no internship with id = " + application.InternshipId);
				}
				uow.ApplicationRepository.AddEntity(application);
				uow.Save();
			}
		}

		public void UpdateApplication(Application application)
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				if (uow.StudentRepository.GetById(application.StudentId) == null 
					|| uow.InternshipRepository.GetById(application.InternshipId) == null)
				{
					throw new Exception("There is no application with student's id = " + application.StudentId 
						+ " and with internship's id = " + application.InternshipId);
				}
				uow.ApplicationRepository.UpdateEntity(application);
				uow.Save();
			}
		}
	}
}
