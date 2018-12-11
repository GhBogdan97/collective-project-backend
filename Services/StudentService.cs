using DatabaseAccess.Models;
using DatabaseAccess.UOW;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class StudentService
    {
		private ApplicationService _applicationService = new ApplicationService();
		private SubscriptionService _subscriptionService = new SubscriptionService();


		public IList<Student> GetAllStudents()
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                return uow.StudentRepository.GetAll();
            }
		}

		public void AddStudent(Student student) {
			using (UnitOfWork uow = new UnitOfWork()) {
				uow.StudentRepository.AddEntity(student);
				uow.Save();
			}
		}

		public void UpdateStudent(Student student)
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				if (uow.StudentRepository.GetById(student.Id) == null)
				{
					throw new Exception("There is no student with id = " + student.Id);
				}
				uow.StudentRepository.UpdateEntity(student);
				uow.Save();
			}
		}

		public IList<Student> GetStudentsByInternshipId(int InternshipId)
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				if (uow.InternshipRepository.GetById(InternshipId) == null)
				{
					throw new Exception("There is no internship with id = " + InternshipId);
				}
			}
			var studentIds = _applicationService.GetStudentIdsByInternshipId(InternshipId);
			IList<Student> studentsByInternshipId = new List<Student>();
			foreach (Student student in GetAllStudents())
			{
				for (int i = 0; i < studentIds.Count(); i++)
				{
					if (student.Id == studentIds[i])
					{
						studentsByInternshipId.Add(student);
					}
				}
			}
			return studentsByInternshipId;
		}

		public IList<Student> GetStudentsByCompanyId(int CompanyId)
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				if (uow.CompanyRepository.GetById(CompanyId) == null)
				{
					throw new Exception("There is no company with id = " + CompanyId);
				}
			}
			var studentIds = _subscriptionService.GetStudentIdsByCompanyId(CompanyId);
			IList<Student> studentsByCompanyId = new List<Student>();
			foreach (Student student in GetAllStudents())
			{
				for (int i = 0; i < studentIds.Count(); i++)
				{
					if (student.Id == studentIds[i])
					{
						studentsByCompanyId.Add(student);
					}
				}
			}
			return studentsByCompanyId;
		}

		public Student GetStudentById(int id)
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				var student = uow.StudentRepository.GetById(id);
				if (student == null)
				{
					throw new Exception("There is no student with id = " + id);
				}
				return student;
			}
		}

		public int CountStudents()
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				return uow.StudentRepository.GetAll().Count();
			}
		}

		public int GetStudentIdForUser(string idUser)
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				var student = uow.StudentRepository.getDbSet().Where(s => s.IdUser == idUser).FirstOrDefault();
				if (student == null)
				{
					throw new Exception("Nu exista student pentru acest user");
				}

				return student.Id;
			}
		}
	}
}
