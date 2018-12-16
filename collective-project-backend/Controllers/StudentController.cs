using API.Mappers;
using API.ViewModels;
using collective_project_backend.ViewModels.AccountViewModels;
using DatabaseAccess.Models;
using Microsoft.AspNetCore.Authorization;
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
    [Route("students")]
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
		[Authorize(Roles = "Company")]
		public ActionResult<List<StudentViewModel>> GetAllStudents()
		{
			return Ok(StudentMapper.GetStudentsViewFrom(_studentService.GetAllStudents()));
		}

		[HttpGet]
		[Route("internship/{InternshipId:int}")]
		//[Authorize(Roles = "Company")]
		public ActionResult<List<StudentViewModel>> GetStudentsByInternshipId(int InternshipId)
		{
			var studentsByInternship = _studentService.GetStudentsByInternshipId(InternshipId);
			return Ok(StudentMapper.GetStudentsViewFrom(studentsByInternship));
		}

		[HttpGet]
		[Route("company/{CompanyId:int}")]
		[Authorize(Roles = "Company")]
		public ActionResult<List<StudentViewModel>> GetStudentsByCompanyId(int CompanyId)
		{
			var studentsByCompany = _studentService.GetStudentsByInternshipId(CompanyId);
			return Ok(StudentMapper.GetStudentsViewFrom(studentsByCompany));
		}

		[HttpPost]
		[Authorize(Roles = "Student")]
		public async Task<IActionResult> RegisterStudentAsync([FromBody] StudentViewModel studentView)
		{
			var student = StudentMapper.ToActualObject(studentView);
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
				return Ok();
			}
			return BadRequest();
		}

		[HttpPut]
		[Authorize(Roles = "Student")]
		public IActionResult UpdateStudent([FromBody] StudentViewModel studentView)
		{
			try
			{
				var student = StudentMapper.ToActualObject(studentView);
				_studentService.UpdateStudent(student);
			}
			catch (Exception e)
			{
				return BadRequest(e.Message);
			}
			return Ok();
		}

		[HttpPost]
		[Route("applications")]
		[Authorize(Roles = "Student")]
		public IActionResult AddApplication([FromBody] ApplicationViewModel applicationView)
		{
			try
			{
				var application = ApplicationMapper.ToActualObject(applicationView, _internshipService);
				_applicationService.AddApplication(application);
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
			return Ok();
		}

		[HttpPut]
		[Route("applications")]
		//[Authorize(Roles = "Student")]
		public IActionResult UpdateApplication([FromBody] ApplicationViewModel applicationView)
		{
			try
			{
				var application = ApplicationMapper.ToActualObject(applicationView, _internshipService);
				_applicationService.UpdateApplication(application);
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
			return Ok();
		}

		[HttpPost]
		[Route("subscriptions")]
		[Authorize(Roles = "Student")]
		public IActionResult AddSubscription([FromBody] SubscriptionViewModel subscriptionView)
		{
			try
			{
				var subscription = SubscriptionMapper.ToActualObject(subscriptionView);
				_subscriptionService.AddSubscription(subscription);
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
			return Ok();
		}

		[HttpPut]
		[Route("subscriptions")]
		[Authorize(Roles = "Student")]
		public IActionResult UpdateSubscription([FromBody] SubscriptionViewModel subscriptionView)
		{
			try
			{
				var subscription = SubscriptionMapper.ToActualObject(subscriptionView);
				_subscriptionService.UpdateSubscription(subscription);
			}
			catch (Exception e)
			{
				return BadRequest(e);
			}
			return Ok();
		}

		[HttpGet]
		[Route("applications/{StudentId:int}/{InternshipId:int}")]
		//[Authorize(Roles = "Company")]
		public ActionResult<bool> ExistsApplication(int StudentId, int InternshipId)
		{
			return Ok(_applicationService.ExistsApplication(StudentId, InternshipId));
		}

		[HttpGet]
		[Route("ratings/{StudentId:int}/{InternshipId:int}")]
		//[Authorize(Roles = "Company")]
		public ActionResult<bool> ExistsRating(int StudentId, int InternshipId)
		{
			return Ok(_ratingService.ExistsRating(StudentId, InternshipId));
		}

        [HttpGet]
        [Route("{id}/cv")]
        public IActionResult GetCV(int id)
        {
            var student = _studentService.GetStudentById(id);
            if (student == null)
                return BadRequest("Studentul nu exista");
            Stream stream = new MemoryStream(student.Cv);
            var resultStream = new FileStreamResult(stream, "application/pdf");
            var cv = new CvViewModel() { Id = student.Id, Cv = student.Cv };

            return Ok(JsonConvert.SerializeObject(cv));
        }
    }
}