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
				StudentId = subscriptionViewModel.StudentId,
			};
		}
	}
}
