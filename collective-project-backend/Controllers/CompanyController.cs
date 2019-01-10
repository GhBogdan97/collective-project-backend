using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Mappers;
using API.ViewModels;
using DatabaseAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace API.Controllers
{
   
    [ApiController]
    public class CompanyController : ControllerBase
	{
		private readonly IEmailSender _emailSender;
		private readonly CompanyService _companyService;
		private readonly StudentService _studentService;

		public CompanyController(CompanyService companyService, StudentService studentService,IEmailSender emailSender)
        {
            _companyService = companyService;
			_studentService = studentService;
			_emailSender = emailSender;
        }

		[HttpGet]
        [Route("companies")]
        public ActionResult<IList<Company>> GetAllCompanies()
        {
            return Ok(_companyService.GetAllCompanies());
        }

		[HttpGet]
		[Route("companies/subscriptions")]
		public ActionResult<IList<CompanyViewModel>> GetAllCompaniesWithSubscription() {
			var userId = User.GetUserId();
			var studentId = _studentService.GetStudentIdForUser(userId);
			var allCompanies = _companyService.GetAllCompanies();
			var viewModels = allCompanies.Select(c => CompanyMapper.ToViewModel(c, studentId)).ToList();
			return Ok(viewModels);
		}

		[HttpPost]
        [Route("companies")]
        public IActionResult UpdateCompany(Company company)
        {
            try {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _companyService.UpdateCompany(company);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
