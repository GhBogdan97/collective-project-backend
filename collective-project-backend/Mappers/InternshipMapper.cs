using API.ViewModels;
using DatabaseAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Mappers
{
    public static class InternshipMapper
    {
        public static InternshipMainAttributesViewModel ToViewModel(Internship internship)
        {
            return new InternshipMainAttributesViewModel()
            {
                Id = internship.Id,
                Description = internship.Description,
                Places = internship.Places,
                Topics = internship.Topics,
                Weeks = internship.Weeks,
                End = internship.End.Date.ToShortDateString(),
                Start = internship.Start.Date.ToShortDateString()
            };
        }

        public static Internship ToActualInternshipObject(InternshipMainAttributesViewModel internshipView)
        {
            return new Internship()
            {
                Description = internshipView.Description,
                Places = internshipView.Places,
                Topics = internshipView.Topics,
                Start = DateTime.Parse(internshipView.Start),
                End = DateTime.Parse(internshipView.End),
                Weeks = internshipView.Weeks,
                Name = internshipView.Name
            };
        }
    }
}