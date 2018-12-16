using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DatabaseAccess.Models;
using DatabaseAccess.UOW;

namespace Services
{
	public class CompanyService
	{
		public IList<Company> GetAllCompanies()
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				return uow.CompanyRepository.GetAll();
			}
		}

		public void UpdateCompany(Company company)
		{

			using (UnitOfWork uow = new UnitOfWork())
			{
				var companyDb = uow.CompanyRepository.GetById(company.Id);

				if (companyDb == null)
				{
					throw new Exception("The company doesn't exist!");
				}

				companyDb.IdUser = company.IdUser ?? companyDb.IdUser;
				companyDb.Name = company.Name ?? companyDb.Name;
				companyDb.Url = company.Url ?? companyDb.Url;
				companyDb.Description = company.Description ?? companyDb.Description;

				if (company.Logo != null)
				{
					companyDb.Logo = company.Logo;
				}

				uow.CompanyRepository.UpdateEntity(companyDb);
				uow.Save();
			}
		}

		public int GetCompanyIdForUser(string idUser)
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				var company = uow.CompanyRepository.getDbSet().Where(c => c.IdUser == idUser).FirstOrDefault();
				if (company == null)
				{
					throw new Exception("Nu exista companie pentru acest user");
				}

				return company.Id;
			}
		}

		public int CountCompanies()
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				return uow.CompanyRepository.GetAll().Count();
			}
		}

		public Company GetCompanyById(int id)
		{
			using (UnitOfWork uow = new UnitOfWork())
			{
				return uow.CompanyRepository.GetById(id);
			}
		}
	}
}

      

     







   