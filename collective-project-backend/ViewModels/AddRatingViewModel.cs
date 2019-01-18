using System.ComponentModel.DataAnnotations;

namespace API.ViewModels
{
    public class AddRatingViewModel
    {
        [Range(0, 5)]
        public int RatingInternship { get; set; }
        [Range(0, 5)]
        public int RatingCompany { get; set; }
        [Range(0, 5)]
        public int RatingMentors { get; set; }
    }
}
