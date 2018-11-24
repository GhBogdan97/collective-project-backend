using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatabaseAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using DatabaseAccess.Models;
using API.ViewModels;
using API.Mappers;
using collective_project_backend.Controllers;
using Microsoft.AspNetCore.Identity;
using collective_project_backend.ViewModels.AccountViewModels;

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

		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public StudentController(StudentService studentService, ApplicationService applicationService,
			InternshipService internshipService, SubscriptionService subscriptionService,
			UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_studentService = studentService;
			_applicationService = applicationService;
			_internshipService = internshipService;
			_subscriptionService = subscriptionService;
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
		[Authorize(Roles = "Company")]
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
				var application = ApplicationMapper.ToActualObject(applicationView);
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
		[Authorize(Roles = "Student")]
		public IActionResult UpdateApplication([FromBody] ApplicationViewModel applicationView)
		{
			try
			{
				var application = ApplicationMapper.ToActualObject(applicationView);
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
	}
}