using API.ViewModels;
using DatabaseAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Mappers
{
    public static class InternshipAddMapper
    {
        public static Internship ToInternship(InternshipAddViewModel internshipAddViewModel,int companyId)
        {
            bool startConversionSucceeded = DateTime.TryParse(internshipAddViewModel.Start,out DateTime start);
            bool endConversionSucceeded = DateTime.TryParse(internshipAddViewModel.End,out DateTime end);

            if(!startConversionSucceeded || !endConversionSucceeded)
            {
                throw new Exception("Data de start sau end nu e corecta.");
            }

            return new Internship
            {
                Description = internshipAddViewModel.Description,
                CompanyId = companyId,
                Places = internshipAddViewModel.Places,
                Start = start,
                End = end,
                Weeks = internshipAddViewModel.Weeks,
                Topics = internshipAddViewModel.Topics
            };
        }
    }
}
