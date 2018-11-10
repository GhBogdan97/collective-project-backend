using DatabaseAccess.DTOs;
using DatabaseAccess.Models;
using DatabaseAccess.UOW;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class InternshipService
    {
        public void UpdateInternship(Internship internship)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var internshipDb = uow.InternshipRepository.GetById(internship.Id);
                if (internshipDb == null)
                {
                    throw new Exception("Internship inexistent");
                }
                if(internship.Start != DateTime.MinValue)
                {
                    internshipDb.Start = internship.Start;
                }
                if(internship.End != DateTime.MinValue)
                {
                    internshipDb.End = internship.End;
                }
                if(internship.Places != 0)
                {
                    internshipDb.Places = internship.Places;
                }
                if(internship.Weeks != 0)
                {
                    internshipDb.Weeks = internship.Weeks;
                }
                internshipDb.Topics = internship.Topics ?? internshipDb.Topics;
                internshipDb.Description = internship.Description ?? internshipDb.Description;

                uow.InternshipRepository.UpdateEntity(internshipDb);
                uow.Save();
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

        public Internship GetInternshipDetails(int id)
        {
            using (var uow = new UnitOfWork())
            {
                var internship = uow.InternshipRepository.GetById(id);

                if(internship == null)
                {
                    throw new Exception("Internship-ul nu exista!");
                }

                return internship;
            }
        }

        public RatingDTO GetInternshipRatings(int id)
        {
            using (var uow = new UnitOfWork())
            {
                var ratings = uow.RatingRepository.getDbSet().Where(t=>t.InternshipId==id).ToList();

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
    }
}
