using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.ViewModels;
using DatabaseAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services;

namespace API.Controllers
{
    [Route("students")]
    [ApiController]
    [Authorize]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public ActionResult<IList<Student>> GetAllStudents()
        {
            return Ok(_studentService.GetAllStudents());
        }

        [HttpGet]
        [Route("{id}/cv")]
        public ActionResult<CvViewModel> GetCV(int id)
        {
            var student = _studentService.GetStudentById(id);
            if (student == null)
                return BadRequest("Studentul nu exista");
            var cv = new CvViewModel() { Id = student.Id, Cv = student.Cv };
            return Ok(JsonConvert.SerializeObject(cv));
        }
    }
}