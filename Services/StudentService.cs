using DatabaseAccess.Models;
using DatabaseAccess.UOW;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class StudentService
    {
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
			}
		}

		public void UpdateStudent(Student student)
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				uow.StudentRepository.UpdateEntity(student);
			}
		}
	}
}
