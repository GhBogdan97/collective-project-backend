using DatabaseAccess.UOW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services
{
    public class CompanyService
    {
        public int GetCompanyIdForUser(string idUser)
        {
            using (UnitOfWork uow = new UnitOfWork())
            {
                var company=uow.CompanyRepository.getDbSet().Where(c => c.IdUser == idUser).FirstOrDefault();
                if (company == null)
                {
                    throw new Exception("Nu exista companie pentru acest user");
                }
                else
                {
                    return company.Id;
                }
            }
        }
    }
}
