﻿using API.Mappers;
using API.ViewModels;
using collective_project_backend.ViewModels.AccountViewModels;
using DatabaseAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;
        private readonly ApplicationService _applicationService;
        private readonly InternshipService _internshipService;
        private readonly SubscriptionService _subscriptionService;
        private readonly RatingService _ratingService;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public StudentController(StudentService studentService, ApplicationService applicationService,
            InternshipService internshipService, SubscriptionService subscriptionService,
            RatingService ratingService,
            UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _studentService = studentService;
            _applicationService = applicationService;
            _internshipService = internshipService;
            _subscriptionService = subscriptionService;
            _ratingService = ratingService;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        [Route("students")]
        [Authorize(Roles = "Company")]
        public ActionResult<List<StudentViewModel>> GetAllStudents()
        {
            return Ok(StudentMapper.GetStudentsViewFrom(_studentService.GetAllStudents()));
        }

        [HttpGet]
        [Route("students/internship/{internshipId}")]
        [Authorize(Roles = "Company")]
        public ActionResult<List<StudentViewModel>> GetStudentsByInternshipId(int internshipId)
        {
            var studentsByInternship = _studentService.GetStudentsByInternshipId(internshipId);
            return Ok(StudentMapper.GetStudentsViewFrom(studentsByInternship));
        }

        [HttpGet]
        [Route("students/company/{companyId}")]
        [Authorize(Roles = "Company")]
        public ActionResult<List<StudentViewModel>> GetStudentsByCompanyId(int companyId)
        {
            var studentsByCompany = _studentService.GetStudentsByInternshipId(companyId);
            return Ok(StudentMapper.GetStudentsViewFrom(studentsByCompany));
        }

        [HttpPost]
        [Route("students/register")]
        public async Task<IActionResult> RegisterStudentAsync([FromForm] RegisterStudentViewModel studentView)
        {
            var student = StudentMapper.ToActualObjectNoId(studentView);
            RegisterViewModel registration = new RegisterViewModel
            {
                Email = studentView.Email,
                Password = studentView.Password,
                ConfirmPassword = studentView.ConfirmPassword
            };

            IdentityResult result = await _userManager.CreateAsync(new ApplicationUser { Email = registration.Email, UserName = registration.Email }, registration.Password);
            if (result.Succeeded)
            {
                var studentUserManager = await _userManager.FindByEmailAsync(registration.Email);
                var roleStudent = await _roleManager.FindByNameAsync("Student");
                await _userManager.AddToRoleAsync(studentUserManager, roleStudent.Name);
                student.IdUser = studentUserManager.Id;
                _studentService.AddStudent(student);

                if(studentView.Cv!=null)
                {
                    _studentService.UploadCV(student.IdUser, studentView.Cv);
                }

                return Ok();
            }
            return BadRequest();
        }

        [HttpPut]
        [Route("students")]
        [Authorize(Roles = "Student")]
        public IActionResult UpdateStudent([FromBody] StudentViewModel studentView)
        {
            try
            {
                var idUser = User.GetUserId();
                var studentId = _studentService.GetStudentIdForUser(idUser);
                var student = StudentMapper.ToActualObject(studentView);
                student.Id = studentId;
                _studentService.UpdateStudent(student);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }

        [HttpGet]
        [Route("student")]
        [Authorize(Roles = "Student")]
        public ActionResult<StudentViewModel> GetStudentUser()
        {
            try
            {
                var studentId = User.GetUserId();
                var student = _studentService.GetStudentByUserId(studentId);
                var studentViewModel = StudentMapper.ToViewModel(student);
                return Ok(studentViewModel);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        //[HttpPost]
        //[Route("students/applications")]
        //[Authorize(Roles = "Student")]
        //public IActionResult AddApplication([FromBody] ApplicationViewModel applicationView)
        //{
        //    try
        //    {
        //        string id = User.GetUserId();
        //        var application = ApplicationMapper.ToActualObject(applicationView, _internshipService);
        //        _applicationService.AddApplication(application);
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e);
        //    }
        //    return Ok();
        //}

        [HttpPost]
        [Route("students/applications")]
        [Authorize(Roles = "Student")]
        public ActionResult<ApplicationExistedViewModel> AddApplication([FromBody] AddApplicationViewModel applicationView)
        {
            try
            {
                string id = User.GetUserId();
                var studentId = _studentService.GetStudentByUserId(id).Id;
                var application = ApplicationMapper.ToActualObjectWithStudentId(applicationView, studentId);
                var addApplicationResponse=_applicationService.AddApplication(application);
                var applicationExisted = new ApplicationExistedViewModel() { ApplicationExisted = addApplicationResponse };
                return Ok(applicationExisted);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpPut]
        [Route("students/applications")]
        [Authorize(Roles = "Student")]
        public IActionResult UpdateApplication([FromBody] ApplicationViewModel applicationView)
        {
            try
            {
                var application = ApplicationMapper.ToActualObject(applicationView, _internshipService);
                _applicationService.RejectOtherApplications(application);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return Ok();
        }

        [HttpPost]
        [Route("students/subscriptions")]
        [Authorize(Roles = "Student")]
        public IActionResult AddSubscription([FromBody] SubscriptionViewModel subscriptionView)
        {
            var userId = User.GetUserId();
            var student = _studentService.GetStudentByUserId(userId);
            try
            {
                _subscriptionService.AddSubscription(subscriptionView.CompanyId, student.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return Ok();
        }

        [HttpPut]
        [Route("students/subscriptions")]
        [Authorize(Roles = "Student")]
        public IActionResult UpdateSubscription([FromBody] SubscriptionViewModel subscriptionView) {
            var userId = User.GetUserId();
            var student = _studentService.GetStudentByUserId(userId);
            try
            {
                _subscriptionService.DeleteSubscription(subscriptionView.CompanyId, student.Id);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
            return Ok();
        }

        [HttpGet]
        [Route("students/applications/{studentId}/{internshipId}")]
        [Authorize(Roles = "Company")]
        public ActionResult<bool> ExistsApplication(int studentId, int internshipId)
        {
            return Ok(_applicationService.ExistsApplication(studentId, internshipId));
        }

        [HttpGet]
        [Route("students/ratings/{studentId}/{internshipId}")]
        [Authorize(Roles = "Company")]
        public ActionResult<bool> ExistsRating(int studentId, int internshipId)
        {
            return Ok(_ratingService.ExistsRating(studentId, internshipId));
        }

        [HttpGet]
        [Route("students/userInfo/{currentUserId}")]
        [Authorize(Roles = "Student")]
        public async Task<ActionResult<List<StudentViewModel>>> GetCurrentUserDetails(string currentUserId)
        {
            var student = StudentMapper.ToViewModel(_studentService.GetStudentByUserId(currentUserId));
            student.Email = (await _userManager.FindByIdAsync(currentUserId)).Email;
            return Ok(student);
        }

        [HttpGet]
        [Route("students/{id}/cv")]
        [Authorize(Roles = "Student,Company")]
        public IActionResult GetCV(int id)
        {
            var student = _studentService.GetStudentById(id);
            if (student == null)
                return BadRequest("Studentul nu exista");
            Stream stream = new MemoryStream(student.Cv);
            return File(stream, "application/octet-stream");
        }

        [HttpGet]
        [Route("student/cv")]
        [Authorize(Roles = "Student")]
        public IActionResult GetUserCV()
        {
            var user = User.GetUserId();
            var student = _studentService.GetStudentByUserId(user);
            if (student == null)
                return BadRequest("Studentul nu exista");
            if(student.Cv==null)
                return BadRequest("Cv-ul nu exista");
            Stream stream = new MemoryStream(student.Cv);
            return File(stream, "application/octet-stream");
        }


        [HttpPost]
        [Route("students/cv")]
        [Authorize(Roles = "Student")]
        public IActionResult UploadCV([FromForm] UploadCvViewModel fileViewModel)
        {
            var studentId = User.GetUserId();

            try
            {
                _studentService.UploadCV(studentId, fileViewModel.Cv);

                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}