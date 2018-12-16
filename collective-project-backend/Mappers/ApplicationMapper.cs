using API.ViewModels;
using DatabaseAccess.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Mappers
{
	public class ApplicationMapper
	{
		private static StudentService _studentService = new StudentService();

		public static ApplicationViewModel ToViewModel(Application application)
		{
			return new ApplicationViewModel()
			{
				InternshipId = application.InternshipId,
				StudentId = application.StudentId,
				Status = application.Status
			};
		}

		public static Application ToActualObject(ApplicationViewModel applicationViewModel, InternshipService internshipService)
		{
			return new Application()
			{
				InternshipId = applicationViewModel.InternshipId,
				Internship = internshipService.GetInternshipById(applicationViewModel.InternshipId),
				StudentId = applicationViewModel.StudentId,
				Student = _studentService.GetStudentById(applicationViewModel.StudentId),
				Status = applicationViewModel.Status
			};
		}

        public static ApplicationForManagementViewModel ToApplicationManagement(Application application)
        {
            return new ApplicationForManagementViewModel()
            {
                Id = application.StudentId,
                Fullname = application.Student.Lastname + " " + application.Student.Firstname,
                Status = application.Status.ToString()
            };
        }
    }
}
