﻿using API.Mappers;
using API.ViewModels;
using DatabaseAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;


namespace API.Controllers
{
    [Route("internships")]
    [ApiController]
    public class InternshipController : ControllerBase
    {

        private readonly InternshipService _internshipService;
        private readonly CompanyService _companyService;
        private readonly PostService _postService;
        private readonly StudentService _studentService;
        private readonly ApplicationService _applicationService;

        public InternshipController(StudentService studentService,
            InternshipService internshipService,
            CompanyService companyService,
            PostService postService,
            ApplicationService applicationService)
        {
            _internshipService = internshipService;
            _companyService = companyService;
            _postService = postService;
            _studentService = studentService;
            _applicationService = applicationService;
        }

        [HttpGet]
        [Route("{id}/management")]
        //[Authorize(Roles = "Company")]
        public ActionResult<List<ApplicationForManagementViewModel>> GetStudentManagementDetails(int id)
        {
            var applications = _applicationService.GetApplicationsForInternship(id);
            var applicationManagement = new List<ApplicationForManagementViewModel>();
            foreach(var app in applications)
            {
                var appManagement = ApplicationMapper.ToApplicationManagement(app);
                applicationManagement.Add(appManagement);
            }
         
            return Ok(JsonConvert.SerializeObject(applicationManagement));
        }

        [HttpGet]
        [Authorize(Roles = "Student")]
        public ActionResult<List<InternshipMainAttributesViewModel>> GetInternshipsForStudent(int id)
        {
            var claim = User.Claims.FirstOrDefault(u => u.Type.Contains("nameidentifier"));
            if (claim != null)
            {
                var userId = claim.Value;
                try
                {
                    var studentId = _studentService.GetStudentIdForUser(userId);
                    var internshipsDb = _internshipService.GetInternshipsForStudent(studentId);
                    var viewModels = new List<InternshipMainAttributesViewModel>();
                    foreach (var internship in internshipsDb)
                    {
                        var viewModel = InternshipMapper.ToViewModel(internship);
                        viewModels.Add(viewModel);
                    }
                    return Ok(viewModels);
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return BadRequest("Studentul nu a fost recunoscut");
        }
        
        [HttpGet]
        [Route("{id:int}/posts")]
        public ActionResult<List<PostViewModel>> GetPostsForInternship(int id)
        {
            try
            {
                var posts = _postService.GetPostsForInternship(id);
                var postViewModels = new List<PostViewModel>();
                foreach (var post in posts)
                {
                    var postModel = PostMapper.ToPostViewModel(post);
                    postViewModels.Add(postModel);
                }
                return Ok(postViewModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

        [HttpGet]
        [Authorize(Roles="Company")]
        public ActionResult<List<InternshipMainAttributesViewModel>> GetAllInternships()
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
                        var viewModel = Mappers.InternshipMapper.ToViewModel(internship);
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
        [Authorize(Roles = "Company")]
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
    
        [Route("{id:int}")]
        [HttpPut]
        [Authorize(Roles = "Company")]
        public IActionResult UpdateInternship([FromBody] InternshipMainAttributesViewModel internshipView, int id)
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
    }
}