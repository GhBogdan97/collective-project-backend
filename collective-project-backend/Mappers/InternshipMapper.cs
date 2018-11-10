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
    }
}
