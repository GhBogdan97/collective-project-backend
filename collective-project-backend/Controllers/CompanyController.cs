using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly CompanyService _companyService;

        public CompanyController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public IActionResult GetAllCompanies()
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
