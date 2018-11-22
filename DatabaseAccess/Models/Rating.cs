using System;
using System.ComponentModel.DataAnnotations;

namespace DatabaseAccess.Models
{
    public class Rating
    {
        public int InternshipId { get; set; }
        public Internship Internship { get; set; }
        
        public int StudentId { get; set; }
        public Student Student { get; set; }

        [Range(0,5)]
        public int RatingInternship { get; set; }
        [Range(0, 5)]
        public int RatingCompany { get; set; }
        [Range(0, 5)]
        public int RatingMentors { get; set; }

        public string Testimonial { get; set; } 

        public DateTime Date { get; set; }

    }
}
