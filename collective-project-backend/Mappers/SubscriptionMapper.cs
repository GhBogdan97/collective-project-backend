using API.ViewModels;
using DatabaseAccess.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Mappers
{
	public class SubscriptionMapper
	{
		private static CompanyService _companyService = new CompanyService();
		private static StudentService _studentService = new StudentService();

		public static SubscriptionViewModel ToViewModel(Subscription subscription)
		{
			return new SubscriptionViewModel()
			{
				CompanyId = subscription.CompanyId,
				StudentId = subscription.StudentId
			};
		}

		public static Subscription ToActualObject(SubscriptionViewModel subscriptionViewModel)
		{
			return new Subscription()
			{
				CompanyId = subscriptionViewModel.CompanyId,
				Company = _companyService.GetCompanyById(subscriptionViewModel.CompanyId),
				StudentId = subscriptionViewModel.StudentId,
				Student = _studentService.GetStudentById(subscriptionViewModel.StudentId)
			};
		}
	}
}
