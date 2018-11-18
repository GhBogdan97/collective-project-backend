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
        private readonly PostService _postService;

        public InternshipController(InternshipService internshipService, CompanyService companyService, PostService postService)
        {
            _internshipService = internshipService;
            _companyService = companyService;
            _postService = postService;
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
        [Authorize(Roles = "Company")]
        public ActionResult<InternshipMainAttributesViewModel> AddInternship(InternshipAddViewModel internship)
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


                return Ok(InternshipMapper.ToViewModel(addedInternship));
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

        }
    
        [Route("{id:int}")]
        [HttpPut]
        [Authorize(Roles = "Company")]
        public IActionResult updateInternship([FromBody] InternshipMainAttributesViewModel internshipView, int id)
        {
            try
            {
                var internship = InternshipMapper.ToActualInternshipObject(internshipView);
                _internshipService.UpdateInternship(internship, id);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }

        [Route("{id:int}/posts")]
        [HttpPost]
        [Authorize(Roles = "Company")]
        public IActionResult SavePost([FromBody] PostViewModel postView, int id)
        {
            try
            {
                var post = PostMapper.ToActualPostObject(postView, id);
                _postService.SavePost(post);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }

        [HttpGet]
        [Route("{id}/testimonials")]
        public ActionResult<TestimonialViewModel> GetTestimonialForInternship(int id)
        {
            var ratings = _internshipService.GetInternshipRatings(id);
            var testimonials = new List<TestimonialViewModel>();
            foreach(var rating in ratings)
            {
                var testimonial = new TestimonialViewModel()
                {
                    Firstname = rating.Student.Firstname,
                    Lastname = rating.Student.Lastname,
                    Testimonial = rating.Testimonial,
                    Date = rating.Date.Date.ToShortDateString()
                };
                testimonials.Add(testimonial);
            }
            return Ok(testimonials);
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