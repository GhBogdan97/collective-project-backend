using API.ViewModels;
using DatabaseAccess.Models;
using System.Collections.Generic;

namespace API.Mappers
{
	public static class StudentMapper
	{
		public static IList<StudentViewModel> GetStudentsViewFrom(IList<Student> students)
		{
			var studentsView = new List<StudentViewModel>();
			foreach (var student in students)
			{
				studentsView.Add(ToViewModel(student));
			}
			return studentsView;
		}

		public static IList<Student> GetStudentsFrom(IList<StudentViewModel> studentsView)
		{
			var students = new List<Student>();
			foreach (var student in studentsView)
			{
				students.Add(ToActualObject(student));
			}
			return students;
		}

		public static StudentViewModel ToViewModel(Student student)
		{
			return new StudentViewModel()
			{
				Firstname = student.Firstname,
				Lastname = student.Lastname,
				University = student.University,
				Specialization = student.Specialization,
				College = student.College,
				Year = student.Year
			};
		}

		public static Student ToActualObject(StudentViewModel studentViewModel)
		{
            return new Student()
            {
                Id = studentViewModel.Id,
				Firstname = studentViewModel.Firstname,
				Lastname = studentViewModel.Lastname,
				University = studentViewModel.University,
				Specialization = studentViewModel.Specialization,
				College = studentViewModel.College,
				Year = studentViewModel.Year
			};
		}

        public static Student ToActualObjectNoId(RegisterStudentViewModel studentViewModel)
        {
            return new Student()
            {
                Firstname = studentViewModel.Firstname,
                Lastname = studentViewModel.Lastname,
                University = studentViewModel.University,
                Specialization = studentViewModel.Specialization,
                College = studentViewModel.College,
                Year = studentViewModel.Year,
            };
        }
    }
}
