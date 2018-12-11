using DatabaseAccess.Models;
using DatabaseAccess.UOW;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class ApplicationService
    {
        public IList<Application> GetApplicationsForInternship(int id)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                return uow.ApplicationsRepository
                    .getDbSet()
                    .Where(a => a.InternshipId == id)
                    .Include(a=>a.Internship)
                    .Include(a=>a.Student)
                    .ToList();             
            }
        }
    }
}
