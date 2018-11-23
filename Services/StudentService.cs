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

        public int CountStudents()
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                return uow.StudentRepository.GetAll().Count();
            }
        }
    }
}
