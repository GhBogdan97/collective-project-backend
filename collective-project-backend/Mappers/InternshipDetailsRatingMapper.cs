using API.ViewModels;
using DatabaseAccess.DTOs;
using DatabaseAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Mappers
{
    public static class InternshipDetailsRatingMapper
    {
        public static InternshipDetailsRatingViewModel ToInternshipDetailsRating(Internship internship, RatingDTO ratingDTO)
        {
            return new InternshipDetailsRatingViewModel
            {
                Internship = internship,
                RatingCompany = ratingDTO.RatingCompany,
                RatingInternship = ratingDTO.RatingInternship,
                RatingMentors = ratingDTO.RatingMentors
            };
        }
    }
}
