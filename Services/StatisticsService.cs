using DatabaseAccess.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{

    public class StatisticsService
    {
        public int GetNrApplicationsPerYear(int companyId, int year)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                int nrApplications = 0;

                var internships = uow.InternshipRepository.getDbSet()
                    .Where(i => i.CompanyId==companyId && i.Start.Year == year)
                    .ToList();


                var applications = uow.ApplicationRepository.getDbSet();
                foreach(var internship in internships)
                {
                    var idInternship = internship.Id;
                    var nrApplicationsPerInternship=applications.Where(a => a.InternshipId == idInternship).Count();
                    nrApplications += nrApplicationsPerInternship;
                }

                return nrApplications;
            }
      
        }
       
    }
}
