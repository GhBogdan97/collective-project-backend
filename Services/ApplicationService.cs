using DatabaseAccess.Models;
using DatabaseAccess.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class ApplicationService {

        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

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

        public async Task SelectStudentForInternshipAsync(Student student, Internship internship)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var app = uow.ApplicationRepository.getDbSet().
                    Where(a => (a.StudentId == student.Id) && (a.InternshipId == internship.Id))
                    .FirstOrDefault();

                if (app == null)
                {
                    throw new Exception("The application doesn't exist!");
                }

                app.Status = DatabaseAccess.Enums.ApplicationStatus.CONTACTAT;
                uow.ApplicationRepository.UpdateEntity(app);
                uow.Save();

                var user = await _userManager.FindByIdAsync(student.IdUser);
                string message = "Ai fost selectat pentru internshipul <<" + internship.Topics + ">> al companiei " + internship.Company.Name + ". In scurt timp vei fi contactat de companie pentru a stabili urmatorii pasi. Felicitari!";

                var emailSenderTask = new Task(() =>
                {
                    EmailSender emailSender = new EmailSender();
                    emailSender.SendEmailAsync(user.Email, "Hai la internship!", message);
                });
                emailSenderTask.Start();
            }
        }

        public async Task ApproveStudentForInternshipAsync(Student student, Internship internship)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var app = uow.ApplicationRepository.getDbSet().
                    Where(a => (a.StudentId == student.Id) && (a.InternshipId == internship.Id))
                    .FirstOrDefault();

                if (app == null)
                {
                    throw new Exception("The application doesn't exist!");
                }
                app.Status = DatabaseAccess.Enums.ApplicationStatus.APROBAT;

                UpdateApplication(app);

                var user = await _userManager.FindByIdAsync(student.IdUser);
                string message = "Ai fost acceptat la internshipul <<" + internship.Topics + ">> al companiei " + internship.Company.Name + ". In scurt timp vei fi contactat de companie pentru a stabili urmatorii pasi. Felicitari!";

                var emailSenderTask = new Task(() =>
                {

                    EmailSender emailSender = new EmailSender();
                    emailSender.SendEmailAsync(user.Email, "Ai fost admis!", message);
                });

                emailSenderTask.Start();
            }
        }



        public async Task RejectStudentForInternshipAsync(Student student, Internship internship)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var app = uow.ApplicationRepository.getDbSet().
                    Where(a => (a.StudentId == student.Id) && (a.InternshipId == internship.Id))
                    .FirstOrDefault();

                if (app == null)
                {
                    throw new Exception("The application doesn't exist!");
                }
                app.Status = DatabaseAccess.Enums.ApplicationStatus.RESPINS;
                UpdateApplication(app);

                var user = await _userManager.FindByIdAsync(student.IdUser);
                string message = "Ne pare rau, dar ai fost respins la internshipul <<" + internship.Topics + ">> al companiei " + internship.Company.Name + ". Multumim pentru participare!";

                var emailSenderTask = new Task(() =>
                {
                    EmailSender emailSender = new EmailSender();
                    emailSender.SendEmailAsync(user.Email, "Te mai asteptam", message);
                });
                emailSenderTask.Start();
            }
        }

        public void RejectInternshipForStudent(int idStudent, int idInternship)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var app = uow.ApplicationRepository.getDbSet().
                    Where(a => (a.StudentId == idStudent) && (a.InternshipId == idInternship))
                    .FirstOrDefault();

                if (app == null)
                {
                    throw new Exception("The application doesn't exist!");
                }

                app.Status = DatabaseAccess.Enums.ApplicationStatus.RESPINS;
                UpdateApplication(app);

            }
        }

        public void ConfirmInternshipExamAttendanceForStudent(int idStudent, int idInternship)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var app = uow.ApplicationRepository.getDbSet().
                    Where(a => (a.StudentId == idStudent) && (a.InternshipId == idInternship))
                    .FirstOrDefault();

                if (app == null)
                {
                    throw new Exception("The application doesn't exist!");
                }

                app.Status = DatabaseAccess.Enums.ApplicationStatus.EXAMINARE;
                UpdateApplication(app);

            }
        }

        public void ConfirmInternshipParticipationForStudent(int idStudent, int idInternship)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var app = uow.ApplicationRepository.getDbSet().
                    Where(a => (a.StudentId == idStudent) && (a.InternshipId == idInternship))
                    .FirstOrDefault();

                if (app == null)
                {
                    throw new Exception("The application doesn't exist!");
                }

                app.Status = DatabaseAccess.Enums.ApplicationStatus.ADMIS;
                UpdateApplication(app);

            }
        }

        public bool AddApplication(Application application)
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

                var existingApplication = uow.ApplicationRepository.getDbSet()
                    .Where(a => a.InternshipId == application.InternshipId && a.StudentId == application.StudentId)
                    .FirstOrDefault();

                if (existingApplication != null)
                    return false;

				uow.ApplicationRepository.AddEntity(application);
				uow.Save();
                return true;
			}
		}


		public void UpdateApplication(Application application)
		{
			using (UnitOfWork uow = new UnitOfWork())
            { 
				uow.ApplicationRepository.UpdateEntity(application);
				uow.Save();
			}
		}

		public void RejectOtherApplications(Application application)
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
				if (application.Status == DatabaseAccess.Enums.ApplicationStatus.ADMIS)
				{
					foreach (Application a in uow.ApplicationRepository.GetAll())
					{
                        if (a.InternshipId != application.InternshipId && a.StudentId!=application.StudentId)
                        {
                            a.Status = DatabaseAccess.Enums.ApplicationStatus.RESPINS;
                            uow.ApplicationRepository.UpdateEntity(a);
                            uow.Save();
                        } 
					}
				}
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


        public IList<Application> GetApplicationsForInternship(int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                return uow.ApplicationRepository
                    .getDbSet()
                    .Where(a => a.InternshipId == id)
                    .Include(a => a.Internship)
                    .Include(a => a.Student)
                    .ToList();
            }
        }
    }
}

