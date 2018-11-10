using API.ViewModels;
using DatabaseAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("statistics")]
    [ApiController]
   
    public class StatisticsController: ControllerBase
    {
        private readonly StatisticsService _statisticsService;
        private readonly InternshipService _internshipService;
        private readonly CompanyService _companyService;

        public StatisticsController(StatisticsService statisticsService, InternshipService internshipService, CompanyService companyService)
        {
            _statisticsService = statisticsService;
            _internshipService = internshipService;
            _companyService = companyService;
        }
      
        [HttpGet]
        [Route("{id}")]
        public ActionResult<List<ApplicationsPerYearViewModel>> GetApplicationsPerYear(int id)
        {

            var claim = User.Claims.FirstOrDefault(u => u.Type.Contains("nameidentifier"));
            if (claim == null)
                return BadRequest("Compania nu a fost recunoscuta");
            
            var userId = claim.Value;
            var applicationsPerYear = new List<ApplicationsPerYearViewModel>();
            IList<Internship> internships = null;

            try
            {
                //var id = _companyService.GetCompanyIdForUser(userId);
                //internships = _internshipService.GetInternshipsForCompany(id);
                //var years = internships.Select(i => i.Start.Year).Distinct().ToList();
                //foreach (var year in years)
                //{
                //    int nrApplications = _statisticsService.GetNrApplicationsPerYear(id, year);
                //    ApplicationsPerYearViewModel viewModel = new ApplicationsPerYearViewModel()
                //    {
                //        Year = year,
                //        NumberOfStudents = nrApplications
                //    };
                //    applicationsPerYear.Add(viewModel);
                //};

                return Ok(applicationsPerYear);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
