﻿using DatabaseAccess.DTOs;
using DatabaseAccess.Models;
using DatabaseAccess.UOW;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class InternshipService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public InternshipService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async void UpdateInternship(Internship internship, int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var internshipDb = uow.InternshipRepository.getDbSet()
                    .Where(i => i.Id == id)
                    .Include(i => i.Applications)
                    .FirstOrDefault();

                if (internshipDb == null)
                {
                    throw new Exception("Internship inexistent");
                }
                if (internship.Start != DateTime.MinValue)
                {
                    internshipDb.Start = internship.Start;
                }
                if (internship.End != DateTime.MinValue)
                {
                    internshipDb.End = internship.End;
                }
                if (internship.Places != 0)
                {
                    internshipDb.Places = internship.Places;
                }
                if (internship.Weeks != 0)
                {
                    internshipDb.Weeks = internship.Weeks;
                }
                internshipDb.Topics = internship.Topics ?? internshipDb.Topics;
                internshipDb.Description = internship.Description ?? internshipDb.Description;
                internshipDb.Name = internship.Name ?? internshipDb.Name;
                internshipDb.OccupiedPlaces = internship.OccupiedPlaces;
                uow.InternshipRepository.UpdateEntity(internshipDb);
                uow.Save();

                var usersEmails = new List<string>();

                foreach (var application in internshipDb.Applications)
                {
                    var student = uow.StudentRepository.GetById(application.StudentId);

                    var user = _userManager.FindByIdAsync(student.IdUser).Result;

                    usersEmails.Add(user.Email);
                }

                Task emailSenderTask = new Task(new Action(() =>
                {
                    string subject = "Internship update ";
                    string message = "Internship " + internship.Description + " was updated! Check it out on by clicking the following link: http://localhost:8080/";
                    var emailSender = new EmailSender();
                    emailSender.SendEmailsAsync(usersEmails, subject, message);
                }));
                emailSenderTask.Start();
            }
        }

        public IList<Internship> GetInternshipsForCompany(int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                if (uow.CompanyRepository.GetById(id) == null)
                    throw new Exception($"Compania cu id-ul {id} nu exista");
                return uow.InternshipRepository.getDbSet().Where(i => i.CompanyId == id).ToList();
            }
        }

        public string GetCompanyNameForInternship(Internship internship)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var companyDb = uow.CompanyRepository.getDbSet()
                    .Where(c => c.Id == internship.CompanyId)
                    .FirstOrDefault();
                return companyDb.Name;
            }
        }

        public string GetStatusForStudentInternship(Internship internship, int studentId)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var applicationDb = uow.ApplicationRepository.getDbSet().Where(a => a.InternshipId == internship.Id && a.StudentId == studentId).FirstOrDefault();
                return applicationDb.Status.ToString();
            }
        }

        public IList<Internship> GetInternshipsForStudent(int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var student = uow.StudentRepository.getDbSet()
                    .Where(s => s.Id == id)
                    .Include(s => s.Applications)
                    .FirstOrDefault();

                if (uow.StudentRepository.GetById(id) == null)
                    throw new Exception($"Studentul cu id-ul {id} nu exista");

                List<Internship> internships = new List<Internship>();
                foreach (var application in student.Applications)
                {
                    var internship = uow.InternshipRepository.GetById(application.InternshipId);
                    internships.Add(internship);
                }
                return internships;
            }
        }


        public Internship AddInternship(Internship internship)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var company = uow.CompanyRepository.getDbSet()
                    .Where(c => c.Id == internship.CompanyId)
                    .Include(c => c.Subscriptions)
                    .FirstOrDefault();

                if (company == null)
                {
                    throw new Exception($"Compania cu id-ul {internship.Id} nu exista");
                }
                uow.InternshipRepository.AddEntity(internship);
                uow.Save();

                var usersEmails = new List<string>();

                foreach (var subscription in company.Subscriptions)
                {
                    var student = uow.StudentRepository.GetById(subscription.StudentId);
                    var user = _userManager.FindByIdAsync(student.IdUser).Result;
                    usersEmails.Add(user.Email);
                }

                Task emailSenderTask = new Task(new Action(() =>
                {
                    var emailSender = new EmailSender();
                    string subject = "New internship!";
                    string message = "A new internship has been added for " + company.Name + "! Check more about it here: http://localhost:8080/";
                    emailSender.SendEmailsAsync(usersEmails, subject, message);
                }));
                emailSenderTask.Start();

                return internship;
            }
        }

        public Internship GetInternshipDetails(int id)
        {
            using (var uow = new UnitOfWork())
            {
                var internship = uow.InternshipRepository.GetById(id);

                if (internship == null)
                {
                    throw new Exception("Internship-ul nu exista!");
                }

                return internship;
            }
        }

        public RatingDTO GetInternshipRatingsAverege(int id)
        {
            using (var uow = new UnitOfWork())
            {
                var ratings = uow.RatingRepository.getDbSet().Where(t => t.InternshipId == id).ToList();

                if (ratings == null)
                {
                    throw new Exception("Nu exista evaluari pentru acest internship!");
                }

                RatingDTO finalRating = new RatingDTO();

                ratings.ForEach(r =>
                {
                    finalRating.RatingCompany += r.RatingCompany;
                    finalRating.RatingInternship += r.RatingInternship;
                    finalRating.RatingMentors += r.RatingMentors;
                });

                finalRating.RatingCompany /= (float)ratings.Count;
                finalRating.RatingInternship /= (float)ratings.Count;
                finalRating.RatingMentors /= (float)ratings.Count;

                return finalRating;
            }
        }

        public IList<Rating> GetInternshipRatings(int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                return uow.RatingRepository.getDbSet()
                    .Include(r => r.Student)
                    .Where(r => r.InternshipId == id)
                    .ToList();
            }
        }

        public int CountInternships()
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                return uow.InternshipRepository.GetAll().Count();
            }
        }

        public IList<Internship> GetAllInternships()
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                return uow.InternshipRepository.GetAll();
            }
        }

        public Internship GetInternshipById(int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                return uow.InternshipRepository.getDbSet()
                    .Where(i => i.Id == id)
                    .Include(i => i.Company)
                    .FirstOrDefault();
			}
		}

        public Rating AddRating(Rating rating)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var existingRating = uow.RatingRepository.getDbSet()
                    .Where(r => r.StudentId == rating.StudentId && r.InternshipId == rating.InternshipId)
                    .FirstOrDefault();

                if(existingRating==null)
                {
                    rating.Date = DateTime.Now;
                    existingRating = rating;
                    uow.RatingRepository.AddEntity(rating);
                }
                else
                {
                    existingRating.Date = DateTime.Now;
                    existingRating.RatingCompany = rating.RatingCompany;
                    existingRating.RatingInternship = rating.RatingInternship;
                    existingRating.RatingMentors = rating.RatingMentors;
                    uow.RatingRepository.UpdateEntity(existingRating);
                }
                uow.Save();
                return existingRating;

            }
        }

        public Rating AddTestimonial(Rating rating)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var existingRating = uow.RatingRepository.getDbSet()
                    .Where(r => r.StudentId == rating.StudentId && r.InternshipId == rating.InternshipId)
                    .Include(r=>r.Student)
                    .FirstOrDefault();

                if (existingRating == null)
                {
                    rating.Date = DateTime.Now;
                    existingRating = rating;
                    uow.RatingRepository.AddEntity(rating);
                }
                else
                {
                    existingRating.Date = DateTime.Now;
                    existingRating.Testimonial = rating.Testimonial;
                    uow.RatingRepository.UpdateEntity(existingRating);
                }
                uow.Save();
                return existingRating;
            }
        }

        public bool CheckIfStudentWasParticipant(int internshipId, int studentId)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var internship = uow.InternshipRepository.getDbSet()
                    .Where(i => i.Id == internshipId)
                    .Include(i => i.Applications)
                    .FirstOrDefault();

                if (internship == null) return false;
                var application = internship.Applications.Where(a => a.StudentId == studentId).FirstOrDefault();
                if (application == null) return false;
                return internship.End <= DateTime.Now && application.Status == DatabaseAccess.Enums.ApplicationStatus.ADMIS;
            }
        }
    }
}
