using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
		public ActionResult<IList<Company>> GetAllCompaniesWithSubscription() {
			var userId = User.GetUserId();
			var studentId = _studentService.GetStudentIdForUser(userId);
			return Ok(_companyService.GetCompaniesForStudent(studentId));
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
