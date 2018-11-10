using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Mappers;
using API.ViewModels;
using DatabaseAccess.Models;
using DatabaseAccess.UOW;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;


namespace API.Controllers
{
    [Route("internships")]
    [ApiController]
    public class InternshipController : ControllerBase
    {

        private readonly InternshipService _internshipService;
        private readonly CompanyService _companyService;

        public InternshipController(InternshipService internshipService, CompanyService companyService)
        {
            _internshipService = internshipService;
            _companyService = companyService;
        }


        [HttpGet]
        [Authorize(Roles="Company")]
        public ActionResult<List<InternshipMainAttributesViewModel>> GetAllInternships()
        {
            var userId = User.GetUserId();
            if (userId != string.Empty)
            {
                try
                {
                    var companyId = _companyService.GetCompanyIdForUser(userId);
                    var internshipsDB = _internshipService.GetInternshipsForCompany(companyId);
                    var viewModels = new List<InternshipMainAttributesViewModel>();
                    foreach (var internship in internshipsDB)
                    {
                        var viewModel = Mappers.InternshipMapper.ToViewModel(internship);
                        viewModels.Add(viewModel);
                    }
                    return Ok(viewModels);

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }

            }
            return BadRequest("Compania nu a fost recunoscuta");
        }


        [HttpPost]
        [Route("add")]
        public IActionResult AddInternship(InternshipAddViewModel internship)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var userID = User.GetUserId();
                if(userID==string.Empty)
                {
                    return BadRequest("Compania nu a fost recunoscuta");
                }
                var companyID = _companyService.GetCompanyIdForUser(userID);
                var addedInternship = _internshipService.AddInternship(InternshipAddMapper.ToInternship(internship,companyID));


                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }
    

        [HttpPut]
        public IActionResult UpdateInternship(Internship internship)
        {
            try
            {
                _internshipService.UpdateInternship(internship);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }

        [HttpGet]
        [Route("details/{id}")]
        [Authorize(Roles="Company,Student")]
        public ActionResult<InternshipDetailsRatingViewModel> GetInternshipById(int id)
        {
            try
            {
                var internship = _internshipService.GetInternshipDetails(id);
                var ratingDTO = _internshipService.GetInternshipRatingsAverege(id);
                var internshipDetailsRating = InternshipDetailsRatingMapper.ToInternshipDetailsRating(internship, ratingDTO);
                return Ok(internshipDetailsRating);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

       
    }
}