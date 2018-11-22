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
        public void UpdateInternship(Internship internship, int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var internshipDb = uow.InternshipRepository.GetById(id);
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


        public void AddInternship(Internship internship)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                if (uow.CompanyRepository.GetById(internship.CompanyId) == null)
                {
                    throw new Exception("There is no company with id = " + internship.CompanyId);
                }
                uow.InternshipRepository.AddEntity(internship);
                uow.Save();
            }
        }

        public IList<Rating> GetInternshipRatings(int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                return uow.RatingRepository.getDbSet()
                    .Include(r=>r.Student)
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
    }

		public IList<Internship> GetAllInternships()
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				return uow.InternshipRepository.GetAll();
			}
		}
	}
}
