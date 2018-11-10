﻿using System;
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
        public IActionResult GetAllInternships()
        {
            var claim = User.Claims.FirstOrDefault(u => u.Type.Contains("nameidentifier"));
            if(claim!=null)
            {
                var userId = claim.Value;
                try
                {
                    var companyId = _companyService.GetCompanyIdForUser(userId);
                    var internshipsDB = _internshipService.GetInternshipsForCompany(companyId);
                    var viewModels = new List<InternshipMainAttributesViewModel>();
                    foreach(var internship in internshipsDB)
                    {
                        var viewModel = new InternshipMainAttributesViewModel()
                        {
                            Id = internship.Id,
                            Description = internship.Description,
                            Places = internship.Places,
                            Topics = internship.Topics,
                            Weeks = internship.Weeks,
                            End = internship.End.Date.ToShortDateString(),
                            Start = internship.Start.Date.ToShortDateString()
                        };
                        viewModels.Add(viewModel);
                    }
                    return Ok(viewModels);

                }catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
                
            }
            return BadRequest("Compania nu a fost recunoscuta");
        }


        [HttpPost]
        [Route("add")]
        public IActionResult AddInternship(Internship internship)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                _internshipService.AddInternship(internship);
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
                var ratingDTO = _internshipService.GetInternshipRatings(id);
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