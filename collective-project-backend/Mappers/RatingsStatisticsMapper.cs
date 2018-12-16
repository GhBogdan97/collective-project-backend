using API.ViewModels;
using DatabaseAccess.DTOs;
using DatabaseAccess.Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Mappers
{
	public class RatingsStatisticsMapper
    {
		private static RatingService _ratingService = new RatingService();

		public static RatingsStatisticsViewModel ToViewModel(RatingDTO rating)
		{
			return new RatingsStatisticsViewModel()
			{
				RatingCompany = rating.RatingCompany,
				RatingMentors = rating.RatingMentors,
				RatingInternship = rating.RatingInternship
            };
		}

		public static RatingDTO ToActualObject(RatingsStatisticsViewModel ratingsViewModel)
		{
			return new RatingDTO()
			{
                RatingCompany = ratingsViewModel.RatingCompany,
                RatingMentors = ratingsViewModel.RatingMentors,
                RatingInternship = ratingsViewModel.RatingInternship
            };
		}
	}
}
