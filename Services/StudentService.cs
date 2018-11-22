using DatabaseAccess.Models;
using DatabaseAccess.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
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
	}
}
