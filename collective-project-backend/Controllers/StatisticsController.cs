using API.Mappers;
using API.ViewModels;
using DatabaseAccess.DTOs;
using DatabaseAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class StatisticsController: ControllerBase
    {
        private readonly StatisticsService _statisticsService;
        private readonly InternshipService _internshipService;
        private readonly StudentService _studentService;
        private readonly CompanyService _companyService;
        private readonly RatingService _ratingService;

        public StatisticsController(StatisticsService statisticsService,
                                    InternshipService internshipService,
                                    StudentService studentService,
                                    CompanyService companyService,
                                    RatingService ratingService)
        {
            _statisticsService = statisticsService;
            _internshipService = internshipService;
            _studentService = studentService;
            _companyService= companyService;
            _ratingService = ratingService;
        }
      
        [HttpGet]
        [Route("statistics/evolution")]
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
        [Route("statistics/general")]
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

        [HttpGet]
        [Route("statistics/ratings/{companyId}")]
        public ActionResult<RatingDTO> GetAverageRatingsCompany(int companyId)
        {
            RatingDTO ratingDTO = _ratingService.getAverageRatings(companyId);
            return Ok(ratingDTO);
        }

        [HttpGet]
        [Route("statistics/piechart/{companyId}")]
        public ActionResult<PiechartDTO> GetStatisticsPiechart(int companyId)
        {
            return Ok(_ratingService.getStatisticsPiechart(companyId));
        }

        [HttpGet]
        [Route("statistics/ratings")]
        public ActionResult<RatingDTO> GetAverageRatingsForCurrentCompany()
        {
            var companyUserId = User.GetUserId();
            var companyId= _companyService.GetCompanyIdForUser(companyUserId);
            RatingDTO ratingDTO = _ratingService.getAverageRatings(companyId);
            return Ok(ratingDTO);
        }

        [HttpGet]
        [Route("statistics/piechart")]
        public ActionResult<PiechartDTO> GetStatisticsForCurrentPiechart()
        {
            var companyUserId = User.GetUserId();
            var companyId = _companyService.GetCompanyIdForUser(companyUserId);
            return Ok(_ratingService.getStatisticsPiechart(companyId));
        }
    }
}
