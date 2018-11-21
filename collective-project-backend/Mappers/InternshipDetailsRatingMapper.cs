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
                Internship = new InternshipMainAttributesViewModel
                {
                    Description = internship.Description,
                    End = internship.End.Date.ToShortDateString(),
                    Start = internship.Start.Date.ToShortDateString(),
                    Places = internship.Places,
                    Id = internship.Id,
                    Topics = internship.Topics,
                    Weeks = internship.Weeks
                },
                RatingCompany = ratingDTO.RatingCompany,
                RatingInternship = ratingDTO.RatingInternship,
                RatingMentors = ratingDTO.RatingMentors
            };
        }
    }
}
