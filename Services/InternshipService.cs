using DatabaseAccess.Models;
using DatabaseAccess.UOW;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services
{
    public class InternshipService
    {
        public IList<Internship> GetAllInternships()
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                return uow.InternshipRepository.GetAll();
            }
        }
    }
}
