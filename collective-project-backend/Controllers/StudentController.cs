using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using DatabaseAccess.Models;

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

		public StudentController(StudentService studentService)
		{
			_studentService = studentService;
		}

		public StudentController(StudentService studentService, ApplicationService applicationService,
			InternshipService internshipService, SubscriptionService subscriptionService)
		{
			_studentService = studentService;
			_applicationService = applicationService;
			_internshipService = internshipService;
			_subscriptionService = subscriptionService;
		}

		[HttpGet]
		public IActionResult GetAllStudents()
		{
			return Ok(_studentService.GetAllStudents());
		}

		[HttpGet]
		public IActionResult GetStudentsByInternshipId(int InternshipId)
		{
			var studentIds = _applicationService.GetStudentIdsByInternshipId(InternshipId);
			var students = _studentService.GetAllStudents();
			IList<Student> studentsByInternshipId = new List<Student>();
			foreach (Student student in students)
			{
				for (int i = 0; i < studentIds.Count(); i++)
				{
					if (student.Id == studentIds[i])
					{
						studentsByInternshipId.Add(student);
					}
				}
			}
			return Ok(studentsByInternshipId);
		}

		[HttpGet]
		public IActionResult GetStudentsByCompanyId(int CompanyId)
		{
			var studentIds = _subscriptionService.GetStudentIdsByCompanyId(CompanyId);
			var students = _studentService.GetAllStudents();
			IList<Student> studentsByCompanyId = new List<Student>();
			foreach (Student student in students)
			{
				for (int i = 0; i < studentIds.Count(); i++)
				{
					if (student.Id == studentIds[i])
					{
						studentsByCompanyId.Add(student);
					}
				}
			}
			return Ok(studentsByCompanyId);
		}

		[HttpPost("addStudent")]
		public IActionResult AddStudent([FromBody] Student student)
		{
			_studentService.AddStudent(student);
			return Ok();
		}

		[HttpPost("updateStudent")]
		public IActionResult UpdateStudent([FromBody] Student student)
		{
			_studentService.UpdateStudent(student);
			return Ok();
		}

		[HttpPost("addApplication")]
		public IActionResult AddApplication([FromBody] Application application)
		{
			_applicationService.AddApplication(application);
			return Ok();
		}

		[HttpPost("updateApplication")]
		public IActionResult UpdateApplication([FromBody] Application application)
		{
			_applicationService.UpdateApplication(application);
			return Ok();
		}
	}
}