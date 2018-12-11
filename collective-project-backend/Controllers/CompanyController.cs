using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DatabaseAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace API.Controllers
{
    [Route("companies")]
    [ApiController]
    public class CompanyController : ControllerBase
	{
		private readonly IEmailSender _emailSender;
		private readonly CompanyService _companyService;

        public CompanyController(CompanyService companyService, IEmailSender emailSender)
        {
            _companyService = companyService;
			_emailSender = emailSender;
        }

		public async Task SendEmailAction()
		{
			await _emailSender.SendEmailAsync("andreea_ciforac@yahoo.com", "subject",
						 $"Enter email body here");
		}

		[HttpGet]
        public ActionResult<IList<Company>> GetAllCompanies()
        {
            return Ok(_companyService.GetAllCompanies());
        }

        [HttpPost]
        [Route("update")]
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
