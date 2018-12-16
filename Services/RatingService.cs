using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseAccess.DTOs;
using DatabaseAccess.Models;
using DatabaseAccess.UOW;

namespace Services
{
    public class RatingService
    {
        public void SaveRating(Rating rating)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                if (uow.InternshipRepository.GetById(rating.InternshipId) == null)
                {
                    throw new Exception("Internship inexistent");
                }
                if (uow.StudentRepository.GetById(rating.StudentId) == null)
                {
                    throw new Exception("Student inexistent");
                }
                uow.RatingRepository.AddEntity(rating);
                uow.Save();
            }
        }

        public IList<Rating> GetRatingsForInternship(int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                if (uow.InternshipRepository.GetById(id) == null)
                {
                    throw new Exception("Internship inexistent");
                }
                var ratingsForInternship = new List<Rating>();
                IList<Rating> ratings = uow.RatingRepository.GetAll();
                foreach (Rating rating in ratings)
                {
                    if (rating.InternshipId == id)
                    {
                        ratingsForInternship.Add(rating);
                    }
                }
                return ratingsForInternship.OrderByDescending(x => x.Date).ToList();
            }
        }

        public void UpdateApplication(Application application)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                if (uow.StudentRepository.GetById(application.StudentId) == null
                    || uow.InternshipRepository.GetById(application.InternshipId) == null)
                {
                    throw new Exception("There is no application with student's id = " + application.StudentId
                        + " and with internship's id = " + application.InternshipId);
                }
                uow.ApplicationRepository.UpdateEntity(application);
                uow.Save();
            }
        }

        public bool ExistsRating(int StudentId, int InternshipId)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var ratings = uow.RatingRepository.GetAll();
                foreach (Rating r in ratings)
                {
                    if (r.InternshipId == InternshipId && r.StudentId == StudentId)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public RatingDTO getAverageRatings(int companyId)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var ratings = uow.RatingRepository.GetAll();
                var internships = uow.InternshipRepository.getDbSet()
                    .Where(i => i.CompanyId == companyId);
                int ratingsCount = 0;
                float avgCompany = 0;
                float avgMentors = 0;
                float avgInternship = 0;
                foreach (Rating r in ratings)
                {
                    foreach (Internship i in internships)
                    {
                        if (i.Id == r.InternshipId)
                        {
                            avgCompany += r.RatingCompany;
                            avgMentors += r.RatingMentors;
                            avgInternship += r.RatingInternship;
                            ratingsCount++;
                        }
                    }
                }
                avgCompany = avgCompany / ratingsCount;
                avgMentors = avgMentors / ratingsCount;
                avgInternship = avgInternship / ratingsCount;
                return new RatingDTO
                {
                    RatingCompany = avgCompany,
                    RatingMentors = avgMentors,
                    RatingInternship = avgInternship
                    
                };
            }
        }

        public PiechartDTO getStatisticsPiechart(int companyId)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                int participantsCount = 0;
                int applicantsCount = 0;
                var internships = uow.InternshipRepository.getDbSet()
                    .Where(i => i.CompanyId == companyId);
                var participants = uow.ApplicationRepository.getDbSet()
                    .Where(a => a.Status == DatabaseAccess.Enums.ApplicationStatus.ADMIS);
                var applicants = uow.ApplicationRepository.getDbSet()
                    .Where(a => a.Status != DatabaseAccess.Enums.ApplicationStatus.ADMIS);
                foreach (var participant in participants)
                {
                    foreach (var internship in internships)
                        if (participant.InternshipId == internship.Id)
                        {
                            participantsCount++;
                            break;
                        }
                }
                foreach (var applicant in applicants)
                {
                    foreach (var internship in internships)
                        if (applicant.InternshipId == internship.Id)
                        {
                            applicantsCount++;
                            break;
                        }
                }
                return new PiechartDTO
                {
                    ApplicantsCount = applicantsCount,
                    ParticipantsCount = participantsCount
                };
            }
        }
    }
}
