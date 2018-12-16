﻿using API.ViewModels;
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
        [Route("evolution")]
        public ActionResult<IList<ApplicationsPerYearViewModel>> GetApplicationsPerYear()
        {

            var userID = User.GetUserId();
            if (userID == string.Empty)
                return BadRequest("Compania nu a fost recunoscuta");
            
            var applicationsPerYear = new List<ApplicationsPerYearViewModel>();
            IList<Internship> internships = null;

            try
            {
                var id = _companyService.GetCompanyIdForUser(userID);
                internships = _internshipService.GetInternshipsForCompany(id);
                var years = internships.Select(i => i.Start.Year).Distinct().ToList();
                foreach (var year in years)
                {
                    int nrApplications = _statisticsService.GetNrApplicationsPerYear(id, year);
                    ApplicationsPerYearViewModel viewModel = new ApplicationsPerYearViewModel()
                    {
                        Year = year,
                        NumberOfStudents = nrApplications
                    };
                    applicationsPerYear.Add(viewModel);
                };

                return Ok(applicationsPerYear);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

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
