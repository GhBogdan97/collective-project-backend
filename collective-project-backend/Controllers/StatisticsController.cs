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
        private readonly StudentService _studentService;
        private readonly CompanyService _companyService;

        public StatisticsController(StatisticsService statisticsService,
                                    InternshipService internshipService,
                                    StudentService studentService,
                                    CompanyService companyService)
        {
            _statisticsService = statisticsService;
            _internshipService = internshipService;
            _studentService = studentService;
            _companyService= companyService;
        }
      
        [HttpGet]
        [Route("{id}")]
        public ActionResult<List<ApplicationsPerYearViewModel>> GetApplicationsPerYear(int id)
        {
            var applicationsPerYear = new List<ApplicationsPerYearViewModel>();
            IList<Internship> internships = null;

            try
            {
               internships = _internshipService.GetInternshipsForCompany(id);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

            var years = internships.Select(i =>  i.Start.Year).Distinct().ToList();
            foreach(var year in years)
            {
                int nrApplications=_statisticsService.GetNrApplicationsPerYear(id, year);
                ApplicationsPerYearViewModel viewModel = new ApplicationsPerYearViewModel()
                {
                    Year = year,
                    NumberOfStudent = nrApplications
                };
            applicationsPerYear.Add(viewModel);
            };
           
            return Ok(applicationsPerYear);
        }

        [HttpGet]
        [Route("general")]
        public ActionResult<GeneralStatisticsViewModel> GetGeneralStatistics()
        {
            var generalStatisticsViewModel = new GeneralStatisticsViewModel()
            {
                NumberOfCompanies = _companyService.CountCompanies(),
                NumberOfStudents = _studentService.CountStudents(),
                NumberOfInternships = _internshipService.CountInternships()
            };
            return Ok(generalStatisticsViewModel);
        }
    }
}
