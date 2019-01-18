using API.ViewModels;
using DatabaseAccess.Enums;
using DatabaseAccess.Models;
using Services;
using System;

namespace API.Mappers
{
    public class ApplicationMapper
	{

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
				StudentId = applicationViewModel.StudentId,
				Status = applicationViewModel.Status
			};
		}

        public static Application ToActualObjectWithStudentId(AddApplicationViewModel applicationViewModel, int studentId)
        {
            ApplicationStatus status = ApplicationStatus.APLICAT;
            Enum.TryParse(applicationViewModel.Status, out status);

            return new Application()
            {
                InternshipId = applicationViewModel.InternshipId,
                StudentId = studentId,
                Status =  status
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
