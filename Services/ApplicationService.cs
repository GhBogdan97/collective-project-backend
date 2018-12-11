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
			using (UnitOfWork uow = new UnitOfWork())
			{
				if (uow.InternshipRepository.GetById(internshipId) == null)
				{
					throw new Exception("There is no internship with id = " + internshipId);
				}
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


		public void UpdateApplicationNew(Application application)
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				var app = uow.ApplicationRepository.getDbSet().
					Where(a => (a.StudentId == application.StudentId) && (a.InternshipId == application.InternshipId)).
					FirstOrDefault();
				if (app == null)
				{
					throw new Exception("The application doesn't exist!");
				}

				uow.ApplicationRepository.UpdateEntity(app);
				uow.Save();
			}
		}

		public void UpdateApplication(Application application)
		{
			//daca studentul a fost declarat admis final la un internship, 
			//toate celelalte aplicatii se stabilesc la respins
			using (UnitOfWork uow = new UnitOfWork())
			{
				if (uow.StudentRepository.GetById(application.StudentId) == null 
					|| uow.InternshipRepository.GetById(application.InternshipId) == null)
				{
					throw new Exception("There is no application with student's id = " + application.StudentId 
						+ " and with internship's id = " + application.InternshipId);
				}
				if (application.Status == DatabaseAccess.Enums.ApplicationStatus.ADMITTED)
				{
					foreach (Application a in uow.ApplicationRepository.GetAll())
					{
						a.Status = DatabaseAccess.Enums.ApplicationStatus.REJECTED;
						uow.ApplicationRepository.UpdateEntity(a);
					}
				}
				uow.ApplicationRepository.UpdateEntity(application);
				uow.Save();
			}
		}

		public bool ExistsApplication(int StudentId, int InternshipId)
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				var applications = uow.ApplicationRepository.GetAll();
				foreach (Application a in applications)
				{
					if (a.InternshipId == InternshipId && a.StudentId == StudentId)
					{
						return true;
					}
				}
				return false;
			}
		}
	}
}
