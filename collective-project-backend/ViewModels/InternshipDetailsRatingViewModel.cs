using DatabaseAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.ViewModels
{
    public class InternshipDetailsRatingViewModel
    {
        public InternshipMainAttributesViewModel Internship { get; set; }

        public float RatingInternship { get; set; }

        public float RatingCompany { get; set; }

        public float RatingMentors { get; set; }
    }
}
