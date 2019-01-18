using DatabaseAccess.Models;
using DatabaseAccess.UOW;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Services
{
    public class StudentService
    {
        private readonly ApplicationService _applicationService;
        private readonly SubscriptionService _subscriptionService;

        public StudentService(ApplicationService applicationService, SubscriptionService subscriptionService)
        {
            _applicationService = applicationService;
            _subscriptionService = subscriptionService;
        }

        public IList<Student> GetAllStudents()
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                return uow.StudentRepository.GetAll();
            }
        }
        public Student GetStudentById(int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                return uow.StudentRepository.getDbSet()
                    .Where(s => s.Id == id)
                    .Include(s => s.Applications)
                    .FirstOrDefault();
            }
        }

        public void AddStudent(Student student)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                uow.StudentRepository.AddEntity(student);
                uow.Save();
            }
        }

        public void UpdateStudent(Student student)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                Console.WriteLine("Id: "+student.Id);
                /*if (uow.StudentRepository.GetById(student.Id) == null)
                {
                    throw new Exception("There is no student with id = " + student.Id);
                }*/

                var studentDb = uow.StudentRepository.getDbSet()
                    .Where(i => i.Id == student.Id)
                    .FirstOrDefault();

                if (studentDb == null)
                {
                    throw new Exception("Student inexistent");
                }

                studentDb.Firstname = student.Firstname ?? studentDb.Firstname;
                studentDb.Lastname = student.Lastname ?? studentDb.Lastname;
                studentDb.University = student.University ?? studentDb.University;
                studentDb.Specialization = student.Specialization ?? studentDb.Specialization;
                studentDb.College = student.College ?? studentDb.College;
                if (student.Cv != null) studentDb.Cv = student.Cv;

                if (student.Year != studentDb.Year)
                {
                    studentDb.Year = student.Year;
                }

                uow.StudentRepository.UpdateEntity(studentDb);
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

        public Student GetStudentByUserId(string idUser)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var idUserz = idUser;
                var student = uow.StudentRepository.getDbSet().Where(s => s.IdUser.Equals(idUser)).FirstOrDefault();
                if (student == null)
                {
                    throw new Exception("Nu exista student pentru acest user");
                }
                return student;
            }
        }

        public void UploadCV(string studentId, IFormFile cv)
        {
            using(var uow = new UnitOfWork())
            using (var reader = new MemoryStream())
            {
                var student = GetStudentByUserId(studentId);
                
                cv.CopyTo(reader);
                var fileBytes = reader.ToArray();

                student.Cv = fileBytes;

                uow.StudentRepository.UpdateEntity(student);
                uow.Save();

            }
        }
    }
}
