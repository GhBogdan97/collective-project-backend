using API.ViewModels;
using DatabaseAccess.Models;

namespace API.Mappers
{
    public static class ApplicationMapper
    {
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
