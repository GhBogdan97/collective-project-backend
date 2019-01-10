using API.ViewModels;
using DatabaseAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Mappers {
	public class CompanyMapper {
		public static CompanyViewModel ToViewModel(Company company, int studentId) {
			return new CompanyViewModel {
				Id = company.Id,
				Name = company.Name,
				Description = company.Description,
				Internships = company.Internships,
				Subscribed = company.Subscriptions.Any(s => s.StudentId == studentId)
			};
		}
	}
}
