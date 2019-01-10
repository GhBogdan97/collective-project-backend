using API.ViewModels;
using DatabaseAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Mappers
{
    public static class StudentRatingsMapper
    {

        public static Rating ToRatingDbObject(AddRatingViewModel ratingViewModel, int studentId, int internshipId)
        {
            return new Rating()
            {
                StudentId = studentId,
                InternshipId = internshipId,
                RatingCompany = ratingViewModel.RatingCompany,
                RatingInternship = ratingViewModel.RatingInternship,
                RatingMentors = ratingViewModel.RatingMentors
            };
        }

        public static Rating ToRatingWithTestimonialDbObject(AddTestimonialViewModel testimonialViewModel, int studentId, int internshipId)
        {
            return new Rating()
            {
                StudentId = studentId,
                InternshipId = internshipId,
                Testimonial = testimonialViewModel.Testimonial
            };
        }

        internal static object ToAddRatingViewModel(Rating addedRating)
        {
            return new AddRatingViewModel()
            {
                RatingCompany = addedRating.RatingCompany,
                RatingMentors = addedRating.RatingMentors,
                RatingInternship = addedRating.RatingInternship
            };
        }

        public static TestimonialViewModel ToTestimonial(Rating addedTestimonial)
        {
            return new TestimonialViewModel()
            {
                Testimonial = addedTestimonial.Testimonial,
                Date = addedTestimonial.Date.Date.ToShortDateString(),
                Firstname = addedTestimonial.Student != null ? addedTestimonial.Student.Firstname : "",
                Lastname = addedTestimonial.Student != null ? addedTestimonial.Student.Lastname : ""
            };
        }
    }
}
